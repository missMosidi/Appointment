using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Appointment.Models
{
    public class Logic
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public int CountBookings(string email)
        {
            //int count = 0;
            List<Booking> book = db.Bookings.ToList().FindAll(x => x.MemberEmail.Equals(email));
            return book.Count;
        }
        public bool isFree(string email)
        {
            bool free = false;

            if (CountBookings(email) >= 4)
            {
                free = true;
            }
            return free;
        }
        public string HairCutName(int? styleid)
        {
            Style style = db.Styles.Find(styleid);
            return style.Hair_Cut;
        }
    }
}