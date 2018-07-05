﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Models.TaskViewModels
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
        }
        public List<Pot> List { get; set; }

    }

    public class Pot{
        [Display(Name = "Deadline")]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Deadline { get; set; }
        public int TaskId { get; set; }
        public string Description { get; set; }
        public string ProtocolDescription { get; set; }
        public bool Status { get; set; }
    }
}
