import os

from random import randint

from .Utilities import assert_create_dir


class Statement:
    def __init__(self, accountID, username, password, path, parent=None, folder=None):
        self.statement = None
        self.filename = None
        self.filenames = {}

        if parent is None:
            parent = 'Statements'

        if folder is None:
            folder = 'Statements'

        if path is not None and os.path.exists(path):
            self.base_dir = os.path.join(path, parent, folder)
        else:
            self.base_dir = os.path.join(os.path.dirname(os.getcwd()), parent, folder)

        assert_create_dir(self.base_dir)

        self.run(accountID, username, password)

    def login(self, accountID, username, password):
        pass

    def navigate_to_statement(self):
        pass

    def save_statement(self):
        # write statement to file
        with open(self.filename, 'wb+') as f:
            f.write(self.statement)

    def quit(self):
        pass

    def run(self, accountID, username, password):
        try:
            self.login(accountID, username, password)
            self.navigate_to_statement()

            if self.filename is None:
                self.filename = f"{randint(0, 100):03}{chr(randint(65, 90))}_{accountID if not None else 'Statement'}"

            self.save_statement()
        except Exception as err:
            raise err
        finally:
            self.quit()
