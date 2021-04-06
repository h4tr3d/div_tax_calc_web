using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DividendTaxCalculatorLib.BrokerReportAdapters.Open.SPB
{
    [XmlRoot("nontrade_money_operation")]
    public class nontrade_money_operations
    {
        [XmlElement("item")]
        public List<nontrade_money_operation> items { get; set; }
    }

    public class nontrade_money_operation
    {
        [XmlAttribute("operationdate")]
        public DateTime operation_date { get; set; }

        [XmlAttribute("currencycode")]
        public string currency_code { get; set; }

        [XmlAttribute("amount")]
        public decimal amount { get; set; }

        [XmlAttribute("analyticname")]
        public string operation_type { get; set; }

        [XmlAttribute("comment")]
        public string comment { get; set; }

        public override string ToString()
        {
            return String.Format("operation_date={0}, currency_code={1}, amount={2}, comment={3}", operation_date, currency_code, amount, comment);
        }
    }
}
