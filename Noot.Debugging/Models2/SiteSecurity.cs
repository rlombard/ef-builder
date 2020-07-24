using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("SiteSecurityID = {SiteSecurityID}")]
    [Table("SiteSecurity", Schema = "com")]
    public partial class SiteSecurity
    {
        public SiteSecurity()
        {
        }
        [Key]
        public int SiteSecurityID { get; set; }

        [Required(ErrorMessage = "Site ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Site ID must be greater than 0.")]
        public int SiteID { get; set; }

        [ForeignKey(nameof(SiteID))]
        public virtual Site Site { get; set; }

        public int? SiteUserID { get; set; }

        public int? SiteRoleID { get; set; }

    }
}

