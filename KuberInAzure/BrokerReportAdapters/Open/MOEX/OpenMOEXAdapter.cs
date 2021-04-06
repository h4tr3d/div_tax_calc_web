using DividendTaxCalculatorLib.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace DividendTaxCalculatorLib.BrokerReportAdapters.Open.MOEX
{
    public class OpenMOEXAdapter : IBrokerReportAdapter
    {
        public List<TaxReportItem> Parse(IFormFile file)
        {
            List<base_non_trade_money_operation> allOperations = new List<base_non_trade_money_operation>();

            Regex comissionRegex = new Regex(@"(?<COMISSION_ROOT>комиссия платежного агента\s(?<COMISSION_VALUE>\S+)\sдоллар)");

            XPathDocument document = new XPathDocument(file.OpenReadStream());
            XPathNavigator navigator = document.CreateNavigator();

            navigator.MoveToChild("broker_report", "");

            if (navigator.MoveToChild("unified_non_trade_money_operations", ""))
            {
                unified_non_trade_money_operations operations = XmlSerializationHelper.Deserialize<unified_non_trade_money_operations>(navigator.OuterXml);

                allOperations.AddRange(operations.items);
            }

            if (navigator.MoveToChild("spot_non_trade_money_operations", ""))
            {
                spot_non_trade_money_operations operations = XmlSerializationHelper.Deserialize<spot_non_trade_money_operations>(navigator.OuterXml);

                allOperations.AddRange(operations.items);
            }

            List<TaxReportItem> brokerReportItems = new List<TaxReportItem>();

            foreach (base_non_trade_money_operation item in allOperations)
            {
                if (item.currency_code == "USD" && 
                    item.comment.Contains("Выплата дохода клиент") && 
                    item.comment.Contains("дивиденды"))
                {
                    TaxReportItem brokerReportItem = new TaxReportItem();

                    brokerReportItem.BrokerName = "Открытие Брокер (ММВБ)";
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
                   
                    //Будем считать, что по ГДР нет налогов
                    brokerReportItem.TaxRate = DividendHelper.GetTaxRateByEmitent(item.comment);
                    brokerReportItem.PaidTaxUSD = brokerReportItem.AmountUSD/(100 - brokerReportItem.TaxRate) * brokerReportItem.TaxRate; 

                    brokerReportItems.Add(brokerReportItem);
                }
            }

            return brokerReportItems;
        }
    }
}
