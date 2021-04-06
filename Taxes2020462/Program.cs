using DividendTaxCalculatorLib.ExchangeRates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taxes2020462
{
    class Program
    {
        static void Main(string[] args)
        {
            ExchangeRateHelper.LoadExchangeRates();
        }
    }
}
