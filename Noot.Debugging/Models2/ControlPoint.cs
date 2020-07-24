using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("ControlPointID = {ControlPointID}")]
    [Table("ControlPoint", Schema = "com")]
    public partial class ControlPoint
    {
        public ControlPoint()
        {
            ControlPoints = new HashSet<ControlPoint>();
            ControlPointEAVs = new HashSet<ControlPointEAV>();
        }
        [Key]
        public int ControlPointID { get; set; }

        public int? BacksightControlPointID { get; set; }

        [ForeignKey(nameof(BacksightControlPointID))]
        public virtual ControlPoint BacksightControlPoint { get; set; }

        public int? SiteID { get; set; }

        public int? CreatorID { get; set; }

        [Required(ErrorMessage = "Control point name is a required field.")]
        [StringLength(50, ErrorMessage = "Control point name cannot exceed more than 50 characters.")]
        public string ControlPointName { get; set; }

        [Required(ErrorMessage = "X is a required field.")]
        public double X { get; set; }

        [Required(ErrorMessage = "Y is a required field.")]
        public double Y { get; set; }

        [Required(ErrorMessage = "Z is a required field.")]
        public double Z { get; set; }

        public double? Bearing { get; set; }

        public double? GradeElev { get; set; }

        public DateTime? DateTime { get; set; }

        public bool? IsLineControlPoint { get; set; }

        public bool? IsAuthorized { get; set; }

        public bool? IsCheckedControlPoint { get; set; }

        [Required(ErrorMessage = "X transformed is a required field.")]
        public double XTransformed { get; set; }

        [Required(ErrorMessage = "Y transformed is a required field.")]
        public double YTransformed { get; set; }

        [Required(ErrorMessage = "Z transformed is a required field.")]
        public double ZTransformed { get; set; }

        [StringLength(100, ErrorMessage = "Group ID cannot exceed more than 100 characters.")]
        public string GroupID { get; set; }

        public int? ControlPointTypeID { get; set; }

        [ForeignKey(nameof(ControlPointTypeID))]
        public virtual ControlPointType ControlPointType { get; set; }

        public virtual ICollection<ControlPoint> ControlPoints { get; set; }

        public virtual ICollection<ControlPointEAV> ControlPointEAVs { get; set; }

    }
}

