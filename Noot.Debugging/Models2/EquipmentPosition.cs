using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("EquipmentPositionID = {EquipmentPositionID}")]
    [Table("EquipmentPosition", Schema = "com")]
    public partial class EquipmentPosition
    {
        public EquipmentPosition()
        {
        }
        [Key]
        public int EquipmentPositionID { get; set; }

        [Required(ErrorMessage = "Equipment ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Equipment ID must be greater than 0.")]
        public int EquipmentID { get; set; }

        [ForeignKey(nameof(EquipmentID))]
        public virtual Equipment Equipment { get; set; }

        [Required(ErrorMessage = "Position ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Position ID must be greater than 0.")]
        public int PositionID { get; set; }

        [ForeignKey(nameof(PositionID))]
        public virtual OrganizationPosition Position { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

    }
}

