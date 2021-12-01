import os

import tempfile

from .Statement import Statement
from .Utilities import LoginFailedException, XPATH, CHROMEDRIVER_PATH
from .Utilities import wait_for_file, get_default_options

from datetime import datetime
from selenium. webdriver import Chrome
from selenium.common.exceptions import TimeoutException
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.remote.webelement import WebElement
from selenium.webdriver.support.expected_conditions import presence_of_element_located as pel
from selenium.webdriver.support.expected_conditions import invisibility_of_element_located as iel

# standard site for logging in
LOGIN_PAGE = 'https://secure.comed.com/MyAccount/MyBillUsage/Pages/Secure/MyBillDetails.aspx'

# for login
X_EMAIL = '//*[@id="main"]/div[2]/div/div/app-signin/section/form/div[1]/div/div[1]/app-input-common/label/input'
X_PASS = '//*[@id="main"]/div[2]/div/div/app-signin/section/form/div[1]/div/div[3]/app-input-masking-common/label/input'
X_SUBMIT = '//*[@id="main"]/div[2]/div/div/app-signin/section/form/div[2]/app-button-common[1]/button'
LOGIN_PAGE_TITLE = 'My Bill Details | ComEd - An Exelon Company'


# for pdf navigation
X_PDF = '//*[@id="MyBillDetails"]/div/div[2]/div[5]/button'
X_FLAG = '//*[@id="MyBillDetails"]/div/div[2]/div[1]/span'

# for account num and bill date
X_ACC_NUM = '//*[@id="AccountSummaryBannerController"]/div[1]/div[1]/p[2]/span'
X_BILL_DATE = '//*[@id="MyBillDetails"]/div/div[2]/div[1]/div[2]'


class ComEDStatement(Statement):
    def __init__(self, username, password, accountID=None, path=None):
        self.browser: [Chrome, None] = None
        self.temp_dir: [tempfile.TemporaryDirectory, None] = None

        super(ComEDStatement, self).__init__(accountID, username, password, path=path, folder='ComED')

    def login(self, accountID, username, password):
        self.temp_dir = tempfile.TemporaryDirectory(dir=self.base_dir)
        options = get_default_options()
        options.add_experimental_option('prefs', {
            "download.default_directory": self.temp_dir.name
        })

        browser = Chrome(options=options, executable_path=CHROMEDRIVER_PATH)
        browser.set_window_size(1920, 1080)
        # go straight to bill usage instead of (as before) the login
        # browser.get('https://secure.comed.com/accounts/login')
        browser.get(LOGIN_PAGE)

        try:
            email_txt: WebElement = WebDriverWait(browser, 10).until(pel((XPATH, X_EMAIL)))

            password_txt: WebElement = WebDriverWait(browser, 10).until(pel((XPATH, X_PASS)))

            submit_btn: WebElement = WebDriverWait(browser, 10).until(pel((XPATH, X_SUBMIT)))
        except TimeoutException as _:
            raise TimeoutException('Login element timeout.')

        email_txt.send_keys(username)
        password_txt.send_keys(password)
        submit_btn.click()

        if browser.title != LOGIN_PAGE_TITLE:
            raise LoginFailedException('Login failed - verify credentials.')
        self.browser = browser

    def navigate_to_statement(self):
        browser = self.browser
        try:
            pdf_btn: WebElement = WebDriverWait(browser, 20).until(pel((XPATH, X_PDF)))

            pdf_btn.click()

            flag_generating: WebElement = WebDriverWait(browser, 20).until(pel((XPATH, X_FLAG)))

            # wait for download to finish to rename and relocate
            WebDriverWait(browser, 30).until(iel(flag_generating))
        except TimeoutException as _:
            raise TimeoutException('Navigation element timeout.')

    def save_statement(self):
        browser = self.browser
        # get acc
        try:
            acc_num: WebElement = WebDriverWait(browser, 20).until(pel((XPATH, X_ACC_NUM)))

            bill_date: WebElement = WebDriverWait(browser, 20).until(pel((XPATH, X_BILL_DATE)))
        except TimeoutException as _:
            raise TimeoutException('Statement download element timeout.')

        file_curr = os.path.join(self.temp_dir.name, 'BillImage.pdf')
        statement_date = datetime.strptime(bill_date.text.strip(), '%m/%d/%Y')

        wait_for_file(file_curr, limit=20)

        filename = f"{statement_date:%m-%y}_ComED_{acc_num.text}.pdf"
        file_dest = os.path.join(self.base_dir, filename)
        os.replace(file_curr, file_dest)

        self.filename = file_dest
        self.filenames[acc_num] = {'filename': self.filename, 'bill_de': f"{statement_date:%m/%d/%Y}"}

    def quit(self):
        if self.browser:
            self.browser.quit()

        self.temp_dir.cleanup()
