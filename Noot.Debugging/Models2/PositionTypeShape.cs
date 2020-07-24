using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("ShapeID = {ShapeID}")]
    [Table("PositionTypeShape", Schema = "com")]
    public partial class PositionTypeShape
    {
        public PositionTypeShape()
        {
            PositionTypes = new HashSet<PositionType>();
        }
        [Key]
        public int ShapeID { get; set; }

        [Required(ErrorMessage = "Name is a required field.")]
        [StringLength(128, ErrorMessage = "Name cannot exceed more than 128 characters.")]
        public string Name { get; set; }

        public virtual ICollection<PositionType> PositionTypes { get; set; }

    }
}

