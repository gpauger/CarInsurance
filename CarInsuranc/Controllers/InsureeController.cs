using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarInsuranc.Models;

namespace CarInsuranc.Controllers
{
    public class InsureeController : Controller
    {
        private InsuranceEntities db = new InsuranceEntities();

        // GET: Insuree
        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }

        // GET: Insuree/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // GET: Insuree/Create
        [HttpPost]
        public ActionResult Create(string firstName, string lastName, string emailAddress,
                                        DateTime DateOfBirth, int carYear, string carMake, string carModel,
                                        int SpeedingTickets, bool dUI, bool CoverageType)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(emailAddress) || string.IsNullOrEmpty(Convert.ToString(DateOfBirth)) ||
                string.IsNullOrEmpty(Convert.ToString(carYear)) || string.IsNullOrEmpty(carMake) ||
                string.IsNullOrEmpty(carModel) || string.IsNullOrEmpty(Convert.ToString(SpeedingTickets)) ||
                string.IsNullOrEmpty(Convert.ToString(dUI)) || string.IsNullOrEmpty(Convert.ToString(CoverageType)))

            {
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                using (InsuranceEntities db = new InsuranceEntities())
                {
                    var insuree = new Insuree();
                    insuree.FirstName = firstName;
                    insuree.LastName = lastName;
                    insuree.EmailAddress = emailAddress;
                    insuree.DateOfBirth = Convert.ToDateTime(DateOfBirth);
                    insuree.CarYear = Convert.ToInt32(carYear);
                    insuree.CarMake = carMake;
                    insuree.CarModel = carModel;
                    insuree.SpeedingTickets = Convert.ToInt32(SpeedingTickets);
                    insuree.DUI = Convert.ToBoolean(dUI);
                    insuree.CoverageType = Convert.ToBoolean(CoverageType);

                    var today = DateTime.Now;
                    var age = today.Year - insuree.DateOfBirth.Year;
                    decimal x = 50 + (10 * insuree.SpeedingTickets);

                    if (age < 18)
                    {
                        x = x + 100;
                    }
                    else if (age < 18)
                    {
                        x = x + 25;
                    }
                    else if (age > 100)
                    {
                        x = x + 100;
                    }
                    if (insuree.CarYear < 2000)
                    {
                        x = x + 25;
                    }
                    if (insuree.CarYear > 2015)
                    {
                        x = x + 25;
                    }
                    if (insuree.CarMake == "Porsche")
                    {
                        x = x + 25;
                    }
                    if (insuree.CarMake == "Porsche" && insuree.CarModel == "911 Carrera")
                    {
                        x = x + 25;
                    }
                    if (insuree.DUI == true)
                    {
                        x = x + (x / 4);
                    }
                    if (insuree.CoverageType == true)
                    {
                        x = x + (x / 2);
                    }




                    insuree.Quote = x;

                    db.Insurees.Add(insuree);
                    db.SaveChanges();

                    int userid = insuree.Id;
                    //return RedirectToAction("Success", userid);
                    return View("Success", insuree);
                }
            }
        }
        public ActionResult Success(int ID)
        {
            var insuree = db.Insurees.SingleOrDefault(i => i.Id == ID);
            return View(insuree);


        }

        // POST: Insuree/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
     
        public ActionResult Create()
        { 
           
            return View();
        }

        // GET: Insuree/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insuree).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insuree);
        }

        // GET: Insuree/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Insuree insuree = db.Insurees.Find(id);
            db.Insurees.Remove(insuree);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
