using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("EquipmentLocationID = {EquipmentLocationID}")]
    [Table("EquipmentLocation", Schema = "com")]
    public partial class EquipmentLocation
    {
        public EquipmentLocation()
        {
        }
        [Key]
        public int EquipmentLocationID { get; set; }

        [Required(ErrorMessage = "Equipment ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Equipment ID must be greater than 0.")]
        public int EquipmentID { get; set; }

        [ForeignKey(nameof(EquipmentID))]
        public virtual Equipment Equipment { get; set; }

        [Required(ErrorMessage = "Location ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Location ID must be greater than 0.")]
        public int LocationID { get; set; }

        [ForeignKey(nameof(LocationID))]
        public virtual Location Location { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

    }
}

