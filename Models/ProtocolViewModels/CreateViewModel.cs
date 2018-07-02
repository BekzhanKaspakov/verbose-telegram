
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Models.ProtocolViewModels
{
    public class CreateViewModel
    {
        public CreateViewModel()
        {
        }
        [Required]
        public int OrganizationID { get; set; }
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
        public List<ApplicationUser> ApplicationUsers { get; set; }
        public List<Organization> Organizations { get; set; }

        public Filter[] Filters { get; set; }

    }
    public class Filter{
        public Filter()
        {

        }
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }


}
