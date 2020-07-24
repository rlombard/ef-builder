using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("MapSetTargetID = {MapSetTargetID}")]
    [Table("MapSetTarget", Schema = "com")]
    public partial class MapSetTarget
    {
        public MapSetTarget()
        {
            MapSetValues = new HashSet<MapSetValue>();
        }
        [Key]
        public int MapSetTargetID { get; set; }

        [Required(ErrorMessage = "Map set ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Map set ID must be greater than 0.")]
        public int MapSetID { get; set; }

        [ForeignKey(nameof(MapSetID))]
        public virtual MapSet MapSet { get; set; }

        [Required(ErrorMessage = "Target query ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Target query ID must be greater than 0.")]
        public int TargetQueryID { get; set; }

        [ForeignKey(nameof(TargetQueryID))]
        public virtual MapSetQuery TargetQuery { get; set; }

        public virtual ICollection<MapSetValue> MapSetValues { get; set; }

    }
}

