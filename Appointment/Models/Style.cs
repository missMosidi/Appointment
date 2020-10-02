using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Appointment.Models
{
    public class Style
    {
        public Style()
        {

        }
        [Key]
        public int StyleId { get; set; }
        [Display(Name ="Hair Style"),Required]
        public string Hair_Cut { get; set; }
        [Display(Name ="Price"),Required]
        public double Price { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}