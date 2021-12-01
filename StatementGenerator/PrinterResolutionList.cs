using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Printing;
using static System.Drawing.Printing.PrinterSettings;


// Probably won't use this class, but I might change my mind later.
namespace FTDStatementPrinter
{
    class PrinterResolutionList : List<AdjustedPrinterResolution>
    {
        public List<AdjustedPrinterResolution> Resolutions { get; set; }
        public PrinterResolutionList(PrinterResolutionCollection list)
        {
            Resolutions = new List<AdjustedPrinterResolution>();
            foreach (PrinterResolution res in list)
            {
                Resolutions.Add(new AdjustedPrinterResolution(res));
            }
        }
    }

    class AdjustedPrinterResolution
    {
        public PrinterResolution Resolution { get; set; }

        public AdjustedPrinterResolution(PrinterResolutionKind kind) {
            Resolution = new PrinterResolution()
            {
                Kind = kind
        };
        }

        public AdjustedPrinterResolution(PrinterResolution p)
        {
            Resolution = p;
        }

        override public string ToString()
        {
            if (Resolution.Kind == PrinterResolutionKind.Custom)
            {
                return $"X: {Resolution.X}, Y: {Resolution.Y}";
            }
            return Resolution.Kind.ToString();
        }

        override public bool Equals(object obj)
        {
            return ToString() == obj.ToString();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
