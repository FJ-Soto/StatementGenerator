import os
import re
import io
from fitz import Document

from requests import Session
from bs4 import BeautifulSoup
from datetime import datetime
from PyPDF2.errors import PdfReadError

from .Statement import Statement
from .Utilities import HEADERS, is_OK, LoginFailedException, HttpFailedException

# standard login pages
LOGIN_PAGE = 'https://www.ftdi.com/signin/default.asp'

# for navigation
NAVIGATION_PAGES = ['https://www.ftdi.com/statement/', 'https://ftd.docugateway.com/oms/unconfirmed/document/display']
DOWNLOAD_LINK_START = 'printDoc('
DOWNLOAD_LINK_END = "'"
DOWNLOAD_LINK_BASE = 'https://ftd.docugateway.com'

LOGGED_IN_TXT = 'FTDi.com - Logged in'

BILL_DE_REG = r"((\d{2}/){2}\d{2})\s+?Closing Date"
BILL_DD_REG = r'\b(\d{2}\/\d{2}\/\d{2})\s+EFT\/WIRE DATE'
BILL_AMOUNT_REG = r'WIRE TO YOUR BANK ACCOUNT\s+?([\d|,]+.\d{2})'


class FTDStatement(Statement):
    """
    This class extends on the sample Statement class.
    The login, navigate_to_statement must be overwritten.
    The save_statement can be overwritten but does not need to.
    """
    def __init__(self, username, password, accountID, path=None):
        # call to super executes the standard procedures
        # 1. login
        # 2. navigate to pdf
        # 3. save pdf
        self.session = Session()

        super(FTDStatement, self).__init__(accountID, username, password,  path=path, folder='FTD')

    def login(self, accountID, username, password):
        data = {'txtAccount': accountID,
                'txtUserID': username,
                'txtPassword': password}

        # log user in AND check if successful
        r = self.session.post(LOGIN_PAGE, data=data, headers=HEADERS)

        res: BeautifulSoup = BeautifulSoup(r.content, 'html.parser')

        if not is_OK(r) or res.select_one('#bodyContentWrap h1').text != LOGGED_IN_TXT:
            raise LoginFailedException('Failed to login.')

        # generate filename now so that accountID can be used
        self.filename = f"{accountID}"

    def navigate_to_statement(self):
        for url in NAVIGATION_PAGES:
            # go to the statement page and attempt to get statement URL
            r = self.session.get(url)
            if not is_OK(r):
                raise HttpFailedException("Failed to reach statement page.")

            # FTD does not return a different code for failed logins so
            # this manually tests whether it will be possible to find the URL for the statement.
            url_begin = r.text.find(DOWNLOAD_LINK_START)
            if url_begin != -1:
                # this gets the start of the path (skips '/')
                url_begin += len(DOWNLOAD_LINK_START) + 1
                # this finds the end of the url
                url_end = r.text.find(DOWNLOAD_LINK_END, url_begin)
                # get the url path as a whole
                url_path = r.text[url_begin:url_end]
                # this url will be used to get the actual document
                statement_url = f"{DOWNLOAD_LINK_BASE}{url_path}"

                r = self.session.get(statement_url)
                # go to url and save content
                if not is_OK(r):
                    raise HttpFailedException("Failed to reach statement PDF.")
                self.statement = r.content

    def save_statement(self):
        try:
            acc_num = self.filename

            # avoid saving and having to rename
            with Document(stream=self.statement, filetype='pdf') as doc:
                text = "".join(page.get_textpage().extractText() for page in doc)

                bill_de_re = re.search(BILL_DE_REG, text, flags=re.IGNORECASE)
                bill_de = datetime.strptime(bill_de_re.group(1), '%m/%d/%y')

                self.filename = os.path.join(self.base_dir, f"{bill_de:%m-%y}_FTD_{acc_num}.pdf")
                self.filenames[acc_num] = {'filename': self.filename,
                                           'bill_de': f"{bill_de:%m/%d/%Y}"}

                if bill_dd := re.search(BILL_DD_REG, text, flags=re.IGNORECASE):
                    self.filenames[acc_num]['bill_dd'] = bill_dd.group(1)

                if bill_amount_match := re.search(BILL_AMOUNT_REG, text, flags=re.IGNORECASE):
                    bill_amount = bill_amount_match.group(1)
                    self.filenames[acc_num]['bill_amount'] = bill_amount

            super(FTDStatement, self).save_statement()
        except PdfReadError as _:
            raise PdfReadError('Document not found.')

    def quit(self):
        if self.session:
            self.session.close()
