using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("AppointmentID = {AppointmentID}")]
    [Table("Appointment", Schema = "com")]
    public partial class Appointment
    {
        public Appointment()
        {
        }
        [Key]
        public int AppointmentID { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Person ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Person ID must be greater than 0.")]
        public int PersonID { get; set; }

        [ForeignKey(nameof(PersonID))]
        public virtual Person Person { get; set; }

        public int? OccupationID { get; set; }

        [ForeignKey(nameof(OccupationID))]
        public virtual Occupation Occupation { get; set; }

        [Required(ErrorMessage = "Site ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Site ID must be greater than 0.")]
        public int SiteID { get; set; }

        [ForeignKey(nameof(SiteID))]
        public virtual Site Site { get; set; }

    }
}

