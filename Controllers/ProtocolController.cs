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
using WebApplication5.Models.ProtocolViewModels;


namespace WebApplication5.Controllers
{
    public class ProtocolController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        public ProtocolController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Protocol
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var id = user.AppID; // Get user id:

            if (User.IsInRole("Admin")){
                var users = _context.Protocols
                                .Include(o => o.Organization)
                                .ToList();
                var protocol = _context.Protocols.Include(p => p.Organization);
                List<Protocol> protocols = await protocol.ToListAsync();
                IndexViewModel model = new IndexViewModel();
                model.list = new List<Prot>();
                foreach (Protocol p in users)
                {

                    model.list.Add(new Prot
                    {
                        ProtocolId = p.Id,
                        Description = p.Description,
                        OrgName = p.Organization.OrgName
                    });
                }
                Console.WriteLine("&&&&&& USERID = {0}******", User.IsInRole("Admin"));
                return View(model);
            } else {
                var users1 = _context.ApplicationUser_Has_Protocols
                                .Include(a => a.ApplicationUser)
                                .Include(b => b.Protocol).ThenInclude(o => o.Organization)
                                .Where(m => m.AppID == id).ToList();
                var users2 = _context.ApplicationUser_Has_Tasks
                                     .Include(a => a.ApplicationUser)
                                     .Include(b => b.Task).ThenInclude(o => o.Protocol).ThenInclude(d => d.Organization)
                                     .Where(m => m.AppID == id ).ToList();
                var protocol = _context.Protocols.Include(p => p.Organization);
                List<Protocol> protocols = await protocol.ToListAsync();
                IndexViewModel model = new IndexViewModel();
                model.list = new List<Prot>();
                foreach (ApplicationUser_has_Protocol p in users1)
                {

                    model.list.Add(new Prot
                    {
                        ProtocolId = p.ProtocolID,
                        Description = p.Protocol.Description,
                        OrgName = p.Protocol.Organization.OrgName
                    });
                }
                foreach (ApplicationUser_has_Task p in users2)
                {
                    int index = model.list.FindIndex(f => f.ProtocolId == p.Task.ProtocolID);
                    if (index < 0 ){
                    model.list.Add(new Prot
                    {
                            ProtocolId = p.Task.ProtocolID,
                            Description = p.Task.Protocol.Description,
                            OrgName = p.Task.Protocol.Organization.OrgName
                    });
                    }
                }

                Console.WriteLine("&&&&&& USERID = {0}******", User.IsInRole("Admin"));
                return View(model);
            }



        }

        // GET: Protocol/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var protocol = await _context.Protocols
                .Include(p => p.Organization)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (protocol == null)
            {
                return NotFound();
            }

            return View(protocol);
        }

        // GET: Protocol/Create
        public IActionResult Create()
        {
            CreateViewModel model = new CreateViewModel();
            model.Organizations = _context.Organizations
                .Select(h => new Organization
                {
                    Id = h.Id,
                    OrgName = h.OrgName,
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

        // POST: Protocol/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel model)
        {

            //var userID = User.Identity.GetUserId();
            string query = "SELECT * FROM Organization WHERE Id = {0}";
            Organization org = await _context.Organizations.FromSql(query, model.OrganizationID)
                .AsNoTracking()
                .SingleOrDefaultAsync();
            Console.WriteLine("&&&&&& ORGNAME = {0}******", org.OrgName);
            Protocol protocol = new Protocol
            {
                OrganizationID = model.OrganizationID,
                Description = model.Description
            };
            if (ModelState.IsValid)
            {
                _context.Add(protocol);
                await _context.SaveChangesAsync();

                for (int i = 0; i < model.Filters.Count(); i++)
                {
                    if (model.Filters[i].Selected)
                    {
                        ApplicationUser_has_Protocol has_Protocol = new ApplicationUser_has_Protocol
                        {
                            AppID = model.Filters[i].ID,
                            Protocol = protocol,
                            ProtocolID = protocol.Id
                        };
                        Console.WriteLine("&&&&&& Filters[i] = {0}******", model.Filters[i]);

                        _context.Add(has_Protocol);
                        protocol.ApplicationUser_has_Protocols.Add(has_Protocol);
                        _context.Update(protocol);
                        var user = await _context.ApplicationUsers
                                                 .SingleOrDefaultAsync(m => m.AppID == model.Filters[i].ID);
                        user.ApplicationUser_has_Protocols.Add(has_Protocol);
                        _context.Update(user);
                        await _context.SaveChangesAsync();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ////Redirect to "Create" I guess
            CreateViewModel md = new CreateViewModel();
            md.Organizations = _context.Organizations
                .Select(h => new Organization
                {
                    Id = h.Id,
                    OrgName = h.OrgName,
                })
                .ToList();
            foreach (Organization asd in md.Organizations)
            {
                Console.WriteLine("&&&&&&{0}******", asd.OrgName);
            }
            md.ApplicationUsers = _context.ApplicationUsers
                .Select(h => new ApplicationUser
                {
                    AppID = h.AppID,
                    FirstName = h.FirstName,
                    Email = h.Email
                })
                .ToList();
            foreach (ApplicationUser asd in md.ApplicationUsers)
            {
                Console.WriteLine("*****{0}******", asd.Email);
            }
            return View(md);
        }

        // GET: Protocol/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var protocol = await _context.Protocols.SingleOrDefaultAsync(m => m.Id == id);
            if (protocol == null)
            {
                return NotFound();
            }
            ViewData["OrganizationID"] = new SelectList(_context.Organizations, "Id", "OrgName", protocol.OrganizationID);
            return View(protocol);
        }

        // POST: Protocol/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,OrganizationID")] Protocol protocol)
        {
            if (id != protocol.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(protocol);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProtocolExists(protocol.Id))
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
            ViewData["OrganizationID"] = new SelectList(_context.Organizations, "Id", "OrgName", protocol.OrganizationID);
            return View(protocol);
        }

        // GET: Protocol/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var protocol = await _context.Protocols
                .Include(p => p.Organization)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (protocol == null)
            {
                return NotFound();
            }

            return View(protocol);
        }

        // POST: Protocol/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var protocol = await _context.Protocols.SingleOrDefaultAsync(m => m.Id == id);
            _context.Protocols.Remove(protocol);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProtocolExists(int id)
        {
            return _context.Protocols.Any(e => e.Id == id);
        }
    }
}
