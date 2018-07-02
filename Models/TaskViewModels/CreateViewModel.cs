using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Models.TaskViewModels
{
    public class CreateViewModel
    {
        public CreateViewModel()
        {
        }
        [Required]
        public int ProtocolID { get; set; }
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
        [Display(Name = "Deadline")]
        [Required(ErrorMessage = "Please enter a deadline")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Deadline { get; set; }
        public List<ApplicationUser> ApplicationUsers { get; set; }
        public List<Protocol> Protocols { get; set; }

        public Filter[] Filters { get; set; }

    }
    public class Filter
    {
        public Filter()
        {

        }
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }
}
