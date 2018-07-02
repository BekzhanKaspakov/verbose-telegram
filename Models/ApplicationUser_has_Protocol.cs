using System;
namespace WebApplication5.Models
{
    public class ApplicationUser_has_Protocol
    {
        public ApplicationUser_has_Protocol()
        {
        }
        public int AppID { get; set; }
        public int ProtocolID { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public Protocol Protocol { get; set; }
    }
}
