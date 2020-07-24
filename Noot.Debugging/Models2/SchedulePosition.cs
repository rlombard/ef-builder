using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("SchedulePositionID = {SchedulePositionID}")]
    [Table("SchedulePosition", Schema = "com")]
    public partial class SchedulePosition
    {
        public SchedulePosition()
        {
        }
        [Key]
        public int SchedulePositionID { get; set; }

        [Required(ErrorMessage = "Schedule ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Schedule ID must be greater than 0.")]
        public int ScheduleID { get; set; }

        [ForeignKey(nameof(ScheduleID))]
        public virtual Schedule Schedule { get; set; }

        [Required(ErrorMessage = "Organization position ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Organization position ID must be greater than 0.")]
        public int OrganizationPositionID { get; set; }

        [ForeignKey(nameof(OrganizationPositionID))]
        public virtual OrganizationPosition OrganizationPosition { get; set; }

    }
}

