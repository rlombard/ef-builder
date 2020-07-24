using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("NamingStandardID = {NamingStandardID}")]
    [Table("NamingStandard", Schema = "com")]
    public partial class NamingStandard
    {
        public NamingStandard()
        {
            LocationTypes = new HashSet<LocationType>();
        }
        [Key]
        public int NamingStandardID { get; set; }

        [Required(ErrorMessage = "Name is a required field.")]
        [StringLength(256, ErrorMessage = "Name cannot exceed more than 256 characters.")]
        public string Name { get; set; }

        [StringLength(512, ErrorMessage = "Description cannot exceed more than 512 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Expression is a required field.")]
        public string Expression { get; set; }

        public virtual ICollection<LocationType> LocationTypes { get; set; }

    }
}

