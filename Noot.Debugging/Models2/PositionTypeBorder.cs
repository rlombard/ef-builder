using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("BorderID = {BorderID}")]
    [Table("PositionTypeBorder", Schema = "com")]
    public partial class PositionTypeBorder
    {
        public PositionTypeBorder()
        {
            PositionTypes = new HashSet<PositionType>();
        }
        [Key]
        public int BorderID { get; set; }

        [Required(ErrorMessage = "Name is a required field.")]
        [StringLength(64, ErrorMessage = "Name cannot exceed more than 64 characters.")]
        public string Name { get; set; }

        public virtual ICollection<PositionType> PositionTypes { get; set; }

    }
}

