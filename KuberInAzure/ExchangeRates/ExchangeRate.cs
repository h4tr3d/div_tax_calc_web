using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DividendTaxCalculatorLib.ExchangeRates
{
    [XmlRoot("ExchangeRates")]
    public class ExchangeRates
    {
        [XmlElement("Items")]
        public List<ExchangeRate> Items { get; set; }
    }

    public class ExchangeRate
    {
        [XmlAttribute("Date")]
        public DateTime Date { get; set; }

        [XmlAttribute("Value")]
        public decimal Value { get; set; }
    }
}
