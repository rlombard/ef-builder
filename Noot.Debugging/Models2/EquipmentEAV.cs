using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("ValueID = {ValueID}")]
    [Table("EquipmentEAV", Schema = "com")]
    public partial class EquipmentEAV
    {
        public EquipmentEAV()
        {
        }
        [Key]
        public int ValueID { get; set; }

        [Required(ErrorMessage = "Element ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Element ID must be greater than 0.")]
        public int ElementID { get; set; }

        [ForeignKey(nameof(ElementID))]
        public virtual Equipment Element { get; set; }

        [Required(ErrorMessage = "Attribute ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Attribute ID must be greater than 0.")]
        public int AttributeID { get; set; }

        [Required(ErrorMessage = "Value no is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Value no must be greater than 0.")]
        public int ValueNo { get; set; }

        public int? IntegerValue { get; set; }

        public double? FloatValue { get; set; }

        [StringLength(512, ErrorMessage = "String value cannot exceed more than 512 characters.")]
        public string StringValue { get; set; }

        public DateTime? DateValue { get; set; }

    }
}

