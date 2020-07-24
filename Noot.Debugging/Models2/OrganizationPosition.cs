using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("OrganizationPositionID = {OrganizationPositionID}")]
    [Table("OrganizationPosition", Schema = "com")]
    public partial class OrganizationPosition
    {
        public OrganizationPosition()
        {
            Assignments = new HashSet<Assignment>();
            EquipmentPositions = new HashSet<EquipmentPosition>();
            LocationPositions = new HashSet<LocationPosition>();
            OrganizationPositions = new HashSet<OrganizationPosition>();
            SchedulePositions = new HashSet<SchedulePosition>();
        }
        [Key]
        public int OrganizationPositionID { get; set; }

        [Required(ErrorMessage = "Name is a required field.")]
        [StringLength(256, ErrorMessage = "Name cannot exceed more than 256 characters.")]
        public string Name { get; set; }

        [StringLength(512, ErrorMessage = "Description cannot exceed more than 512 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Start date is a required field.")]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? SuperiorPositionID { get; set; }

        [ForeignKey(nameof(SuperiorPositionID))]
        public virtual OrganizationPosition SuperiorPosition { get; set; }

        [Required(ErrorMessage = "Organization structure ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Organization structure ID must be greater than 0.")]
        public int OrganizationStructureID { get; set; }

        [ForeignKey(nameof(OrganizationStructureID))]
        public virtual OrganizationStructure OrganizationStructure { get; set; }

        public int? PositionTypeID { get; set; }

        [ForeignKey(nameof(PositionTypeID))]
        public virtual PositionType PositionType { get; set; }

        public int? ExternalPositionID { get; set; }

        public virtual ICollection<Assignment> Assignments { get; set; }

        public virtual ICollection<EquipmentPosition> EquipmentPositions { get; set; }

        public virtual ICollection<LocationPosition> LocationPositions { get; set; }

        public virtual ICollection<OrganizationPosition> OrganizationPositions { get; set; }

        public virtual ICollection<SchedulePosition> SchedulePositions { get; set; }

    }
}

