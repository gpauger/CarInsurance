using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarInsuranc.Models;
using CarInsurance.ViewModels;

namespace CarInsuranc.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            using (InsuranceEntities db = new InsuranceEntities())
            {
                List<Insuree> quotes = db.Insurees.Where(x => x.Quote > 0).ToList();
                var quoteVms = new List<QuoteVM>();
                foreach (var quote in quotes)
                {
                    var quoteVm = new QuoteVM();
                    quoteVm.FirstName = quote.FirstName;
                    quoteVm.LastName = quote.LastName;
                    quoteVm.EmailAddress = quote.EmailAddress;
                    quoteVm.Quote = Convert.ToString(quote.Quote);

                    quoteVms.Add(quoteVm);

                }
                return View(quoteVms);
            }
        }
    }
}