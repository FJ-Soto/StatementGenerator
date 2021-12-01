namespace FTDStatementPrinter
{
    public class StatementDetail
    {

        public string Filename { get; set; }
        public string BillStart { get; set; }
        public string BillEnd { get; set; }
        public string BillDue { get; set; }
        public decimal? BillAmount { get; set; }
        public StatementDetail(string filename, string billEnd, string billStart=null, string billDue=null, decimal? amount=null)
        {
            Filename = filename;
            BillEnd = billEnd;

            BillStart = billStart;
            BillDue = billDue;
            BillAmount = amount;
        }
    }
}
