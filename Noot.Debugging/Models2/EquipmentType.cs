using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("EquipmentTypeID = {EquipmentTypeID}")]
    [Table("EquipmentType", Schema = "com")]
    public partial class EquipmentType
    {
        public EquipmentType()
        {
            Equipment = new HashSet<Equipment>();
            EquipmentTypeEAVs = new HashSet<EquipmentTypeEAV>();
        }
        [Key]
        public int EquipmentTypeID { get; set; }

        [Required(ErrorMessage = "Name is a required field.")]
        [StringLength(256, ErrorMessage = "Name cannot exceed more than 256 characters.")]
        public string Name { get; set; }

        [StringLength(512, ErrorMessage = "Description cannot exceed more than 512 characters.")]
        public string Description { get; set; }

        [StringLength(256, ErrorMessage = "Code cannot exceed more than 256 characters.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Is active is a required field.")]
        public bool IsActive { get; set; }

        public virtual ICollection<Equipment> Equipment { get; set; }

        public virtual ICollection<EquipmentTypeEAV> EquipmentTypeEAVs { get; set; }

    }
}

