import os
import re

from fitz import Document
from requests import Session
from bs4 import BeautifulSoup
from datetime import datetime
from selenium.webdriver import Chrome
from selenium.common.exceptions import TimeoutException
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.remote.webelement import WebElement
from selenium.webdriver.support.expected_conditions import presence_of_element_located as pel

from .Statement import Statement
from .Utilities import ID, LoginFailedException
from .Utilities import get_default_options, create_session_browser
from .Utilities import SELENIUM_PROFILE_PATH, SELENIUM_PROFILE_NAME, CHROMEDRIVER_PATH

LOGIN_PAGE = 'https://amazon.syf.com/login/'
LOGIN_PAGE_TITLE = 'Amazon - Account Summary'

STATEMENT_PAGE = 'https://amazon.syf.com/eService/EBill/eBillAction'
CSS_STMT_DATE = '#ebillform > div.container > div > div > div.activityListing > ol > li:nth-child(1) > div:first-child  p:nth-child(2) > span'
ID_USER = 'userId'
ID_PASS = 'password'
ID_SUBMIT = 'secure-login'
ID_HOMEPAGE = 'main-content-area'

ACC_NUM_REG = r'Account Number ending in \b(\d+)'
BILL_DE_REG = r'New Balance as of \b(\d{2}\/\d{2}\/\d{4})'
BILL_AMOUNT_REG = r'New Balance as of \d{2}\/\d{2}\/\d{4}\s+\$\b([0-9,]+\.\d{2})'
BILL_DD_REG = r'Payment Due Date\s+\b(\d{2}\/\d{2}\/\d{4})'
BILL_DS_REG = r'Previous Balance as of \b(\d{2}\/\d{2}\/\d{4})'

STATEMENT_URL = 'https://amazon.syf.com/eService/EBill/eBillViewPDFAction.action?stmtDate='


class AmazonSynchrony(Statement):
    def __init__(self, username, password, accountID=None, path=None):
        self.browser: [Chrome, None] = None
        self.session = Session()

        super(AmazonSynchrony, self).__init__(accountID, username, password, path=path, folder='AmazonSynchrony')

    def login(self, accountID, username, password):
        options = get_default_options()
        options.headless = False
        options.add_argument('--disable-blink-features=AutomationControlled')
        options.add_argument(f'--profile-directory={SELENIUM_PROFILE_NAME}')
        options.add_argument(f'--user-data-dir={SELENIUM_PROFILE_PATH}')

        browser = Chrome(executable_path=CHROMEDRIVER_PATH, options=options)
        browser.set_window_size(800, 800)

        browser.get(LOGIN_PAGE)
        try:
            email_txt: WebElement = WebDriverWait(browser, 10).until(pel((ID, ID_USER)))
            password_txt: WebElement = WebDriverWait(browser, 10).until(pel((ID, ID_PASS)))
            submit_btn: WebElement = WebDriverWait(browser, 10).until(pel((ID, ID_SUBMIT)))
        except TimeoutException as _:
            raise TimeoutException('Login element timeout.')

        email_txt.send_keys(username)
        password_txt.send_keys(password)
        submit_btn.click()

        try:
            WebDriverWait(browser, 15).until(pel((ID, ID_HOMEPAGE)))
        except TimeoutException as _:
            raise TimeoutException('Homepage timeout.')

        if browser.title != LOGIN_PAGE_TITLE:
            raise LoginFailedException('Login failed - verify credentials.')

        create_session_browser(self.session, browser)

    def navigate_to_statement(self):
        r = self.session.get(STATEMENT_PAGE)
        doc = BeautifulSoup(r.content, 'html.parser')

        statement_date_txt = doc.select_one(CSS_STMT_DATE).text.split('\t')[-1]
        statement_date = datetime.strptime(statement_date_txt, '%B %d, %Y')

        self.statement = self.session.get(f"{STATEMENT_URL}{statement_date:%B %d, %Y}&inLine=true").content

        with Document(stream=self.statement, filetype='pdf') as doc:
            text = "".join(page.get_textpage().extractText() for page in doc)

            # required
            acc_num_match = re.search(ACC_NUM_REG, text)
            acc_num = acc_num_match.group(1)

            # optional
            bill_de_match = re.search(BILL_DE_REG, text)
            bill_de = datetime.strptime(bill_de_match.group(1), '%m/%d/%Y')

            self.filenames[acc_num] = {'bill_de': f"{bill_de:%m/%d/%Y}"}

            if bill_amount_match := re.search(BILL_AMOUNT_REG, text):
                bill_amount = bill_amount_match.group(1)
                self.filenames[acc_num]['bill_amount'] = bill_amount

            if bill_ds_match := re.search(BILL_DS_REG, text):
                bill_ds = bill_ds_match.group(1)
                self.filenames[acc_num]['bill_ds'] = bill_ds

            if bill_dd_match := re.search(BILL_DD_REG, text):
                bill_dd = bill_dd_match.group(1)
                self.filenames[acc_num]['bill_dd'] = bill_dd

        self.filename = os.path.join(self.base_dir, f"{statement_date:%m-%y}_AmazonSynchrony_{acc_num}.pdf")
        self.filenames[acc_num]['filename'] = self.filename

    def quit(self):
        if self.browser:
            self.browser.quit()

        if self.session:
            self.session.close()
