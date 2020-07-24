using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("ControlPointTypeID = {ControlPointTypeID}")]
    [Table("ControlPointType", Schema = "com")]
    public partial class ControlPointType
    {
        public ControlPointType()
        {
            ControlPoints = new HashSet<ControlPoint>();
        }
        [Key]
        public int ControlPointTypeID { get; set; }

        [Required(ErrorMessage = "Name is a required field.")]
        [StringLength(256, ErrorMessage = "Name cannot exceed more than 256 characters.")]
        public string Name { get; set; }

        [StringLength(512, ErrorMessage = "Description cannot exceed more than 512 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Is active is a required field.")]
        public bool IsActive { get; set; }

        public virtual ICollection<ControlPoint> ControlPoints { get; set; }

    }
}

