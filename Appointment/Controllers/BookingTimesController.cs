using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Appointment.Models;

namespace Appointment.Controllers
{
    public class BookingTimesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BookingTimes
        public ActionResult Index()
        {
            return View(db.BookingTimes.ToList());
        }

        // GET: BookingTimes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookingTime bookingTime = db.BookingTimes.Find(id);
            if (bookingTime == null)
            {
                return HttpNotFound();
            }
            return View(bookingTime);
        }

        // GET: BookingTimes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BookingTimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TimeId,Time")] BookingTime bookingTime)
        {
            if (ModelState.IsValid)
            {
                db.BookingTimes.Add(bookingTime);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bookingTime);
        }

        // GET: BookingTimes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookingTime bookingTime = db.BookingTimes.Find(id);
            if (bookingTime == null)
            {
                return HttpNotFound();
            }
            return View(bookingTime);
        }

        // POST: BookingTimes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TimeId,Time")] BookingTime bookingTime)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookingTime).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bookingTime);
        }

        // GET: BookingTimes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookingTime bookingTime = db.BookingTimes.Find(id);
            if (bookingTime == null)
            {
                return HttpNotFound();
            }
            return View(bookingTime);
        }

        // POST: BookingTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookingTime bookingTime = db.BookingTimes.Find(id);
            db.BookingTimes.Remove(bookingTime);
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
