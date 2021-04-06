using DividendTaxCalculatorLib.ExchangeRates;
using ServiceReference1;
using System;
using System.Collections.Generic;
using System.Data;

namespace Taxes2020
{
    class Program
    {
        public async static void LoadExchangeRates()
        {
            ExchangeRates rates = new ExchangeRates();
            rates.Items = new List<ExchangeRate>();

            DailyInfoSoapClient client = new DailyInfoSoapClient();

            DateTime firstDay = new DateTime(2020, 1, 1);

            for (int i = 0; i < 365; i++)
            {
                DateTime currentDate = firstDay.AddDays(i);

                DataSet dataSet = await client.GetCursOnDateAsync(currentDate);

                DataRow[] rows = dataSet.Tables["ValuteCursOnDate"].Select("VchCode = 'USD'");

                if (rows.Length != 1)
                {
                    throw new Exception("Invalid response from CBR");
                }

                decimal value = (decimal)rows[0]["Vcurs"];

                rates.Items.Add(new ExchangeRate() { Date = currentDate, Value = value });
            }

            string xml = XmlSerializationHelper.Serialize<ExchangeRates>(rates);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
