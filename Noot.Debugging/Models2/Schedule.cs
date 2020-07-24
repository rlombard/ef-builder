using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("ScheduleID = {ScheduleID}")]
    [Table("Schedule", Schema = "com")]
    public partial class Schedule
    {
        public Schedule()
        {
            EquipmentSchedules = new HashSet<EquipmentSchedule>();
            Events = new HashSet<Event>();
            SchedulePositions = new HashSet<SchedulePosition>();
        }
        [Key]
        public int ScheduleID { get; set; }

        [Required(ErrorMessage = "Site ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Site ID must be greater than 0.")]
        public int SiteID { get; set; }

        [ForeignKey(nameof(SiteID))]
        public virtual Site Site { get; set; }

        [Required(ErrorMessage = "Name is a required field.")]
        [StringLength(256, ErrorMessage = "Name cannot exceed more than 256 characters.")]
        public string Name { get; set; }

        [StringLength(510, ErrorMessage = "Description cannot exceed more than 510 characters.")]
        public string Description { get; set; }

        public virtual ICollection<EquipmentSchedule> EquipmentSchedules { get; set; }

        public virtual ICollection<Event> Events { get; set; }

        public virtual ICollection<SchedulePosition> SchedulePositions { get; set; }

    }
}

