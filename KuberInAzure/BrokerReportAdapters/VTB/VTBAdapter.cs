using DividendTaxCalculatorLib.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace DividendTaxCalculatorLib.BrokerReportAdapters.VTB
{
    public class VTBAdapter : IBrokerReportAdapter
    {
        public List<TaxReportItem> Parse(IFormFile file)
        {
            XPathDocument document = new XPathDocument(file.OpenReadStream());
            XPathNavigator navigator = document.CreateNavigator();

            navigator.MoveToChild("Report", "report577p_v1");
            navigator.MoveToChild("Tablix_b4", "report577p_v1");
            navigator.MoveToChild("DDS_place_Collection", "report577p_v1");
            navigator.MoveToChild("DDS_place", "report577p_v1");
            navigator.MoveToChild("Подробности16_Collection", "report577p_v1");

            VTBReportItems operations = XmlSerializationHelper.Deserialize<VTBReportItems>(navigator.OuterXml);

            List<TaxReportItem> brokerReportItems = new List<TaxReportItem>();

            Regex taxRegex = new Regex(@"(?<TAX_ROOT>налог\s(?<TAX_VALUE>\S+)\sUSD)");
            Regex comissionRegex = new Regex(@"(?<COMISSION_ROOT>комиссия\s(?<COMISSION_VALUE>\S+)\sUSD)");

            foreach (VTBReportItem item in operations.items)
            {
                if (item.currency_code == "USD" && 
                    (
                    (item.operation_type == "Дивиденды") || 
                    
                    (item.operation_type == "Зачисление денежных средств" && item.comment != null && item.comment.StartsWith("Дивиденды"))
                    
                    )
                    )
                {
                    TaxReportItem brokerReportItem = new TaxReportItem();

                    brokerReportItem.BrokerName = "ВТБ Брокер (ММВБ+СПб)";
                    brokerReportItem.OperationDate = item.operation_date;
                    brokerReportItem.AmountUSD = item.amount;
                    brokerReportItem.CurrencyCode = item.currency_code;
                    brokerReportItem.Comment = item.comment;

                    if (taxRegex.IsMatch(item.comment))
                    {
                        Match m = taxRegex.Match(item.comment);
                        brokerReportItem.PaidTaxUSD = decimal.Parse(m.Groups["TAX_VALUE"].Value.Replace('.', ','));
                    }
                    else
                    {
                        brokerReportItem.PaidTaxUSD = 0;
                    }

                    if (comissionRegex.IsMatch(item.comment))
                    {
                        Match m = comissionRegex.Match(item.comment);
                        brokerReportItem.ComissionUSD = decimal.Parse(m.Groups["COMISSION_VALUE"].Value.Replace('.', ','));
                    }
                    else
                    {
                        brokerReportItem.ComissionUSD = 0;
                    }

                    brokerReportItem.TaxRate = Math.Round(100 * brokerReportItem.PaidTaxUSD / (brokerReportItem.PaidTaxUSD + brokerReportItem.AmountUSD));


                    brokerReportItems.Add(brokerReportItem);
                }
            }

            return brokerReportItems;
        }
    }
}
