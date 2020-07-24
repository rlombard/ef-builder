using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("EventLogID = {EventLogID}")]
    [Table("EventLog", Schema = "com")]
    public partial class EventLog
    {
        public EventLog()
        {
        }
        [Key]
        public int EventLogID { get; set; }

        [Required(ErrorMessage = "Action is a required field.")]
        [StringLength(64, ErrorMessage = "Action cannot exceed more than 64 characters.")]
        public string Action { get; set; }

        [Required(ErrorMessage = "Entity is a required field.")]
        [StringLength(128, ErrorMessage = "Entity cannot exceed more than 128 characters.")]
        public string Entity { get; set; }

        [Required(ErrorMessage = "Key data is a required field.")]
        [StringLength(512, ErrorMessage = "Key data cannot exceed more than 512 characters.")]
        public string KeyData { get; set; }

        [StringLength(128, ErrorMessage = "Property cannot exceed more than 128 characters.")]
        public string Property { get; set; }

        [Required(ErrorMessage = "User name is a required field.")]
        [StringLength(128, ErrorMessage = "User name cannot exceed more than 128 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Machine name is a required field.")]
        [StringLength(256, ErrorMessage = "Machine name cannot exceed more than 256 characters.")]
        public string MachineName { get; set; }

        [Required(ErrorMessage = "Event date time is a required field.")]
        public DateTime EventDateTime { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public string ExceptionInfo { get; set; }

    }
}

