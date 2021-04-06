using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DividendTaxCalculatorLib.Common
{
    public static class DividendHelper
    {
        private static readonly string[] ZeroTaxEmitents = new string[] {
        "Invesco Ltd", "Polymetal", "BNY Mellon", "ROS AGRO", "TCS GROUP", "VEON LTD-ADR", "MOBILE TELESYSTEMS-ADR", "MTS ADR", "РУСАГРО"
        };

        private static readonly string[] HighTaxEmitents = new string[] {
        "Energy Transfer"
        };

        public static decimal GetTaxRateByEmitent(string name)
        {
            foreach (string zero in ZeroTaxEmitents)
            {
                if (name.Contains(zero))
                {
                    return 0;
                }
            }

            foreach (string high in HighTaxEmitents)
            {
                if (name.Contains(high))
                {
                    return 37;
                }
            }

            return 10;
        }
    }
}
