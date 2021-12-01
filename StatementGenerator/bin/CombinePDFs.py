import os
import sys

from fitz import Document
from datetime import datetime
from os.path import exists, join

from StatementScripts.Utilities import assert_create_dir


def append_to_doc(doc1: Document, doc_to_append):
    doc1.insert_pdf(doc_to_append)


if __name__ == '__main__':
    initial_folder = os.getcwd()
    try:
        if len(sys.argv) <= 1:
            raise ValueError("No arguments passed.", 1)
        if '--p' in sys.argv[1]:
            try:
                initial_folder = sys.argv[1].split('=')[1]
            except IndexError as err:
                raise IndexError('Unable to parse path.', 2)
            files = sys.argv[2:]
        else:
            files = sys.argv[1:]

        if len(files) < 1:
            raise ValueError("No specified files", 3)
        assert_create_dir(join(initial_folder, "Statements", "General"))

        save_loc = join(initial_folder, "Statements", "General")
        if not exists(files[0]):
            raise ValueError("Initial file does not exist.", 4)

        master_doc = Document(files[0])

        for file in files[1:]:
            doc2 = Document(file)
            append_to_doc(master_doc, doc2)

        filename = join(save_loc, f"{datetime.now():%m-%y}_Cumulative_Statement.pdf")
        master_doc.save(filename)
        print(filename)
    except IndexError as err:
        print(err.args[0])
        exit(err.args[1])
    except ValueError as err:
        print(err.args[0])
        exit(err.args[1])
    except Exception as err:
        print(err)
