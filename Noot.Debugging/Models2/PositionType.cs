using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("PositionTypeID = {PositionTypeID}")]
    [Table("PositionType", Schema = "com")]
    public partial class PositionType
    {
        public PositionType()
        {
            OrganizationPositions = new HashSet<OrganizationPosition>();
            PositionTypeEAVs = new HashSet<PositionTypeEAV>();
            PositionTypeOccupations = new HashSet<PositionTypeOccupation>();
        }
        [Key]
        public int PositionTypeID { get; set; }

        [Required(ErrorMessage = "Name is a required field.")]
        [StringLength(256, ErrorMessage = "Name cannot exceed more than 256 characters.")]
        public string Name { get; set; }

        [StringLength(512, ErrorMessage = "Description cannot exceed more than 512 characters.")]
        public string Description { get; set; }

        public int? Rank { get; set; }

        [StringLength(256, ErrorMessage = "Color cannot exceed more than 256 characters.")]
        public string Color { get; set; }

        public int? ShapeID { get; set; }

        [ForeignKey(nameof(ShapeID))]
        public virtual PositionTypeShape Shape { get; set; }

        [Required(ErrorMessage = "Site ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Site ID must be greater than 0.")]
        public int SiteID { get; set; }

        [ForeignKey(nameof(SiteID))]
        public virtual Site Site { get; set; }

        public int? BorderID { get; set; }

        [ForeignKey(nameof(BorderID))]
        public virtual PositionTypeBorder Border { get; set; }

        [StringLength(256, ErrorMessage = "Border color cannot exceed more than 256 characters.")]
        public string BorderColor { get; set; }

        public double? BorderThickness { get; set; }

        public virtual ICollection<OrganizationPosition> OrganizationPositions { get; set; }

        public virtual ICollection<PositionTypeEAV> PositionTypeEAVs { get; set; }

        public virtual ICollection<PositionTypeOccupation> PositionTypeOccupations { get; set; }

    }
}

