using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication5.Data;
using WebApplication5.Models;
using WebApplication5.Models.TaskViewModels;

namespace WebApplication5.Controllers
{
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public TaskController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: Task
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            var idd = user.AppID; // Get user id:

            
            if (User.IsInRole("Admin"))
            {
                var users = _context.Tasks
                                 .Include(o => o.Protocol)
                                 .ToList();
                var task = _context.Tasks.Include(p => p.Protocol);
                List<Models.Task> tasks = await task.ToListAsync();
                IndexViewModel model = new IndexViewModel();
                model.List = new List<Pot>();
                foreach (Models.Task p in users)
                {

                    model.List.Add(new Pot
                    {
                        Deadline = p.Deadline,
                        TaskId = p.Id,
                        Description = p.Description,
                        ProtocolDescription = p.Protocol.Description,
                        Status = p.Status
                                               
                    });
                }
                return View(model);
            } else {
                var users = _context.ApplicationUser_Has_Tasks
                                .Include(a => a.ApplicationUser)
                                .Include(b => b.Task).ThenInclude(o => o.Protocol)
                                .Where(m => m.Task.ProtocolID == id && m.AppID == idd).ToList();
                var task = _context.Tasks.Include(p => p.Protocol);
                List<Models.Task> tasks = await task.ToListAsync();
                IndexViewModel model = new IndexViewModel();
                model.List = new List<Pot>();
                foreach (ApplicationUser_has_Task p in users)
                {

                    model.List.Add(new Pot
                    {
                        Deadline = p.Task.Deadline,
                        TaskId = p.TaskID,
                        Description = p.Task.Description,
                        ProtocolDescription = p.Task.Protocol.Description,
                        Status = p.Task.Status
                    });
                }
                return View(model);
            }


        }
        // GET: Task/Details/5
        public async Task<IActionResult> Complete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(t => t.Protocol)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }
            task.Status = true;
            _context.Update(task);
            await _context.SaveChangesAsync();
            Console.WriteLine("~~~~~~~~~~ {0} ************", task.Status);


            return RedirectToAction("Index", "Task", new { id = task.ProtocolID });
        }
        // GET: Task/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(t => t.Protocol)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: Task/Create
        public IActionResult Create()
        {
            CreateViewModel model = new CreateViewModel();
            model.Protocols = _context.Protocols
                .Select(h => new Protocol
                {
                    Id = h.Id,
                    Description = h.Description,
                })
                .ToList();

            model.ApplicationUsers = _context.ApplicationUsers
                .Select(h => new ApplicationUser
                {
                    AppID = h.AppID,
                    FirstName = h.FirstName,
                    Email = h.Email
                })
                .ToList();
            List<Filter> asd = _context.ApplicationUsers
                .Select(h => new Filter
                {
                    ID = h.AppID,
                    Name = h.Email,
                    Selected = false
                }).ToList();

            model.Filters = asd.ToArray();

            
            return View(model);
        }

        // POST: Task/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            //var userID = User.Identity.GetUserId();
            string query = "SELECT * FROM Protocol WHERE Id = {0}";
            Protocol pro = await _context.Protocols.FromSql(query, model.ProtocolID)
                .AsNoTracking()
                .SingleOrDefaultAsync();
            Console.WriteLine("&&&&&& ProtName = {0}******", pro.Description);
            Models.Task task = new Models.Task
            {
                ProtocolID = model.ProtocolID,
                Description = model.Description,
                Deadline = model.Deadline
            };
            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();

                for (int i = 0; i < model.Filters.Count(); i++)
                {
                    if (model.Filters[i].Selected)
                    {
                        ApplicationUser_has_Task has_Task = new ApplicationUser_has_Task
                        {
                            AppID = model.Filters[i].ID,
                            Task = task,
                            TaskID = task.Id
                        };
                        Console.WriteLine("&&&&&& Filters[i] = {0}******", model.Filters[i]);

                        _context.Add(has_Task);
                        task.ApplicationUser_has_Tasks.Add(has_Task);
                        _context.Update(task);
                        var user = await _context.ApplicationUsers
                                                 .SingleOrDefaultAsync(m => m.AppID == model.Filters[i].ID);
                        user.ApplicationUser_has_Tasks.Add(has_Task);
                        _context.Update(user);
                        await _context.SaveChangesAsync();
                    }
                }
                return RedirectToAction("Index", "Task", new { id = model.ProtocolID });
            }
            ////Redirect to "Create" I guess
            CreateViewModel mod = new CreateViewModel();
            mod.Protocols = _context.Protocols
                .Select(h => new Protocol
                {
                    Id = h.Id,
                    Description = h.Description,
                })
                .ToList();

            mod.ApplicationUsers = _context.ApplicationUsers
                .Select(h => new ApplicationUser
                {
                    AppID = h.AppID,
                    FirstName = h.FirstName,
                    Email = h.Email
                })
                .ToList();
            List<Filter> asd = _context.ApplicationUsers
                .Select(h => new Filter
                {
                    ID = h.AppID,
                    Name = h.Email,
                    Selected = false
                }).ToList();

            mod.Filters = asd.ToArray();
            return View(mod);
        }

        // GET: Task/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.SingleOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }
            ViewData["ProtocolID"] = new SelectList(_context.Protocols, "Id", "Description", task.ProtocolID);
            return View(task);
        }

        // POST: Task/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Deadline,Description,ProtocolID")] Models.Task task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProtocolID"] = new SelectList(_context.Protocols, "Id", "Description", task.ProtocolID);
            return View(task);
        }

        // GET: Task/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(t => t.Protocol)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Task/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.Tasks.SingleOrDefaultAsync(m => m.Id == id);
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
