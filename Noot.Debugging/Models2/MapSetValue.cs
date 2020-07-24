using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("MapSetValueID = {MapSetValueID}")]
    [Table("MapSetValue", Schema = "com")]
    public partial class MapSetValue
    {
        public MapSetValue()
        {
        }
        [Key]
        public int MapSetValueID { get; set; }

        [Required(ErrorMessage = "Map set ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Map set ID must be greater than 0.")]
        public int MapSetID { get; set; }

        [ForeignKey(nameof(MapSetID))]
        public virtual MapSet MapSet { get; set; }

        [Required(ErrorMessage = "Map set target ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Map set target ID must be greater than 0.")]
        public int MapSetTargetID { get; set; }

        [ForeignKey(nameof(MapSetTargetID))]
        public virtual MapSetTarget MapSetTarget { get; set; }

        [Required(ErrorMessage = "Master value is a required field.")]
        [StringLength(256, ErrorMessage = "Master value cannot exceed more than 256 characters.")]
        public string MasterValue { get; set; }

        [Required(ErrorMessage = "Target value is a required field.")]
        [StringLength(256, ErrorMessage = "Target value cannot exceed more than 256 characters.")]
        public string TargetValue { get; set; }

    }
}

