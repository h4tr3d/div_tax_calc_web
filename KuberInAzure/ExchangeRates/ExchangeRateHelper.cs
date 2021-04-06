using DividendTaxCalculatorLib.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DividendTaxCalculatorLib.ExchangeRates
{
    public static class ExchangeRateHelper
    {
        private static readonly int[] YEARs = new[] { 2019, 2020};

        private static Dictionary<DateTime, decimal> exhangeRatesHash;

        static ExchangeRateHelper()
        {
            exhangeRatesHash = new Dictionary<DateTime, decimal>();

            foreach(int year in YEARs)
            {
                using (Stream stream = typeof(ExchangeRateHelper).Assembly.GetManifestResourceStream($"KuberInAzure.ExchangeRates.USD_RUB_{year}.xml"))
                {
                    ExchangeRates exhangeRates = XmlSerializationHelper.Deserialize<ExchangeRates>(stream);

                    foreach (ExchangeRate item in exhangeRates.Items)
                    {
                        exhangeRatesHash.Add(item.Date, item.Value);
                    }
                }
            }
        }

        public static decimal GetExchangeRateByDate(DateTime dateTime)
        {
            return exhangeRatesHash[dateTime];
        }

        //public static void LoadExchangeRates()
        //{
        //    ExchangeRates rates = new ExchangeRates();
        //    rates.Items = new List<ExchangeRate>();

        //    cbr.DailyInfoSoapClient client = new cbr.DailyInfoSoapClient();

        //    DateTime firstDay = new DateTime(YEAR, 1, 1);

        //    for (int i = 0; i < 365; i++)
        //    {
        //        DateTime currentDate = firstDay.AddDays(i);

        //        DataSet dataSet = client.GetCursOnDate(currentDate);

        //        DataRow[] rows = dataSet.Tables["ValuteCursOnDate"].Select("VchCode = 'USD'");

        //        if (rows.Length != 1)
        //        {
        //            throw new Exception("Invalid response from CBR");
        //        }

        //        decimal value = (decimal)rows[0]["Vcurs"];

        //        rates.Items.Add(new ExchangeRate() { Date = currentDate, Value = value });
        //    }

        //    string xml = XmlSerializationHelper.Serialize<ExchangeRates>(rates);
        //}
    }
}
