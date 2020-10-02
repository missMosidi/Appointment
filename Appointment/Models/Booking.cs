using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Appointment.Models
{
    public class Booking
    {
        [Key]
        public int BookId { get; set; }
        [Display(Name = "Member Email")]
        public string MemberEmail { get; set; }
        [Required]
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString ="{0:dd-MM-yy}")]
        [DataType(DataType.Date)]
        public DateTime BookDate { get; set; }
        [Display(Name ="Time"),Required]
        public int TimeId { get; set; }
        public virtual BookingTime bookingTime { get; set; }
        [Display(Name ="Hair Cut")]
        public string Hair_Cut { get; set; }
        public string Status { get; set; }
        [Display(Name ="Is Free?")]
        public bool IsFree { get; set; }
        [Display(Name ="Date Created")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yy}")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        public Style style { get; set; }
        public Nullable<int> StyleId { get; set; }
    }
}