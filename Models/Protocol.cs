using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication5.Models
{
    public class Protocol
    {
        public Protocol()
        {
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        public List<ApplicationUser_has_Protocol> ApplicationUser_has_Protocols { get; set; }

        public int OrganizationID { get; set; }
        public Organization Organization { get; set; }

        public ICollection<Task> Tasks { get; set; }
    }
}
