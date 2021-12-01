import os
import re
from datetime import datetime

from fitz import Document
from selenium.webdriver import Chrome
from selenium.common.exceptions import TimeoutException, ElementNotInteractableException
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.remote.webelement import WebElement
from selenium.webdriver.support.expected_conditions import presence_of_element_located as pel

from requests import Session
from .Statement import Statement
from .Utilities import LoginFailedException, CHROMEDRIVER_PATH
from .Utilities import ID, CSS, create_session_browser, get_default_options

LOGIN_PAGE = 'https://www.peoplesgasdelivery.com/secure/auth/l/acct/summary_accounts?show_list=true'
ID_LOGIN_PAGE = 'siteHeaderContainer'
ID_USER = 'signInName'
ID_PASS = 'password'
ID_SUBMIT = 'next'
CSS_ACCOUNT_NUMS = 'tr.selectableRow input'
ACCOUNT_OVERVIEW = "https://www.peoplesgasdelivery.com/accountsummary/View/AccountOverview?&acct="
CSS_STATEMENT = 'a[href*="BillPdf?"]'

BILL_DE_REG = r'Bill Period:\s+\d{2}\/\d{2}\/\d{4}\s+to\s+\b(\d{2}\/\d{2}\/\d{4})'
BILL_DS_REG = r'Bill Period:\s+\b(\d{2}\/\d{2}\/\d{4})'
BILL_AMOUNT_REG = r'Amount Due By \s+\d{2}\/\d{2}\/\d{4}\s+\$\b([0-9,]+\.\d{2})'
BILL_DD_REG = r'Amount Due By \s+\b(\d{2}\/\d{2}\/\d{4})'


class PeoplesGasStatement(Statement):
    def __init__(self, username, password, accountID=None, path=None):
        self.browser: [Chrome, None] = None
        self.session = Session()
        self.account_res = {}

        super(PeoplesGasStatement, self).__init__(accountID, username, password, path=path, folder='PeoplesGas')

    def login(self, accountID, username, password):
        options = get_default_options()

        browser = Chrome(executable_path=CHROMEDRIVER_PATH, options=options)
        browser.set_window_size(1920, 1080)

        browser.get(LOGIN_PAGE)

        # wait for redirects to finish
        WebDriverWait(browser, 20).until(pel((ID, ID_LOGIN_PAGE)))
        try:
            user: WebElement = WebDriverWait(browser, 10).until(pel((ID, ID_USER)))
            password_txt: WebElement = WebDriverWait(browser, 10).until(pel((ID, ID_PASS)))
            submit_btn: WebElement = WebDriverWait(browser, 10).until(pel((ID, ID_SUBMIT)))

        except TimeoutException as err:
            print(err)
            raise TimeoutException('Login element timeout.')

        start = datetime.now()
        while True and (datetime.now() - start).seconds <= 10:
            try:
                user.send_keys(username)
                password_txt.send_keys(password)
                submit_btn.click()
                break
            except ElementNotInteractableException as _:
                pass

        try:
            # wait for links to be visible to grab all
            WebDriverWait(browser, 10).until(pel((CSS, CSS_ACCOUNT_NUMS)))
        except Exception as _:
            raise LoginFailedException()

        pos_accounts = browser.find_elements_by_css_selector(CSS_ACCOUNT_NUMS)
        account_nums = [account.get_property('value') for account in pos_accounts]

        for account_num in account_nums:
            browser.get(f"{ACCOUNT_OVERVIEW}{account_num}")
            statement_url: WebElement = WebDriverWait(browser, 10).until(pel((CSS, CSS_STATEMENT)))
            self.account_res[account_num] = statement_url.get_attribute('href')

        self.browser = browser

    def save_statement(self):
        create_session_browser(self.session, self.browser)

        for acc_num, statement_url in self.account_res.items():
            self.statement = self.session.get(statement_url).content

            with Document(stream=self.statement, filetype='pdf') as doc:
                text = "".join(page.get_textpage().extractText() for page in doc)

                bill_de_re = re.search(BILL_DE_REG, text, flags=re.IGNORECASE)
                bill_de = datetime.strptime(bill_de_re.group(1), '%m/%d/%Y')

                self.filename = os.path.join(self.base_dir, f'{bill_de:%m-%y}_GAS_{acc_num}.pdf')

                self.filenames[acc_num] = {'filename': self.filename,
                                           'bill_de': f"{bill_de:%m/%d/%Y}"}

                if bill_dd := re.search(BILL_DD_REG, text, flags=re.IGNORECASE):
                    self.filenames[acc_num]['bill_dd'] = bill_dd.group(1)

                if bill_ds := re.search(BILL_DS_REG, text, flags=re.IGNORECASE):
                    self.filenames[acc_num]['bill_ds'] = bill_ds.group(1)

                if bill_amount_re := re.search(BILL_AMOUNT_REG, text, flags=re.IGNORECASE):
                    self.filenames[acc_num]['bill_amount'] = bill_amount_re.group(1).replace(',', '')

            super(PeoplesGasStatement, self).save_statement()

    def quit(self):
        if self.browser:
            self.browser.quit()

        if self.session:
            self.session.close()
