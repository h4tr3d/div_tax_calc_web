using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DividendTaxCalculatorLib.Common
{
    public interface IBrokerReportAdapter
    {
        List<TaxReportItem> Parse(IFormFile file);
    }
}
