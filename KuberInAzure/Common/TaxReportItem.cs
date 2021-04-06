using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DividendTaxCalculatorLib.Common
{
    /// <summary>
    /// Запись о дивидендах в брокерском отчете
    /// </summary>
    public class TaxReportItem
    { 
        /// <summary>
        /// Dividend date
        /// </summary>
        public DateTime OperationDate { get; set; }

        public string CurrencyCode { get; set; }

        public decimal AmountDirtyUSD { get; set; }

        public decimal AmountUSD { get; set; }

        public string Comment { get; set; }

        public string BrokerName { get; set; }

        public decimal ExchangeRate { get; set; }

        public decimal PaidTaxUSD { get; set; }

        public decimal TaxToPayUSD { get; set; }

        public decimal ComissionUSD { get; set; }

        public decimal AmountDirtyRUB { get; set; }

        public decimal AmountRUB { get; set; }

        public decimal PaidTaxRUB { get; set; }

        public decimal TaxToPayRUB { get; set; }

        public decimal TaxRate { get; set; }
    }
}
