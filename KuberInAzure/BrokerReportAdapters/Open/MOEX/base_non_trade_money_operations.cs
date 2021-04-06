using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DividendTaxCalculatorLib.BrokerReportAdapters.Open.MOEX
{
    public class base_non_trade_money_operation
    {
        [XmlAttribute("operation_date")]
        public DateTime operation_date { get; set; }

        [XmlAttribute("currency_code")]
        public string currency_code { get; set; }

        [XmlAttribute("amount")]
        public decimal amount { get; set; }

        [XmlAttribute("comment")]
        public string comment { get; set; }

        public override string ToString()
        {
            return String.Format("operation_date={0}, currency_code={1}, amount={2}, comment={3}", operation_date, currency_code, amount, comment);
        }
    }
}
