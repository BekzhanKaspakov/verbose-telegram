using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication5.Models;

namespace WebApplication5.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.ApplicationUsers.Any())
            {
                return;   // DB has been seeded
            }
  

            var ApplicationUsers = new ApplicationUser[]
            {
                new ApplicationUser{Email = "bekzhan.kaspakov@mail.ru",FirstName="Carson",LastName="Alexander"},
                new ApplicationUser{Email = "bekzhan.kaspakov@gmail.com",FirstName="Meredith",LastName="Alonso"},
                new ApplicationUser{Email = "asd@mail.ru",FirstName="Arturo",LastName="Anand"},
                new ApplicationUser{Email = "asdf@mail.ru", FirstName="Gytis",LastName="Barzdukas"},
                new ApplicationUser{Email = "asdfg@mail.ru", FirstName="Yan",LastName="Li"},
                new ApplicationUser{Email = "bekzhan.kaspakov@mail.ru", FirstName="Peggy",LastName="Justice"},
                new ApplicationUser{FirstName="Laura",LastName="Norman"},
                new ApplicationUser{FirstName="Nino",LastName="Olivetto"}
            };
            foreach (ApplicationUser s in ApplicationUsers)
            {
                context.ApplicationUsers.Add(s);
            }
            context.SaveChanges();

            var protocols = new Protocol[]
            {
                new Protocol{Description="Chemistry"},
                new Protocol{Description="Microeconomics"},
                new Protocol{Description="Macroeconomics"},
                new Protocol{Description="Calculus"},
                new Protocol{Description="Trigonometry"},
                new Protocol{Description="Composition"},
                new Protocol{Description="Literature"}
            };
            foreach (Protocol c in protocols)
            {
                context.Protocols.Add(c);
            }
            context.SaveChanges();

            var Tasks = new Models.Task[]
            {
                new Models.Task{ProtocolID=1,Description="Contrary to popular belief, Lorem Ipsum is not simply random text",Deadline=DateTime.Parse("2002-09-01")},
                new Models.Task{ProtocolID=1,Description="It has roots in a piece of classical Latin literature from 45 BC",Deadline=DateTime.Parse("2002-09-01")},
                new Models.Task{ProtocolID=1,Description="making it over 2000 years old. Richard McClintock, a Latin ",Deadline=DateTime.Parse("2003-09-01")},
                new Models.Task{ProtocolID=2,Description="professor at Hampden-Sydney College in Virginia, looked up one of ",Deadline=DateTime.Parse("2002-09-01")},
                new Models.Task{ProtocolID=2,Description="the more obscure Latin words, consectetur, from a Lorem Ipsum ",Deadline=DateTime.Parse("2002-09-01")},
                new Models.Task{ProtocolID=2,Description="passage, and going through the cites of the word in classical ",Deadline=DateTime.Parse("2001-09-01")},
                new Models.Task{ProtocolID=3,Description="literature, discovered the undoubtable source. Lorem Ipsum ",Deadline=DateTime.Parse("2003-09-01")},
                new Models.Task{ProtocolID=4,Description="comes from sections 1.10.32 and 1.10.33 of de Finibus Bonorum",Deadline=DateTime.Parse("2005-09-01")},
            };
            foreach (Models.Task e in Tasks)
            {
                context.Tasks.Add(e);
            }
            context.SaveChanges();

            var ApplicationUser_has_Protocols = new ApplicationUser_has_Protocol[]
            {
                new ApplicationUser_has_Protocol {
                    AppID = ApplicationUsers.Single(s => s.LastName == "Alexander").AppID,
                    ProtocolID = 1
                },
                new ApplicationUser_has_Protocol {
                    AppID = ApplicationUsers.Single(s => s.LastName == "Alexander").AppID,
                    ProtocolID = 1
                    },
                new ApplicationUser_has_Protocol {
                    AppID = ApplicationUsers.Single(s => s.LastName == "Alexander").AppID,
                    ProtocolID = 1
                    },
                new ApplicationUser_has_Protocol {
                    AppID = ApplicationUsers.Single(s => s.LastName == "Alonso").AppID,
                    ProtocolID = 1
                    },
                new ApplicationUser_has_Protocol {
                    AppID = ApplicationUsers.Single(s => s.LastName == "Alonso").AppID,
                    ProtocolID = 1
                    },
                new ApplicationUser_has_Protocol {
                    AppID = ApplicationUsers.Single(s => s.LastName == "Alonso").AppID,
                    ProtocolID = 1
                    },
                new ApplicationUser_has_Protocol {
                    AppID = ApplicationUsers.Single(s => s.LastName == "Anand").AppID,
                    ProtocolID = 1
                    },
                new ApplicationUser_has_Protocol {
                    AppID = ApplicationUsers.Single(s => s.LastName == "Anand").AppID,
                    ProtocolID = 1
                    },
                new ApplicationUser_has_Protocol {
                    AppID = ApplicationUsers.Single(s => s.LastName == "Barzdukas").AppID,
                    ProtocolID = 1
                    },
                new ApplicationUser_has_Protocol {
                    AppID = ApplicationUsers.Single(s => s.LastName == "Li").AppID,
                    ProtocolID = 1
                    },
                new ApplicationUser_has_Protocol {
                    AppID = ApplicationUsers.Single(s => s.LastName == "Justice").AppID,
                    ProtocolID = 1
                    }
            };

            foreach (ApplicationUser_has_Protocol e in ApplicationUser_has_Protocols)
            {
                context.ApplicationUser_Has_Protocols.Add(e);
            }
            context.SaveChanges();
     

            var ApplicationUser_has_Tasks = new ApplicationUser_has_Task[]
            {
                new ApplicationUser_has_Task {
                    TaskID = 1,
                    AppID = ApplicationUsers.Single(i => i.LastName == "Kapoor").AppID
                    },
                new ApplicationUser_has_Task {
                    TaskID = 1,
                    AppID = ApplicationUsers.Single(i => i.LastName == "Harui").AppID
                    },
                new ApplicationUser_has_Task {
                    TaskID = 2,
                    AppID = ApplicationUsers.Single(i => i.LastName == "Zheng").AppID
                    },
                new ApplicationUser_has_Task {
                    TaskID = 2,
                    AppID = ApplicationUsers.Single(i => i.LastName == "Zheng").AppID
                    },
                new ApplicationUser_has_Task {
                    TaskID = 3,
                    AppID = ApplicationUsers.Single(i => i.LastName == "Fakhouri").AppID
                    },
                new ApplicationUser_has_Task {
                    TaskID = 3,
                    AppID = ApplicationUsers.Single(i => i.LastName == "Harui").AppID
                    },
                new ApplicationUser_has_Task {
                    TaskID = 4,
                    AppID = ApplicationUsers.Single(i => i.LastName == "Abercrombie").AppID
                    },
                new ApplicationUser_has_Task {
                    TaskID = 4,
                    AppID = ApplicationUsers.Single(i => i.LastName == "Abercrombie").AppID
                    },
           };

            foreach (ApplicationUser_has_Task ci in ApplicationUser_has_Tasks)
            {
                context.ApplicationUser_Has_Tasks.Add(ci);
            }
            context.SaveChanges();


        }
    }
}
