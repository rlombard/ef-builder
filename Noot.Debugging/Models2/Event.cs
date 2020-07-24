using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("EventID = {EventID}")]
    [Table("Event", Schema = "com")]
    public partial class Event
    {
        public Event()
        {
            Events = new HashSet<Event>();
            EventRecurrences = new HashSet<EventRecurrence>();
        }
        [Key]
        public int EventID { get; set; }

        [Required(ErrorMessage = "Schedule ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Schedule ID must be greater than 0.")]
        public int ScheduleID { get; set; }

        [ForeignKey(nameof(ScheduleID))]
        public virtual Schedule Schedule { get; set; }

        public int? EventCategoryID { get; set; }

        [ForeignKey(nameof(EventCategoryID))]
        public virtual EventCategory EventCategory { get; set; }

        public int? ExceptionForEventID { get; set; }

        [ForeignKey(nameof(ExceptionForEventID))]
        public virtual Event ExceptionForEvent { get; set; }

        [Required(ErrorMessage = "Start date is a required field.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is a required field.")]
        public DateTime EndDate { get; set; }

        [StringLength(510, ErrorMessage = "Subject cannot exceed more than 510 characters.")]
        public string Subject { get; set; }

        public string Body { get; set; }

        [Required(ErrorMessage = "Is all day is a required field.")]
        public bool IsAllDay { get; set; }

        [Required(ErrorMessage = "Importance is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Importance must be greater than 0.")]
        public int Importance { get; set; }

        [Required(ErrorMessage = "Is excluded is a required field.")]
        public bool IsExcluded { get; set; }

        public virtual ICollection<Event> Events { get; set; }

        public virtual ICollection<EventRecurrence> EventRecurrences { get; set; }

    }
}

