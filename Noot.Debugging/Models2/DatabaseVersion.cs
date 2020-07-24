using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("DatabaseID = {DatabaseID}")]
    [Table("DatabaseVersion", Schema = "com")]
    public partial class DatabaseVersion
    {
        public DatabaseVersion()
        {
        }
        [Key]
        public int DatabaseID { get; set; }

        [Required(ErrorMessage = "Name is a required field.")]
        [StringLength(128, ErrorMessage = "Name cannot exceed more than 128 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is a required field.")]
        [StringLength(512, ErrorMessage = "Description cannot exceed more than 512 characters.")]
        public string Description { get; set; }

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

