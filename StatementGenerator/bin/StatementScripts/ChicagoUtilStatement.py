import io
import os
import re

from fitz import Document

from .Statement import Statement
from .Utilities import LoginFailedException, CHROMEDRIVER_PATH
from .Utilities import create_session_browser, get_default_options

from requests import Session
from datetime import datetime
from selenium.webdriver import Chrome
from urllib3 import disable_warnings
from selenium.webdriver.common.by import By
from urllib3.exceptions import InsecureRequestWarning
from selenium.common.exceptions import TimeoutException
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.remote.webelement import WebElement
from selenium.webdriver.support.expected_conditions import presence_of_element_located as pel

# standard site for logging in
LOGIN_PAGE = 'https://utilitybill.chicago.gov/main/public/user/'
LOGIN_W_ACC_NUM = 'https://utilitybill.chicago.gov/main/user/document/display/latest?account='

# for login
ID_USER = 'username'
ID_PASS = 'password'
IS_SUBMIT = 'login'

# for locating acc num
CSS_ACC_NUM = '#top-acct > div > div.right_bar > span'

# for saving doc
CSS_DOC_LINK = '#left > div.show-this > div > object > embed'
CSS_BILL_DATE = '#right2 > div.formContainer.StatementDetails > div > fieldset > div:nth-child(4) > span'

LOGGED_IN_TXT = 'userhome'

ACC_NUM_REG = r'Account Number ending in \b(\d+)'
BILL_DE_REG = r'Bill Period:\s+(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\-\d{2}\-\d{4}\s+-\s+\b((?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\-\d{2}\-\d{4})'
BILL_AMOUNT_REG = r'\$([0-9,]+\.\d{2})\s+TOTAL DUE'
BILL_DD_REG = r'\b((Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\-\d{2}\-\d{4})\s+To Avoid Penalties, Pay By:'
BILL_DS_REG = r'Bill Period:\s+\b((?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\-\d{2}\-\d{4})\s+'


class ChicagoUtilStatement(Statement):
    def __init__(self, username, password, accountID=None, path=None):
        self.browser: [Chrome, None] = None
        self.session = Session()

        super(ChicagoUtilStatement, self).__init__(accountID, username, password, path=path, folder='ChicagoUtility')

    def login(self, accountID, username, password):
        options = get_default_options()
        browser = Chrome(executable_path=CHROMEDRIVER_PATH, options=options)
        browser.set_window_size(1920, 1080)

        # if accountID is None:
        browser.get(LOGIN_PAGE)
        if accountID is not None:
            # perform accountID validation
            reg1 = re.compile(r'^\d{13}$|^\d{7}-\d{6}$')
            reg2 = re.compile(r'\d{13}$')

            if not reg1.search(accountID):
                raise ValueError('AccountID is not quite correct or malformed.\n')
            elif reg2.search(accountID):
                accountID = f"{accountID[0:7]}-{accountID[7:]}"

        # login phase
        try:
            user: WebElement = WebDriverWait(browser, 10).until(pel((By.ID, ID_USER)))
            password_txt: WebElement = WebDriverWait(browser, 10).until(pel((By.ID, ID_PASS)))
            submit_btn: WebElement = WebDriverWait(browser, 10).until(pel((By.ID, IS_SUBMIT)))
        except TimeoutException as _:
            raise TimeoutException('Login element timeout.')

        user.send_keys(username)
        password_txt.send_keys(password)
        submit_btn.click()

        if LOGGED_IN_TXT not in browser.current_url:
            raise LoginFailedException()

        if accountID is None:
            try:
                acc_num_txt: WebElement = WebDriverWait(browser, 10).until(pel((By.CSS_SELECTOR, CSS_ACC_NUM)))
                accountID = acc_num_txt.text
            except TimeoutException as _:
                raise TimeoutException('Login element timeout: account_id.')

        browser.get(f"{LOGIN_W_ACC_NUM}{accountID}")

        self.filename = f"{accountID}"
        self.browser = browser

    def navigate_to_statement(self):
        browser = self.browser

        try:
            document_link: WebElement = WebDriverWait(browser, 10).until(pel((By.CSS_SELECTOR, CSS_DOC_LINK)))
            # bill_date: WebElement = WebDriverWait(browser, 10).until(pel((By.CSS_SELECTOR, CSS_BILL_DATE)))
            # statement_date = datetime.strptime(bill_date.text, '%B %d, %Y')
        except TimeoutException as _:
            raise TimeoutException('Navigation element timeout.')

        # perform download via session
        self.session.verify = False
        disable_warnings(InsecureRequestWarning)

        create_session_browser(self.session, browser)

        self.statement = self.session.get(document_link.get_property('src')).content

        acc_num = self.filename
        with Document(stream=self.statement, filetype='pdf') as doc:
            text = "".join(page.get_textpage().extractText() for page in doc)\

            bill_de_match = re.search(BILL_DE_REG, text)
            bill_de = datetime.strptime(bill_de_match.group(1), '%b-%d-%Y')

            self.filename = os.path.join(self.base_dir, f"{bill_de:%m-%y}_Utility_{acc_num}.pdf")

            self.filenames[acc_num] = {'filename': self.filename,
                                       'bill_de': f"{bill_de:%m/%d/%Y}"}

            if bill_amount_match := re.search(BILL_AMOUNT_REG, text):
                bill_amount = bill_amount_match.group(1)
                self.filenames[acc_num]['bill_amount'] = bill_amount

            if bill_ds_match := re.search(BILL_DS_REG, text):
                bill_ds = datetime.strptime(bill_ds_match.group(1), '%b-%d-%Y')
                self.filenames[acc_num]['bill_ds'] = f"{bill_ds:%m/%d/%Y}"

            if bill_dd_match := re.search(BILL_DD_REG, text):
                bill_dd = datetime.strptime(bill_dd_match.group(1), '%b-%d-%Y')
                self.filenames[acc_num]['bill_dd'] = f"{bill_dd:%m/%d/%Y}"
    
    def quit(self):
        if self.browser:
            self.browser.quit()

        if self.session:
            self.session.close()
