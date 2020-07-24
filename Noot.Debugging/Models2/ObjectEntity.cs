using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("ObjectEntityID = {ObjectEntityID}")]
    [Table("ObjectEntity", Schema = "com")]
    public partial class ObjectEntity
    {
        public ObjectEntity()
        {
        }
        [Key]
        public int ObjectEntityID { get; set; }

        [Required(ErrorMessage = "Object name is a required field.")]
        [StringLength(512, ErrorMessage = "Object name cannot exceed more than 512 characters.")]
        public string ObjectName { get; set; }

        [Required(ErrorMessage = "Entity ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Entity ID must be greater than 0.")]
        public int EntityID { get; set; }

        public int? LibraryID { get; set; }

    }
}

