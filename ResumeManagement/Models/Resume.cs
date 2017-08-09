namespace ResumeManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class Resume : BaseModel
    {
        public Resume()
        {

        }

        [Key]
        [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
        public string ResumeId { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Resume Name")]
        public string Name { get; set; }

        // ApplicantName, Email, Phone, Address, Summary, Skill, WorkExperience, StudyExperience, Certificate

        [Required]
        [StringLength(50)]
        [Display(Name = "Applicant Name")]
        public string ApplicantName { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [StringLength(3000)]
        [Display(Name = "Summary")]
        public string Summary { get; set; }

        [Required]
        [StringLength(3000)]
        [Display(Name = "Skill")]
        public string Skill { get; set; }

        [Required]
        [StringLength(5000)]
        [Display(Name = "WorkExperience")]
        public string WorkExperience { get; set; }

        [Required]
        [StringLength(5000)]
        [Display(Name = "StudyExperience")]
        public string StudyExperience { get; set; }

        [Required]
        [StringLength(800)]
        [Display(Name = "Certificate")]
        public string Certificate { get; set; }

        [Display(Name = "CVLetter")]
        [InverseProperty("Resume")]
        public virtual ICollection<Package> CVLetters { get; set; } = new HashSet<Package>();
    }
}
