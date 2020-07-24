using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("LocationGRID = {LocationGRID}")]
    [Table("LocationGraphicReference", Schema = "com")]
    public partial class LocationGraphicReference
    {
        public LocationGraphicReference()
        {
        }
        [Key]
        public int LocationGRID { get; set; }

        [Required(ErrorMessage = "Location ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Location ID must be greater than 0.")]
        public int LocationID { get; set; }

        [ForeignKey(nameof(LocationID))]
        public virtual Location Location { get; set; }

        [Required(ErrorMessage = "Library ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Library ID must be greater than 0.")]
        public int LibraryID { get; set; }

        [Required(ErrorMessage = "Element ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Element ID must be greater than 0.")]
        public int ElementID { get; set; }

        public double? Offset { get; set; }

        public double? Length { get; set; }

    }
}

