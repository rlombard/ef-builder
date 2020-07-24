using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("SiteID = {SiteID}")]
    [Table("Site", Schema = "com")]
    public partial class Site
    {
        public Site()
        {
            Appointments = new HashSet<Appointment>();
            Equipment = new HashSet<Equipment>();
            EventCategories = new HashSet<EventCategory>();
            Locations = new HashSet<Location>();
            LocationTypeGroups = new HashSet<LocationTypeGroup>();
            Materials = new HashSet<Material>();
            OrganizationStructures = new HashSet<OrganizationStructure>();
            People = new HashSet<Person>();
            PositionTypes = new HashSet<PositionType>();
            Schedules = new HashSet<Schedule>();
            Sites = new HashSet<Site>();
            SiteEAVs = new HashSet<SiteEAV>();
            SiteSecurities = new HashSet<SiteSecurity>();
        }
        [Key]
        public int SiteID { get; set; }

        [Required(ErrorMessage = "Name is a required field.")]
        [StringLength(256, ErrorMessage = "Name cannot exceed more than 256 characters.")]
        public string Name { get; set; }

        [StringLength(512, ErrorMessage = "Description cannot exceed more than 512 characters.")]
        public string Description { get; set; }

        [StringLength(128, ErrorMessage = "Code cannot exceed more than 128 characters.")]
        public string Code { get; set; }

        public byte[] Logo { get; set; }

        [Required(ErrorMessage = "Is active is a required field.")]
        public bool IsActive { get; set; }

        [StringLength(512, ErrorMessage = "External ID cannot exceed more than 512 characters.")]
        public string ExternalID { get; set; }

        public int? SuperiorSiteID { get; set; }

        [ForeignKey(nameof(SuperiorSiteID))]
        public virtual Site SuperiorSite { get; set; }

        [StringLength(500, ErrorMessage = "Time zone cannot exceed more than 500 characters.")]
        public string TimeZone { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }

        public virtual ICollection<Equipment> Equipment { get; set; }

        public virtual ICollection<EventCategory> EventCategories { get; set; }

        public virtual ICollection<Location> Locations { get; set; }

        public virtual ICollection<LocationTypeGroup> LocationTypeGroups { get; set; }

        public virtual ICollection<Material> Materials { get; set; }

        public virtual ICollection<OrganizationStructure> OrganizationStructures { get; set; }

        public virtual ICollection<Person> People { get; set; }

        public virtual ICollection<PositionType> PositionTypes { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }

        public virtual ICollection<Site> Sites { get; set; }

        public virtual ICollection<SiteEAV> SiteEAVs { get; set; }

        public virtual ICollection<SiteSecurity> SiteSecurities { get; set; }

    }
}

