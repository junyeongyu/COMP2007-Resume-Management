namespace ResumeManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Package : BaseModel
    {
        [Key]
        [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
        public string PackageId { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Package Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "Resume")]
        public string ResumeId { get; set; }

        [ForeignKey("ResumeId")]
        public virtual Resume Resume { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "CVLetter")]
        public string CVLetterId { get; set; }

        [ForeignKey("CVLetterId")]
        public virtual CVLetter CVLetter { get; set; }
    }
}
