using System;
using System.Collections.Generic;

namespace WebApplication5.Models.ProtocolViewModels
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
        }
        public List<Prot> list { get; set; }

    }

    public class Prot{
        public int ProtocolId { get; set; }
        public string Description { get; set; }
        public string OrgName { get; set; }
    }
}
