namespace ResumeManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CVLetter : BaseModel
    {
        public CVLetter()
        {

        }

        [Key]
        [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
        public string CVLetterId { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "CVLetter Name")]
        public string Name { get; set; }
        
        [Required]
        [StringLength(5000)]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Display(Name = "Resumes")]
        [InverseProperty("CVLetter")]
        public virtual ICollection<Package> Resumes { get; set; } = new HashSet<Package>();

        public override string ToString()
        {
            return String.Format("{0}", Name);
        }
    }
}
