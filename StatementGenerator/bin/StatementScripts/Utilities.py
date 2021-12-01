import os
from os import makedirs
from os.path import exists
from datetime import datetime
from threading import Thread
from requests import Response
from selenium.webdriver.chrome.options import Options


HEADERS = {'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.69 Safari/537.36',
           'Content-Type': 'application/x-www-form-urlencoded'}


CHROMEDRIVER_PATH = os.path.join(os.path.dirname(os.path.dirname(os.path.realpath(__file__))), "Driver", "chromedriver.exe")

SELENIUM_PROFILE_PATH = r"C:\Users\fsoto\AppData\Local\Google\Chrome\Selenium"
SELENIUM_PROFILE_NAME = "Profile 4"

XPATH = 'xpath'
ID = 'id'
CSS = 'css selector'


def get_default_options():
    opts = Options()
    # the option below makes the 'started listening' log go away in CMD
    opts.add_experimental_option("excludeSwitches", ["enable-logging"])
    opts.add_experimental_option('excludeSwitches', ['enable-automation'])
    opts.add_argument("--log-level=3")
    opts.add_argument(f"user-agent={HEADERS['User-Agent']}")
    opts.headless = True

    return opts


def is_OK(r: Response):
    return r.status_code == 200


class LoginFailedException(Exception):
    def __init__(self, msg="Unable to successfully log in. Check that server is up and credentials are valid."):
        super(LoginFailedException, self).__init__(msg)


class HttpFailedException(Exception):
    def __init__(self, msg="Unable to successfully retrieve page."):
        super(HttpFailedException, self).__init__(msg)


def wait_for_file(file, limit: float):
    if limit <= 0:
        raise ValueError(f"Time limit should be greater than zero. {limit} <= 0")

    def wait():
        start = datetime.now()
        keep_trying = True
        while keep_trying:
            # this keep_trying flag is used to determine if sucess
            if os.path.exists(file):
                keep_trying = False
            if (datetime.now() - start).seconds > limit:
                break

        if keep_trying:
            print(start, datetime.now())
            raise OSError(f'{file} was not found within {limit} second(s).')
    t = Thread(target=wait)
    t.start()
    t.join()


def assert_create_dir(statement_dir):
    # TODO: look for possible exception handling
    if not exists(statement_dir):
        makedirs(statement_dir)


def create_session_browser(session, browser):
    session.headers.update(HEADERS)

    for c in browser.get_cookies():
        session.cookies.update({c['name']: c['value']})
