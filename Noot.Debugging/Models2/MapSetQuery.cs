using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("QueryID = {QueryID}")]
    [Table("MapSetQuery", Schema = "com")]
    public partial class MapSetQuery
    {
        public MapSetQuery()
        {
            MapSets = new HashSet<MapSet>();
            MapSetTargets = new HashSet<MapSetTarget>();
        }
        [Key]
        public int QueryID { get; set; }

        [Required(ErrorMessage = "Data source ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Data source ID must be greater than 0.")]
        public int DataSourceID { get; set; }

        [Required(ErrorMessage = "Name is a required field.")]
        [StringLength(256, ErrorMessage = "Name cannot exceed more than 256 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Query text is a required field.")]
        public string QueryText { get; set; }

        [Required(ErrorMessage = "Identity column is a required field.")]
        [StringLength(256, ErrorMessage = "Identity column cannot exceed more than 256 characters.")]
        public string IdentityColumn { get; set; }

        public virtual ICollection<MapSet> MapSets { get; set; }

        public virtual ICollection<MapSetTarget> MapSetTargets { get; set; }

    }
}

