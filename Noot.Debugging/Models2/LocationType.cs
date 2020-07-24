using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("LocationTypeID = {LocationTypeID}")]
    [Table("LocationType", Schema = "com")]
    public partial class LocationType
    {
        public LocationType()
        {
            Locations = new HashSet<Location>();
            LocationTypeEAVs = new HashSet<LocationTypeEAV>();
            LocationTypeEntities = new HashSet<LocationTypeEntity>();
        }
        [Key]
        public int LocationTypeID { get; set; }

        [Required(ErrorMessage = "Name is a required field.")]
        [StringLength(256, ErrorMessage = "Name cannot exceed more than 256 characters.")]
        public string Name { get; set; }

        [StringLength(512, ErrorMessage = "Description cannot exceed more than 512 characters.")]
        public string Description { get; set; }

        public int? NamingStandardID { get; set; }

        [ForeignKey(nameof(NamingStandardID))]
        public virtual NamingStandard NamingStandard { get; set; }

        public int? DisplayNamingStandardID { get; set; }

        [ForeignKey(nameof(DisplayNamingStandardID))]
        public virtual NamingStandard DisplayNamingStandard { get; set; }

        [Required(ErrorMessage = "Is active is a required field.")]
        public bool IsActive { get; set; }

        public int? LocationTypeGroupID { get; set; }

        [ForeignKey(nameof(LocationTypeGroupID))]
        public virtual LocationTypeGroup LocationTypeGroup { get; set; }

        public virtual ICollection<Location> Locations { get; set; }

        public virtual ICollection<LocationTypeEAV> LocationTypeEAVs { get; set; }

        public virtual ICollection<LocationTypeEntity> LocationTypeEntities { get; set; }

    }
}

