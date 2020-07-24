using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("EquipmentID = {EquipmentID}")]
    [Table("Equipment", Schema = "com")]
    public partial class Equipment
    {
        public Equipment()
        {
            EquipmentEAVs = new HashSet<EquipmentEAV>();
            EquipmentLocations = new HashSet<EquipmentLocation>();
            EquipmentPositions = new HashSet<EquipmentPosition>();
            EquipmentSchedules = new HashSet<EquipmentSchedule>();
        }
        [Key]
        public int EquipmentID { get; set; }

        [Required(ErrorMessage = "Name is a required field.")]
        [StringLength(256, ErrorMessage = "Name cannot exceed more than 256 characters.")]
        public string Name { get; set; }

        [StringLength(512, ErrorMessage = "Description cannot exceed more than 512 characters.")]
        public string Description { get; set; }

        [StringLength(512, ErrorMessage = "Serial number cannot exceed more than 512 characters.")]
        public string SerialNumber { get; set; }

        [StringLength(512, ErrorMessage = "Asset code cannot exceed more than 512 characters.")]
        public string AssetCode { get; set; }

        [Required(ErrorMessage = "Start date is a required field.")]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Site ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Site ID must be greater than 0.")]
        public int SiteID { get; set; }

        [ForeignKey(nameof(SiteID))]
        public virtual Site Site { get; set; }

        [Required(ErrorMessage = "Equipment type ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Equipment type ID must be greater than 0.")]
        public int EquipmentTypeID { get; set; }

        [ForeignKey(nameof(EquipmentTypeID))]
        public virtual EquipmentType EquipmentType { get; set; }

        [Required(ErrorMessage = "Equipment status ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Equipment status ID must be greater than 0.")]
        public int EquipmentStatusID { get; set; }

        [ForeignKey(nameof(EquipmentStatusID))]
        public virtual EquipmentStatus EquipmentStatus { get; set; }

        [Required(ErrorMessage = "Is active is a required field.")]
        public bool IsActive { get; set; }

        public virtual ICollection<EquipmentEAV> EquipmentEAVs { get; set; }

        public virtual ICollection<EquipmentLocation> EquipmentLocations { get; set; }

        public virtual ICollection<EquipmentPosition> EquipmentPositions { get; set; }

        public virtual ICollection<EquipmentSchedule> EquipmentSchedules { get; set; }

    }
}

