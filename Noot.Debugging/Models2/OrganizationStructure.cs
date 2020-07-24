using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("OrganizationStructureID = {OrganizationStructureID}")]
    [Table("OrganizationStructure", Schema = "com")]
    public partial class OrganizationStructure
    {
        public OrganizationStructure()
        {
            OrganizationPositions = new HashSet<OrganizationPosition>();
            OrganizationStructureEAVs = new HashSet<OrganizationStructureEAV>();
        }
        [Key]
        public int OrganizationStructureID { get; set; }

        [Required(ErrorMessage = "Name is a required field.")]
        [StringLength(256, ErrorMessage = "Name cannot exceed more than 256 characters.")]
        public string Name { get; set; }

        [StringLength(512, ErrorMessage = "Description cannot exceed more than 512 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Effective date is a required field.")]
        public DateTime EffectiveDate { get; set; }

        [Required(ErrorMessage = "Site ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Site ID must be greater than 0.")]
        public int SiteID { get; set; }

        [ForeignKey(nameof(SiteID))]
        public virtual Site Site { get; set; }

        [Required(ErrorMessage = "Is active is a required field.")]
        public bool IsActive { get; set; }

        public virtual ICollection<OrganizationPosition> OrganizationPositions { get; set; }

        public virtual ICollection<OrganizationStructureEAV> OrganizationStructureEAVs { get; set; }

    }
}

