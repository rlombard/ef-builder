using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("MaterialID = {MaterialID}")]
    [Table("Material", Schema = "com")]
    public partial class Material
    {
        public Material()
        {
            MaterialEAVs = new HashSet<MaterialEAV>();
        }
        [Key]
        public int MaterialID { get; set; }

        [Required(ErrorMessage = "Name is a required field.")]
        [StringLength(256, ErrorMessage = "Name cannot exceed more than 256 characters.")]
        public string Name { get; set; }

        [StringLength(512, ErrorMessage = "Description cannot exceed more than 512 characters.")]
        public string Description { get; set; }

        [StringLength(512, ErrorMessage = "Code cannot exceed more than 512 characters.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Start date is a required field.")]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Site ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Site ID must be greater than 0.")]
        public int SiteID { get; set; }

        [ForeignKey(nameof(SiteID))]
        public virtual Site Site { get; set; }

        [Required(ErrorMessage = "Material type ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Material type ID must be greater than 0.")]
        public int MaterialTypeID { get; set; }

        [ForeignKey(nameof(MaterialTypeID))]
        public virtual MaterialType MaterialType { get; set; }

        [StringLength(256, ErrorMessage = "Unit of measure cannot exceed more than 256 characters.")]
        public string UnitOfMeasure { get; set; }

        [Required(ErrorMessage = "Quantity is a required field.")]
        public double Quantity { get; set; }

        [Required(ErrorMessage = "Is active is a required field.")]
        public bool IsActive { get; set; }

        public virtual ICollection<MaterialEAV> MaterialEAVs { get; set; }

    }
}

