using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("MapSetID = {MapSetID}")]
    [Table("MapSet", Schema = "com")]
    public partial class MapSet
    {
        public MapSet()
        {
            MapSetTargets = new HashSet<MapSetTarget>();
            MapSetValues = new HashSet<MapSetValue>();
        }
        [Key]
        public int MapSetID { get; set; }

        [Required(ErrorMessage = "Name is a required field.")]
        [StringLength(256, ErrorMessage = "Name cannot exceed more than 256 characters.")]
        public string Name { get; set; }

        [StringLength(512, ErrorMessage = "Description cannot exceed more than 512 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Master query ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Master query ID must be greater than 0.")]
        public int MasterQueryID { get; set; }

        [ForeignKey(nameof(MasterQueryID))]
        public virtual MapSetQuery MasterQuery { get; set; }

        public virtual ICollection<MapSetTarget> MapSetTargets { get; set; }

        public virtual ICollection<MapSetValue> MapSetValues { get; set; }

    }
}

