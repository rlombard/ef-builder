using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("StartDate = {StartDate}")]
    [Table("StageOrgTree", Schema = "com")]
    public partial class StageOrgTree
    {
        public StageOrgTree()
        {
        }
        [Required(ErrorMessage = "External position ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for External position ID must be greater than 0.")]
        public int ExternalPositionID { get; set; }

        [Key]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is a required field.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Superior position ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Superior position ID must be greater than 0.")]
        public int SuperiorPositionID { get; set; }

        [Required(ErrorMessage = "Rank is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Rank must be greater than 0.")]
        public int Rank { get; set; }

        [Required(ErrorMessage = "Name is a required field.")]
        [StringLength(512, ErrorMessage = "Name cannot exceed more than 512 characters.")]
        public string Name { get; set; }

    }
}

