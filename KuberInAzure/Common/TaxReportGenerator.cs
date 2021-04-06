using DividendTaxCalculatorLib.BrokerReportAdapters.Open.MOEX;
using DividendTaxCalculatorLib.BrokerReportAdapters.Open.SPB;
using DividendTaxCalculatorLib.BrokerReportAdapters.VTB;
using DividendTaxCalculatorLib.ExchangeRates;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DividendTaxCalculatorLib.Common
{
    public static class TaxReportGenerator
    {
        public static List<TaxReportItem> GenerateTaxReport(IFormFile[] files)
        {
            List<TaxReportItem> joinedTaxReportItems = new List<TaxReportItem>();

            foreach (IFormFile file in files)
            {
                IBrokerReportAdapter brokerReportAdapter = CreateReportAdapter(file);

                List<TaxReportItem> taxReportItems = brokerReportAdapter.Parse(file);

                joinedTaxReportItems.AddRange(taxReportItems);
            }

            CalculateTaxes(joinedTaxReportItems);

            return joinedTaxReportItems;
        }

        public static void CalculateTaxes(List<TaxReportItem> joinedTaxReportItems)
        {
            foreach (TaxReportItem item in joinedTaxReportItems)
            {
                if (item.TaxRate >= 13)
                {
                    item.TaxToPayUSD = 0;
                }
                else
                {
                    item.TaxToPayUSD = item.AmountUSD / (100 - item.TaxRate) * (13 - item.TaxRate);
                }

                item.ExchangeRate = ExchangeRateHelper.GetExchangeRateByDate(item.OperationDate.Date);
                item.PaidTaxRUB  = item.PaidTaxUSD  * item.ExchangeRate;
                item.TaxToPayRUB = item.TaxToPayUSD * item.ExchangeRate;
                
                item.AmountRUB = item.AmountUSD * item.ExchangeRate;

                item.AmountDirtyUSD = item.AmountUSD + item.PaidTaxUSD;
                item.AmountDirtyRUB = item.AmountRUB + item.PaidTaxRUB;
            }
        }

        public static MemoryStream SaveTaxReport(List<TaxReportItem> taxReportItems)
        {
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(memoryStream, Encoding.UTF8);

            writer.WriteLine("Брокер\tДата\tВалюта\tКурс USD\tОбщая выплата (USD)\tДивиденд (USD)\tКомиссия (USD)\tСтавка налога\tУплаченный налог (USD)\tДоплата налога (USD)\tОбщая выплата (RUB)\tДивиденд (RUB)\tУплаченный налог (RUB)\tДоплата налога (RUB)\tОписание");

            foreach (TaxReportItem item in taxReportItems)
            {
                writer.WriteLine($"{item.BrokerName}\t{item.OperationDate.Date.ToShortDateString()}\t{item.CurrencyCode}\t{item.ExchangeRate.ToString("0.0000")}\t{item.AmountDirtyUSD.ToString("0.00")}\t{item.AmountUSD.ToString("0.00")}\t{item.ComissionUSD.ToString("0.00")}\t{item.TaxRate}\t{item.PaidTaxUSD.ToString("0.00")}\t{item.TaxToPayUSD.ToString("0.00")}\t{item.AmountDirtyRUB.ToString("0.00")}\t{item.AmountRUB.ToString("0.00")}\t{item.PaidTaxRUB.ToString("0.00")}\t{item.TaxToPayRUB.ToString("0.00")}\t{item.Comment}");
            }

            writer.Flush();

            memoryStream.Position = 0;

            return memoryStream;
        }

        private static IBrokerReportAdapter CreateReportAdapter(IFormFile file)
        {
            string fileName = new FileInfo(file.FileName).Name;

            if (fileName.StartsWith("Open_MOEX_"))
            {
                return new OpenMOEXAdapter();
            }
            if (fileName.StartsWith("Open_SPB_"))
            {
                return new OpenSBPAdapter();
            }
            if (fileName.StartsWith("VTB_"))
            {
                return new VTBAdapter();
            }
            else
            {
                throw new Exception($"The file {fileName} is invalid.");
            }
        }
    }
}
