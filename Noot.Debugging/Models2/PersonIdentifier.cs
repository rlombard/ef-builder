using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("IdentifierID = {IdentifierID}")]
    [Table("PersonIdentifier", Schema = "com")]
    public partial class PersonIdentifier
    {
        public PersonIdentifier()
        {
        }
        [Key]
        public int IdentifierID { get; set; }

        [Required(ErrorMessage = "Identifier type ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Identifier type ID must be greater than 0.")]
        public int IdentifierTypeID { get; set; }

        [ForeignKey(nameof(IdentifierTypeID))]
        public virtual IdentifierType IdentifierType { get; set; }

        public int? NumericValue { get; set; }

        [StringLength(256, ErrorMessage = "Text value cannot exceed more than 256 characters.")]
        public string TextValue { get; set; }

        [Required(ErrorMessage = "Person ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Person ID must be greater than 0.")]
        public int PersonID { get; set; }

        [ForeignKey(nameof(PersonID))]
        public virtual Person Person { get; set; }

    }
}

