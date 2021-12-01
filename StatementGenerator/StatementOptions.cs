using System.Collections.Generic;

namespace FTDStatementPrinter
{
    public class StatementOption
    {
        public string Name { get; set; }
        public string Value { get; set; }
        private  StatementOption(string name, string value) {
            Name = name;
            Value = value;
        }

        public static List<StatementOption> StatementTypes()
        {
            return new List<StatementOption>{
                new StatementOption("FTD", "FTD"),
                new StatementOption("ComED", "ComED"),
                new StatementOption("Peoples Gas", "PeoplesGas"),
                new StatementOption("Xfinity", "Xfinity"),
                new StatementOption("Chicago Utilities", "Util"),
                new StatementOption("Amazon Synchrony", "AmazonSynchrony")
            };
        }
    }
}
