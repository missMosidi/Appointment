using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Appointment.Models
{
    public class BookingTime
    {
        [Key]
        public int TimeId { get; set; }
        [Display(Name ="Time"),Required]
       // [DataType(DataType.Time)]
        public string Time { get; set; }
    }
}