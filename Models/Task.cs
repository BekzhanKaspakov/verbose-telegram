using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Models
{
    public class Task
    {
        public Task()
        {
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Deadline")]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Deadline { get; set; }
        [Required]
        [StringLength(50)]
        public string Description { get; set; }
        public bool Status { get; set; }
        public ICollection<ApplicationUser_has_Task> ApplicationUser_has_Tasks { get; set; }

        public Protocol Protocol { get; set; }
        public int ProtocolID { get; set; }

    }
}
