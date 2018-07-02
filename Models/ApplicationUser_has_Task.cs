using System;
namespace WebApplication5.Models
{
    public class ApplicationUser_has_Task
    {
        public ApplicationUser_has_Task()
        {
        }
        public int AppID { get; set; }
        public int TaskID { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Task Task { get; set; }
    }
}
