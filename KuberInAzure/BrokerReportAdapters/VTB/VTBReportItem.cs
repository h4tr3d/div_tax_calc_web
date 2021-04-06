using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DividendTaxCalculatorLib.BrokerReportAdapters.VTB
{
    //<Tablix_b4>
    //<DDS_place_Collection>
    //  <DDS_place DDS_place="Основной рынок">
    //    <Подробности16_Collection>
    //    <Подробности16 debt_type4="2020-06-25T00:00:00" debt_date4="0.28" decree_amount2="USD" operation_type="Дивиденды" notes1="Дивиденды по акциям Kraft Heinz Company за 31.05.2019. НДС не обл. Эмитентом удержан налог 0.12 USD." />

    [XmlRoot("Подробности16_Collection", Namespace = "report577p_v1")]
    public class VTBReportItems
    {
        [XmlElement("Подробности16")]
        public List<VTBReportItem> items { get; set; }
    }

    public class VTBReportItem
    {
        [XmlAttribute("debt_type4")]
        public DateTime operation_date { get; set; }

        [XmlAttribute("debt_date4")]
        public decimal amount { get; set; }

        [XmlAttribute("decree_amount2")]
        public string currency_code { get; set; }

        [XmlAttribute("operation_type")]
        public string operation_type { get; set; }

        [XmlAttribute("notes1")]
        public string comment { get; set; }

        public override string ToString()
        {
            return String.Format("operation_date={0}, currency_code={1}, amount={2}, comment={3}", operation_date, currency_code, amount, comment);
        }
    }
}
