using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("LocationPositionID = {LocationPositionID}")]
    [Table("LocationPosition", Schema = "com")]
    public partial class LocationPosition
    {
        public LocationPosition()
        {
        }
        [Key]
        public int LocationPositionID { get; set; }

        [Required(ErrorMessage = "Location ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Location ID must be greater than 0.")]
        public int LocationID { get; set; }

        [ForeignKey(nameof(LocationID))]
        public virtual Location Location { get; set; }

        [Required(ErrorMessage = "Organization position ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Organization position ID must be greater than 0.")]
        public int OrganizationPositionID { get; set; }

        [ForeignKey(nameof(OrganizationPositionID))]
        public virtual OrganizationPosition OrganizationPosition { get; set; }

        public DateTime? AssignmentStartDate { get; set; }

        public DateTime? AssignmentEndDate { get; set; }

    }
}

