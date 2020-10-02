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
    [Authorize(Roles ="Customer,Admin,Clerk,Manager")]
    public class BookingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Logic lo = new Logic();
        // GET: Bookings
    
        public ActionResult ListOfBookings()
        {
            return View(db.Bookings.Where(i => i.Status == "Not Yet Confirmed").ToList());
        }

        public ActionResult ListOfConfirmedBookings()
        {
            return View(db.Bookings.Where(i => i.Status == "Confirmed").ToList());

        }

        public ActionResult Index()
        {
            var bookings = db.Bookings.Include(b => b.bookingTime).Include(b => b.style);
            return View(bookings.ToList());
        }
        public string Confirm(int? BookId)
        {
            Booking book = db.Bookings.Find(BookId);
            book.Status = "Confirmed";
            db.SaveChanges();
            return "Appointment has been confirmed";
        }
        public ActionResult Confirmed(int? id)
        {
            Booking book = db.Bookings.Find(id);
            string search = null;
            search = Confirm(id);
            ViewBag.Seacrh = search;

            return View(book);
        }
        public ActionResult ConfirmForUser(int? id)
        {
            Booking booking = db.Bookings.Find(id);
            string search = null;
            search = Confirm(id);
            ViewBag.Seacrh = search;
            Booking b = db.Bookings.ToList().Last();
            return View(booking);
        }

        public ActionResult ViewStatus()
        {
            string m = HttpContext.User.Identity.Name;
            List<Booking> b = db.Bookings.ToList().FindAll(p => p.MemberEmail == m);
            foreach (var item in b)
            {
                if (item.Status=="Confirmed")
                {
                    ViewBag.c = "Confirmed";
                }
                if(item.Status=="Not Yet Confirmed")
                {
                    ViewBag.n = "Not Yet Confirmed";
                }
            }
            return View(b);
        }
        public ActionResult DateBooked()
        {
            return View();
        }
        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }
     
        public ActionResult Create()
        {
            ViewBag.TimeId = new SelectList(db.BookingTimes, "TimeId", "Time");
            ViewBag.StyleId = new SelectList(db.Styles, "StyleId", "Hair_Cut");
            return View();
        }

        
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookId,MemberEmail,BookDate,TimeId,Hair_Cut,Status,IsFree,DateCreated,StyleId")] Booking booking)
        {
            int result = DateTime.Compare(DateTime.Now, booking.BookDate);
            string errorMessage = null;

            //Booking newTime = db.Bookings.Find(booking.TimeId);
            //Booking newDate = db.Bookings.Find(booking.BookDate);
            int sTime = db.Bookings.Where(p => p.TimeId == booking.TimeId).Select(p => p.TimeId).FirstOrDefault();
            DateTime sDate = db.Bookings.Where(p => p.BookDate == booking.BookDate).Select(p => p.BookDate).FirstOrDefault();
            if ((booking.BookDate == sDate) && (booking.TimeId == sTime))
            {
                errorMessage = "Time for this date has been booked. Please pick a different time or date.";

            }
            else
            {
                if (result > 0)
                {
                    errorMessage = "Date chosen has passed.";
                }
                else if (result == 0)
                {
                    errorMessage = "You cannot book today's date.";
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        if (lo.CountBookings(User.Identity.Name) >= 4)
                        {
                            Booking b = new Booking
                            {
                                MemberEmail = HttpContext.User.Identity.Name,
                                BookDate = booking.BookDate,
                                TimeId = booking.TimeId,
                                style = booking.style,
                                Status = "Not Yet Confirmed",
                                DateCreated = DateTime.Now,
                                StyleId = booking.StyleId,
                                IsFree = true
                            };
                            db.Bookings.Add(b);
                            db.SaveChanges();
                            TempData["AlertMessage"] = "This hair cut is free";
                            return RedirectToAction("DateBooked", "Bookings");
                        }
                        else if (lo.CountBookings(User.Identity.Name) < 4)
                        {
                            Booking b = new Booking
                            {
                                MemberEmail = HttpContext.User.Identity.Name,
                                BookDate = booking.BookDate,
                                TimeId = booking.TimeId,
                                style = booking.style,
                                Status = "Not Yet Confirmed",
                                DateCreated = DateTime.Now,
                                StyleId = booking.StyleId,
                                IsFree = false
                            };
                            db.Bookings.Add(b);
                            db.SaveChanges();
                            return RedirectToAction("DateBooked", "Bookings");
                        }
                    }
                }
            }
            ViewBag.Error = errorMessage;
            ViewBag.TimeId = new SelectList(db.BookingTimes, "TimeId", "Time", booking.TimeId);
            ViewBag.StyleId = new SelectList(db.Styles, "StyleId", "Hair_Cut", booking.StyleId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.TimeId = new SelectList(db.BookingTimes, "TimeId", "Time", booking.TimeId);
            ViewBag.StyleId = new SelectList(db.Styles, "StyleId", "Hair_Cut", booking.StyleId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookId,MemberEmail,BookDate,TimeId,Hair_Cut,Status,IsFree,DateCreated,StyleId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TimeId = new SelectList(db.BookingTimes, "TimeId", "Time", booking.TimeId);
            ViewBag.StyleId = new SelectList(db.Styles, "StyleId", "Hair_Cut", booking.StyleId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
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
