using System;
using System.Collections.Generic;

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
        public DateTime Deadline { get; set; }
        public int TaskId { get; set; }
        public string Description { get; set; }
        public string ProtocolDescription { get; set; }
    }
}
