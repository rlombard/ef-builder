using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("AssignmentID = {AssignmentID}")]
    [Table("Assignment", Schema = "com")]
    public partial class Assignment
    {
        public Assignment()
        {
        }
        [Key]
        public int AssignmentID { get; set; }

        [Required(ErrorMessage = "Start date is a required field.")]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? PersonID { get; set; }

        [ForeignKey(nameof(PersonID))]
        public virtual Person Person { get; set; }

        public int? OrganizationPositionID { get; set; }

        [ForeignKey(nameof(OrganizationPositionID))]
        public virtual OrganizationPosition OrganizationPosition { get; set; }

    }
}

