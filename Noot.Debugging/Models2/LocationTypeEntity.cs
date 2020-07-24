using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("LocationTypeEntityID = {LocationTypeEntityID}")]
    [Table("LocationTypeEntity", Schema = "com")]
    public partial class LocationTypeEntity
    {
        public LocationTypeEntity()
        {
        }
        [Key]
        public int LocationTypeEntityID { get; set; }

        [Required(ErrorMessage = "Location type ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Location type ID must be greater than 0.")]
        public int LocationTypeID { get; set; }

        [ForeignKey(nameof(LocationTypeID))]
        public virtual LocationType LocationType { get; set; }

        [Required(ErrorMessage = "Entity ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Entity ID must be greater than 0.")]
        public int EntityID { get; set; }

    }
}

