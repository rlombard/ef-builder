using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("CategoryID = {CategoryID}")]
    [Table("EventCategory", Schema = "com")]
    public partial class EventCategory
    {
        public EventCategory()
        {
            Events = new HashSet<Event>();
        }
        [Key]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Name is a required field.")]
        [StringLength(256, ErrorMessage = "Name cannot exceed more than 256 characters.")]
        public string Name { get; set; }

        [StringLength(512, ErrorMessage = "Description cannot exceed more than 512 characters.")]
        public string Description { get; set; }

        [StringLength(256, ErrorMessage = "Colour cannot exceed more than 256 characters.")]
        public string Colour { get; set; }

        [Required(ErrorMessage = "Is work day is a required field.")]
        public bool IsWorkDay { get; set; }

        [Required(ErrorMessage = "Site ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Site ID must be greater than 0.")]
        public int SiteID { get; set; }

        [ForeignKey(nameof(SiteID))]
        public virtual Site Site { get; set; }

        public virtual ICollection<Event> Events { get; set; }

    }
}

