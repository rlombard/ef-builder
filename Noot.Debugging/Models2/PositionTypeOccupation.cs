using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("RequirementID = {RequirementID}")]
    [Table("PositionTypeOccupation", Schema = "com")]
    public partial class PositionTypeOccupation
    {
        public PositionTypeOccupation()
        {
        }
        [Key]
        public int RequirementID { get; set; }

        [Required(ErrorMessage = "Position type ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Position type ID must be greater than 0.")]
        public int PositionTypeID { get; set; }

        [ForeignKey(nameof(PositionTypeID))]
        public virtual PositionType PositionType { get; set; }

        [Required(ErrorMessage = "Occupation ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Occupation ID must be greater than 0.")]
        public int OccupationID { get; set; }

        [ForeignKey(nameof(OccupationID))]
        public virtual Occupation Occupation { get; set; }

    }
}

