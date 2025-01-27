import sys
import os
import traceback

from PyPDF2.errors import PdfReadError
from selenium.common.exceptions import ElementNotVisibleException, TimeoutException

from StatementScripts.ComEDStatement import ComEDStatement
from StatementScripts.FTDStatement import FTDStatement
from StatementScripts.XfinityStatement import XfinityStatement
from StatementScripts.ChicagoUtilStatement import ChicagoUtilStatement
from StatementScripts.PeoplesGasStatement import PeoplesGasStatement
from StatementScripts.AmazonSynchrony import AmazonSynchrony
from StatementScripts.Utilities import LoginFailedException, HttpFailedException

matcher = {'ComED': ComEDStatement,
           'FTD': FTDStatement,
           'Xfinity': XfinityStatement,
           'Util': ChicagoUtilStatement,
           'PeoplesGas': PeoplesGasStatement,
           'AmazonSynchrony': AmazonSynchrony}

options = {'-u': 'username',
           '-p': 'password',
           '-a': 'accountID',
           '--p': 'path'}


def usage():
    print(f'py StatementMain.py [{"|".join(matcher.keys())}] "-u=username" "-p=password" "-a=[accountID]" "--p=[path]"')


def is_valid_path(path):
    return os.path.exists(path)


if __name__ == '__main__':
    has_username = False
    has_password = False
    num_args = len(sys.argv[1:])
    args = {}

    if num_args not in [1, 3, 4, 5]:
        print('Incorrect number of arguments.')
        print('If you are supplying a path, ensure that the path is enclosed with double-quotes.')
        exit(1)

    if sys.argv[1] == '--help':
        usage()
        exit(0)

    if sys.argv[1] not in matcher:
        print('Unsupported statement type.')
        exit(2)

    used_options = set()
    for arg in sys.argv[2:]:
        try:
            items = arg.split('=')
            if len(items) != 2:
                raise ValueError(f'Unable to parse "{arg}".', 3)

            k, v = arg.split('=')

            if k == '-u':
                has_username = True

            if k == '-p':
                has_password = True

            if k in used_options:
                raise ValueError(f'"{k}" is used more than once.', 5)
            else:
                used_options.add(k)

            if k in options:
                args[options[k]] = v
            else:
                raise ValueError(f'"{k}" is not a valid option.', 4)

        except ValueError as err:
            print(err.args[0])
            print(f"Ensure format is like [{', '.join(options.keys())}]=value.")
            exit(err.args[1])

    if 'path' in args and not is_valid_path(args['path']):
        print(f'Directory: "{args["path"]}" does not exist.')
        exit(6)

    if not has_username:
        print('No username supplied.')
        exit(7)

    if not has_password:
        print('No password supplied.')
        exit(8)

    try:
        statement_obj = matcher[sys.argv[1]](accountID=args.setdefault('accountID', None),
                                             username=args['username'], password=args['password'],
                                             path=args.setdefault('path', None))

        res = []
        for acc_num, details in statement_obj.filenames.items():
            # required/expected
            detail_line = [f"-a=\"{acc_num}\"",
                           f"-f=\"{details['filename']}\"",
                           f"-be=\"{details['bill_de']}\""]

            if 'bill_ds' in details:
                detail_line.append(f"-bs=\"{details['bill_ds']}\"")

            if 'bill_dd' in details:
                detail_line.append(f"-bd=\"{details['bill_dd']}\"")

            if 'bill_amount' in details:
                detail_line.append(f"-ba=\"{details['bill_amount']}\"")

            if detail_line:
                res.append(" ".join(detail_line))

        for detail_line in res:
            print(detail_line)

    except LoginFailedException as err:
        print(f"There was an error attempting to log in.\n{err}")
        exit(9)
    except HttpFailedException as err:
        print(f"There was an error reaching the website.\n{err}")
        exit(10)
    except TimeoutException as err:
        print(f"Unable to locate a page element.\n{err}")
        exit(11)
    except ElementNotVisibleException as err:
        print(f"An element is not intractable.\n{err}")
        exit(12)
    except PdfReadError as err:
        print(err)
        exit(13)
    except Exception as err:
        print(err)
        traceback.print_exc()
        exit(14)
