using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("PersonID = {PersonID}")]
    [Table("Person", Schema = "com")]
    public partial class Person
    {
        public Person()
        {
            Appointments = new HashSet<Appointment>();
            Assignments = new HashSet<Assignment>();
            PersonEAVs = new HashSet<PersonEAV>();
            PersonIdentifiers = new HashSet<PersonIdentifier>();
        }
        [Key]
        public int PersonID { get; set; }

        [Required(ErrorMessage = "First name is a required field.")]
        [StringLength(256, ErrorMessage = "First name cannot exceed more than 256 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Surname is a required field.")]
        [StringLength(512, ErrorMessage = "Surname cannot exceed more than 512 characters.")]
        public string Surname { get; set; }

        [StringLength(128, ErrorMessage = "Title cannot exceed more than 128 characters.")]
        public string Title { get; set; }

        [StringLength(1024, ErrorMessage = "Other names cannot exceed more than 1024 characters.")]
        public string OtherNames { get; set; }

        [Required(ErrorMessage = "Initials is a required field.")]
        [StringLength(64, ErrorMessage = "Initials cannot exceed more than 64 characters.")]
        public string Initials { get; set; }

        [Required(ErrorMessage = "Is active is a required field.")]
        public bool IsActive { get; set; }

        public int? ExternalPersonID { get; set; }

        public byte[] DisplayImage { get; set; }

        [Required(ErrorMessage = "Site ID is a required field.")]
        [Range(1, 99999999, ErrorMessage = "Value for Site ID must be greater than 0.")]
        public int SiteID { get; set; }

        [ForeignKey(nameof(SiteID))]
        public virtual Site Site { get; set; }

        public int? SecUserID { get; set; }

        public int? MineFormsUserID { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }

        public virtual ICollection<Assignment> Assignments { get; set; }

        public virtual ICollection<PersonEAV> PersonEAVs { get; set; }

        public virtual ICollection<PersonIdentifier> PersonIdentifiers { get; set; }

    }
}

