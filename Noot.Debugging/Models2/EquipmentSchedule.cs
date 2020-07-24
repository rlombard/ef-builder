using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("EquipmentScheduleID = {EquipmentScheduleID}")]
    [Table("EquipmentSchedule", Schema = "com")]
    public partial class EquipmentSchedule
    {
        public EquipmentSchedule()
        {
        }
        [Key]
        public int EquipmentScheduleID { get; set; }

        [Required(ErrorMessage = "Equipment ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Equipment ID must be greater than 0.")]
        public int EquipmentID { get; set; }

        [ForeignKey(nameof(EquipmentID))]
        public virtual Equipment Equipment { get; set; }

        [Required(ErrorMessage = "Schedule ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Schedule ID must be greater than 0.")]
        public int ScheduleID { get; set; }

        [ForeignKey(nameof(ScheduleID))]
        public virtual Schedule Schedule { get; set; }

    }
}

