using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReportCatalog.Models
{
    [Table("rpt_setup")]
    public partial class RptSetup
    {
        [Key]
        public Guid Id { get; set; }
        [DisplayName("Report Code")]
        [Required(ErrorMessage = "Report Setup's Code is required.")]
        [StringLength(60, ErrorMessage = "Report Setup's Code cannot have more than 60 characters.")]
        public string RptCode { get; set; }
        [DisplayName("Description")]
        [Required(ErrorMessage = "Report Setup's Description is required.")]
        [StringLength(100, ErrorMessage = "Report Setup's Description cannot have more than 100 characters.")]
        public string RptDesc { get; set; }
        [DisplayName("Position")]
        [StringLength(3, ErrorMessage = "Position cannot have more than 3 characters.")]
        [RegularExpression("^[0-9]*$")]
        public string Pos { get; set; }
        [DisplayName("File Name")]
        [Required(ErrorMessage = "File Name is required.")]
        [StringLength(50, ErrorMessage = "File Name cannot have more than 50 characters.")]
        public string Filename { get; set; }
        [DisplayName("Parameter")]
        [StringLength(40, ErrorMessage = "Parameter cannot have more than 40 characters.")]
        public string RptParam { get; set; }
        public int? Version { get; set; }
        public bool Disabled { get; set; } = false;
        public string Note { get; set; }
        public byte[] ReportData { get; set; }
    }
}
