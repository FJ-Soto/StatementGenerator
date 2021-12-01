import os
import re

from fitz import Document

from .Statement import Statement
from .Utilities import LoginFailedException, XPATH, ID, CSS, CHROMEDRIVER_PATH
from .Utilities import create_session_browser, get_default_options

from requests import Session
from datetime import datetime
from selenium.webdriver import Chrome
from selenium.common.exceptions import TimeoutException
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.remote.webelement import WebElement
from selenium.webdriver.support.expected_conditions import presence_of_element_located as pel

# standard login pages
LOGIN_PAGE = 'https://customer.xfinity.com/#/billing/brite'
LOGIN_PAGE_TITLE = 'XFINITY | My Account | EcoBill® Online Bill Pay'
ID_USER = 'user'
ID_PASS = 'passwd'
ID_SUBMIT = 'sign_in'

CSS_PDF_LINK = 'a.download-link[href]'
X_ACC_NUM = '//*[@id="bb"]/div[2]/div/div[2]/div/div[1]/div/span[2]'
X_BILL_DATE = '//*[@id="bb"]/div[1]/div/div/span[2]'

ACC_NUM_REG = r'Account Number\s+\b([0-9\s]+)'
BILL_DE_REG = r'Services From\s+(?:(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\s+\d{2},\s+\d{4}\s+to\s+)\b((?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\s+\d{2},\s+\d{4})'
BILL_DS_REG = r'Services From\s+\b((?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\s+\d{2},\s+\d{4})'
BILL_AMOUNT_REG = r'Amount due\s+\$\b([0-9,]+\.\d+)'
BILL_DD_REG = r'Automatic payment\s+\b((?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\s+\d{2},\s+\d{4})'


class XfinityStatement(Statement):
    def __init__(self, username, password, accountID=None, path=None):
        self.browser: [Chrome, None] = None
        self.session = Session()

        super(XfinityStatement, self).__init__(accountID, username, password, path=path, folder='Xfinity')

    def login(self, accountID, username, password):
        options = get_default_options()

        browser = Chrome(executable_path=CHROMEDRIVER_PATH, options=options)
        browser.set_window_size(1920, 1080)

        browser.get(LOGIN_PAGE)

        # login phase
        try:
            user: WebElement = WebDriverWait(browser, 10).until(pel((ID, ID_USER)))
            password_txt: WebElement = WebDriverWait(browser, 10).until(pel((ID, ID_PASS)))
            submit_btn: WebElement = WebDriverWait(browser, 10).until(pel((ID, ID_SUBMIT)))
        except TimeoutException as _:
            raise TimeoutException('Login element timeout.')

        user.send_keys(username)
        password_txt.send_keys(password)
        submit_btn.click()

        if browser.title != LOGIN_PAGE_TITLE:
            raise LoginFailedException()

        self.browser = browser

    def navigate_to_statement(self):
        browser = self.browser
        # attempting to use session
        try:
            pdf_link: WebElement = WebDriverWait(browser, 10).until(pel((CSS, CSS_PDF_LINK)))
            # acc_num_txt: WebElement = WebDriverWait(browser, 10).until(pel((XPATH, X_ACC_NUM)))
            # bill_date: WebElement = WebDriverWait(browser, 10).until(pel((XPATH, X_BILL_DATE)))
        except TimeoutException as _:
            raise TimeoutException('Navigation element timeout.')

        create_session_browser(self.session, browser)
        self.statement = self.session.get(pdf_link.get_attribute('href')).content

        with Document(stream=self.statement, filetype='pdf') as doc:
            text = "".join(page.get_textpage().extractText() for page in doc)

            acc_num_re = re.search(ACC_NUM_REG, text)
            acc_num = acc_num_re.group(1).strip().replace(' ', '')

            bill_de_re = re.search(BILL_DE_REG, text, flags=re.IGNORECASE)
            bill_de = datetime.strptime(bill_de_re.group(1), '%b %d, %Y')

            self.filename = os.path.join(self.base_dir, f"{bill_de:%m-%y}_Xfinity_{acc_num}.pdf")
            self.filenames[acc_num] = {'bill_de': f"{bill_de:%m/%d/%Y}",
                                       'filename': self.filename}

            if bill_dd_re := re.search(BILL_DD_REG, text):
                bill_dd = datetime.strptime(bill_dd_re.group(1), '%b %d, %Y')
                self.filenames[acc_num]['bill_dd'] = f"{bill_dd:%m/%d/%Y}"

            if bill_ds_re := re.search(BILL_DS_REG, text):
                bill_ds = datetime.strptime(bill_ds_re.group(1), '%b %d, %Y')
                self.filenames[acc_num]['bill_ds'] = f"{bill_ds:%m/%d/%Y}"

            if bill_amount_re := re.search(BILL_AMOUNT_REG, text, flags=re.IGNORECASE):
                self.filenames[acc_num]['bill_amount'] = bill_amount_re.group(1).replace(',', '')

    def quit(self):
        if self.browser:
            self.browser.quit()

        if self.session:
            self.session.close()
