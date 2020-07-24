using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("IdentifierTypeID = {IdentifierTypeID}")]
    [Table("IdentifierType", Schema = "com")]
    public partial class IdentifierType
    {
        public IdentifierType()
        {
            PersonIdentifiers = new HashSet<PersonIdentifier>();
        }
        [Key]
        public int IdentifierTypeID { get; set; }

        [Required(ErrorMessage = "Name is a required field.")]
        [StringLength(256, ErrorMessage = "Name cannot exceed more than 256 characters.")]
        public string Name { get; set; }

        [StringLength(512, ErrorMessage = "Description cannot exceed more than 512 characters.")]
        public string Description { get; set; }

        [StringLength(512, ErrorMessage = "Authority name cannot exceed more than 512 characters.")]
        public string AuthorityName { get; set; }

        [Required(ErrorMessage = "Is numeric is a required field.")]
        public bool IsNumeric { get; set; }

        [Required(ErrorMessage = "Is mandatory is a required field.")]
        public bool IsMandatory { get; set; }

        [Required(ErrorMessage = "Is primary is a required field.")]
        public bool IsPrimary { get; set; }

        public virtual ICollection<PersonIdentifier> PersonIdentifiers { get; set; }

    }
}

