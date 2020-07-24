using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("OccupationID = {OccupationID}")]
    [Table("Occupation", Schema = "com")]
    public partial class Occupation
    {
        public Occupation()
        {
            Appointments = new HashSet<Appointment>();
            OccupationEAVs = new HashSet<OccupationEAV>();
            PositionTypeOccupations = new HashSet<PositionTypeOccupation>();
        }
        [Key]
        public int OccupationID { get; set; }

        [Required(ErrorMessage = "Name is a required field.")]
        [StringLength(256, ErrorMessage = "Name cannot exceed more than 256 characters.")]
        public string Name { get; set; }

        [StringLength(512, ErrorMessage = "Description cannot exceed more than 512 characters.")]
        public string Description { get; set; }

        [StringLength(128, ErrorMessage = "Code cannot exceed more than 128 characters.")]
        public string Code { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }

        public virtual ICollection<OccupationEAV> OccupationEAVs { get; set; }

        public virtual ICollection<PositionTypeOccupation> PositionTypeOccupations { get; set; }

    }
}

