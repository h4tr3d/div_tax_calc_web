using DividendTaxCalculatorLib.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace DividendTaxCalculatorLib.BrokerReportAdapters.Open.SPB
{
    public class OpenSBPAdapter : IBrokerReportAdapter
    {
        public List<TaxReportItem> Parse(IFormFile file)
        {
            XPathDocument document = new XPathDocument(file.OpenReadStream());
            XPathNavigator navigator = document.CreateNavigator();

            navigator.MoveToChild("report", "");
            navigator.MoveToChild("nontrade_money_operation", "");

            nontrade_money_operations operations = XmlSerializationHelper.Deserialize<nontrade_money_operations>(navigator.OuterXml);

            List<TaxReportItem> brokerReportItems = new List<TaxReportItem>();

            Regex comissionRegex = new Regex(@"(?<COMISSION_ROOT>комиссия платежного агента\s[<](?<COMISSION_VALUE>\S+)[>]\sдоллар)");

            foreach (nontrade_money_operation item in operations.items)
            {
                if (item.currency_code == "USD" && 
                    item.operation_type == "Зачисление дивидендов")
                {
                    TaxReportItem brokerReportItem = new TaxReportItem();

                    brokerReportItem.BrokerName = "Открытие Брокер (СПб)";
                    brokerReportItem.OperationDate = item.operation_date;
                    brokerReportItem.AmountUSD = item.amount;
                    brokerReportItem.CurrencyCode = item.currency_code;
                    brokerReportItem.Comment = item.comment;

                    if (comissionRegex.IsMatch(item.comment))
                    {
                        Match m = comissionRegex.Match(item.comment);
                        brokerReportItem.ComissionUSD = decimal.Parse(m.Groups["COMISSION_VALUE"].Value.Replace('.', ','));
                    }
                    else
                    {
                        brokerReportItem.ComissionUSD = 0;
                    }

                    brokerReportItem.TaxRate = DividendHelper.GetTaxRateByEmitent(item.comment);
                    brokerReportItem.PaidTaxUSD = brokerReportItem.AmountUSD / (100 - brokerReportItem.TaxRate) * brokerReportItem.TaxRate;

                    brokerReportItems.Add(brokerReportItem);
                }
            }

            return brokerReportItems;
        }
    }
}
