import win32api
import win32print
import sys

COLOR = 1
NOCOLOR = 2
DUPLEX = 3
SINGLEX = 1


# print quality normal 	=	600 x 300
# print quality draft 	= 	300 x 300
# print quality photo 	= 	600 x 600
# print quality Max	= 	1200 x 1200
#
#
# in PDEVMODE there is PrintQuality and YResolution

# TODO: write a main that parses commands receives
def print_file(filepath, do_color=None, do_duplex=None, quality=None, printer=None):
    """

    :param str filepath:
    :param bool do_color:
    :param bool do_duplex:
    :param tuple quality:
    :param printer:
    :return:
    """
    print_defaults = {"DesiredAccess": win32print.PRINTER_ALL_ACCESS}
    level = 2

    printer_handle = win32print.OpenPrinter(win32print.GetDefaultPrinter(), print_defaults)

    printer_attr = win32print.GetPrinter(printer_handle, level)
    printer_attr['pDevMode'].Duplex = 3
    printer_attr['pDevMode'].Color = 1

    win32print.SetPrinter(printer_handle, level, printer_attr, 0)
    win32api.ShellExecute(0, 'print', filepath, None, ".", 0)

    win32print.ClosePrinter(printer_handle)


if __name__ == '__main__':
    num_args = len(sys.argv) - 1
    settings = {}

    def usage():
        print("py PrintFile.py filepath")

    def print_help():
        print('Parameters')
        print("\t-c=[COLOR | NOCOLOR]")
        print("\t-d=[DUPLEX|SINGLEX]")
        print("\t-x=dpiX")
        print("\t-y=dpiY"
              "\t-p=printer")

    def parse_color(x):
        if x == 'NOCOLOR':
            return NOCOLOR
        elif x == 'COLOR':
            return COLOR

        print('Incorrect usage of parameter -c.')
        print("\t-c=[COLOR | NOCOLOR]")
        exit(2)

    def parse_duplex(x):
        if x == 'DUPLEX':
            return DUPLEX
        elif x == 'SINGLEX':
            return SINGLEX

        print('Incorrect usage of parameter -d.')
        print("\t-d=[DUPLEX|SINGLEX]")
        exit(3)

    def parse_quality(x):
        try:
            return int(x)
        except ValueError:
            print(f'Incorrect usage of parameter for quality.')
            print("\t-x|-y=[integer dpi]")
            exit(4)


    params = {'-c': parse_color,
              '-d': parse_duplex,
              '-x': parse_quality,
              '-y': parse_quality,
              '-p': lambda x: x}

    if num_args == 0:
        print('Number of arguments not supported.')
        usage()
        exit(1)

    settings['filepath'] = sys.argv[1]

    for arg in sys.argv[1:]:
        if arg[0:2] in params:
            params[arg[0:2]](arg[3:])
        else:
            print(f'Unsupported argument: {arg}')
            exit(6)
    # settings = {'filepath': sys.argv[1],
    #             'color': sys.argv[2],
    #             'duplex': sys.argv[3],
    #             'quality': sys.argv[4]}

