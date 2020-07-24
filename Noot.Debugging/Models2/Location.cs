using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("LocationID = {LocationID}")]
    [Table("Location", Schema = "com")]
    public partial class Location
    {
        public Location()
        {
            EquipmentLocations = new HashSet<EquipmentLocation>();
            Locations = new HashSet<Location>();
            LocationEAVs = new HashSet<LocationEAV>();
            LocationGraphicReferences = new HashSet<LocationGraphicReference>();
            LocationPositions = new HashSet<LocationPosition>();
        }
        [Key]
        public int LocationID { get; set; }

        [Required(ErrorMessage = "Name is a required field.")]
        [StringLength(512, ErrorMessage = "Name cannot exceed more than 512 characters.")]
        public string Name { get; set; }

        [StringLength(256, ErrorMessage = "Display name cannot exceed more than 256 characters.")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "Location type ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Location type ID must be greater than 0.")]
        public int LocationTypeID { get; set; }

        [ForeignKey(nameof(LocationTypeID))]
        public virtual LocationType LocationType { get; set; }

        public int? GreaterLocationID { get; set; }

        [ForeignKey(nameof(GreaterLocationID))]
        public virtual Location GreaterLocation { get; set; }

        [Required(ErrorMessage = "Is active is a required field.")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Site ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Site ID must be greater than 0.")]
        public int SiteID { get; set; }

        [ForeignKey(nameof(SiteID))]
        public virtual Site Site { get; set; }

        [StringLength(256, ErrorMessage = "External ID cannot exceed more than 256 characters.")]
        public string ExternalID { get; set; }

        public virtual ICollection<EquipmentLocation> EquipmentLocations { get; set; }

        public virtual ICollection<Location> Locations { get; set; }

        public virtual ICollection<LocationEAV> LocationEAVs { get; set; }

        public virtual ICollection<LocationGraphicReference> LocationGraphicReferences { get; set; }

        public virtual ICollection<LocationPosition> LocationPositions { get; set; }

    }
}

