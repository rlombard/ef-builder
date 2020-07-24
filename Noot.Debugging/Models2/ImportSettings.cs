using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [DebuggerDisplay("SettingID = {SettingID}")]
    [Table("ImportSettings", Schema = "com")]
    public partial class ImportSettings
    {
        public ImportSettings()
        {
        }
        [Key]
        public int SettingID { get; set; }

        [Required(ErrorMessage = "Setting name is a required field.")]
        [StringLength(64, ErrorMessage = "Setting name cannot exceed more than 64 characters.")]
        public string SettingName { get; set; }

        [Required(ErrorMessage = "Setting value is a required field.")]
        [StringLength(256, ErrorMessage = "Setting value cannot exceed more than 256 characters.")]
        public string SettingValue { get; set; }

    }
}

