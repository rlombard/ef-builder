using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("Schema = {Schema}")]
    [Table("SchemaVersion", Schema = "com")]
    public partial class SchemaVersion
    {
        public SchemaVersion()
        {
        }
        [Required(ErrorMessage = "Major is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Major must be greater than 0.")]
        public int Major { get; set; }

        [Required(ErrorMessage = "Minor is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Minor must be greater than 0.")]
        public int Minor { get; set; }

        [Required(ErrorMessage = "Revision is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Revision must be greater than 0.")]
        public int Revision { get; set; }

        [Required(ErrorMessage = "Implementation date is a required field.")]
        public DateTime ImplementationDate { get; set; }

        public string Comment { get; set; }

    }
}

