using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("EventID = {EventID}")]
    [Table("EventRecurrence", Schema = "com")]
    public partial class EventRecurrence
    {
        public EventRecurrence()
        {
        }
        [Key]
        public int EventID { get; set; }

        [ForeignKey(nameof(EventID))]
        public virtual Event Event { get; set; }

        public int? DayOrdinal { get; set; }

        [StringLength(200, ErrorMessage = "Days of month cannot exceed more than 200 characters.")]
        public string DaysOfMonth { get; set; }

        [Required(ErrorMessage = "Days of week is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Days of week must be greater than 0.")]
        public int DaysOfWeek { get; set; }

        [Required(ErrorMessage = "First day of week is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for First day of week must be greater than 0.")]
        public int FirstDayOfWeek { get; set; }

        [Required(ErrorMessage = "Frequency is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Frequency must be greater than 0.")]
        public int Frequency { get; set; }

        [StringLength(200, ErrorMessage = "Hours of day cannot exceed more than 200 characters.")]
        public string HoursOfDay { get; set; }

        [StringLength(500, ErrorMessage = "Minutes of hour cannot exceed more than 500 characters.")]
        public string MinutesOfHour { get; set; }

        [Required(ErrorMessage = "Interval is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Interval must be greater than 0.")]
        public int Interval { get; set; }

        public int? MaxOccurrences { get; set; }

        public int? MonthOfYear { get; set; }

        public DateTime? RecursUntil { get; set; }

    }
}

