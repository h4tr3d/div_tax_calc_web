using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DividendTaxCalculatorLib.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace KuberInAzure.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("ru-RU");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            List<TaxReportItem> taxReportItems = TaxReportGenerator.GenerateTaxReport(Request.Form.Files.ToArray());

            MemoryStream memoryStream = TaxReportGenerator.SaveTaxReport(taxReportItems);

            await Task.Delay(10);

            return new FileStreamResult(memoryStream, @"text/plain")
            {
                FileDownloadName = "Tax_Report.txt"
            };
        }

        public void OnGet()
        {

        }
    }
}
