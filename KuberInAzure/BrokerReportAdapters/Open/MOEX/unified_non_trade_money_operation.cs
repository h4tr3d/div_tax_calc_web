using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DividendTaxCalculatorLib.BrokerReportAdapters.Open.MOEX
{
    //<unified_non_trade_money_operations>
    //<item operation_date="2020-01-08T00:00:00" currency_code="RUB" amount="-12.30" comment="Комиссия Брокера / Доп. комиссия Брокера &quot;Сборы ТС&quot; за заключение сделок 08.01.2019 на Фондовый Рынок Московской биржи по счету 173345i" />

    [XmlRoot("unified_non_trade_money_operations")]
    public class unified_non_trade_money_operations
    {
        [XmlElement("item")]
        public List<unified_non_trade_money_operation> items { get; set; }
    }

    public class unified_non_trade_money_operation : base_non_trade_money_operation
    {
       
    }
}
