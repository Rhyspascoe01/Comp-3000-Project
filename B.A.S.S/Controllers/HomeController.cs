using B.A.S.S.Models;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace B.A.S.S.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RouteContexts _context;
        private readonly StopContext _contexttwo;
        private readonly BusContext _contextthree;
        private readonly AccountContext _contextfour;
        public HomeController(ILogger<HomeController> logger, RouteContexts context, StopContext contexttwo, BusContext contextthree, AccountContext contextfour)
        {
            _logger = logger;
            _context = context;
            _contexttwo = contexttwo;
            _contextthree = contextthree;
            _contextfour = contextfour;
        }


        public async Task<IActionResult> Index()
        {
            
            return View(await _context.RouteController.ToListAsync());
        }
        public async Task<IActionResult> UserIndex()
        {

            return View(await _context.RouteController.ToListAsync());
        }
        public async Task<IActionResult> BusIndex()
        {

            return View(await _context.RouteController.ToListAsync());
        }
        public async Task<IActionResult> TimeTableViewer(int ID)
        {
            var TimeTable = await _context.RouteController.Where(i => i.RouteID == ID).FirstOrDefaultAsync();
            if (TimeTable != null)
            {
                var Model = new RouteController()
                {
                    RouteName = TimeTable.RouteName,
                    RouteNumber = TimeTable.RouteNumber,
                    
            };
                @ViewData["Number"] = TimeTable.RouteNumber;
                @ViewData["Name"] = TimeTable.RouteName;
            }
            
            return View(await _contexttwo.Stops.Where(i => i.RouteID == ID).ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> TimeTableEditor(int id)
        {
            var TimeTable = await _context.RouteController.Where(i => i.RouteID == id).FirstOrDefaultAsync();
            if (TimeTable != null)
            {
                var Model = new RouteController()
                {
                    RouteName = TimeTable.RouteName,
                    RouteDescription = TimeTable.RouteDescription
                };
                ViewData["Name"] = TimeTable.RouteName;
                ViewData["Description"] = TimeTable.RouteDescription;
            }
               
                return View(await _contexttwo.Stops.Where(i => i.RouteID == id).ToListAsync()); 
        }
        [HttpGet]
        public async Task<IActionResult> TimeFrameEdit(int ID)
        {
            var TimeFrame = await _contexttwo.Stops.Where(i => i.ID == ID).FirstOrDefaultAsync();
            if (TimeFrame != null)
            {
                var EditModel = new StopController()
                {
                    ID = TimeFrame.ID,
                    RouteID = TimeFrame.RouteID,
                    StopName = TimeFrame.StopName,
                    StopTime = TimeFrame.StopTime,
                    RouteName = TimeFrame.RouteName,
                    RouteDescription = TimeFrame.RouteDescription
                };
                ViewData["Time"] = TimeFrame.StopTime;
                ViewData["Name"] = TimeFrame.StopName;
                ViewData["RouteName"] = TimeFrame.RouteName;
                return View(EditModel);
            }
            return RedirectToAction("TimeFrameEdit");
        }
        [HttpGet]
        public async Task<IActionResult> TimeFrameDelete(int ID)
        {
            var TimeFrame = await _contexttwo.Stops.Where(i => i.ID == ID).FirstOrDefaultAsync();
            if (TimeFrame != null)
            {
                var DeleteModel = new StopController()
                {
                    ID = TimeFrame.ID,
                    RouteID = TimeFrame.RouteID,
                    StopName = TimeFrame.StopName,
                    StopTime = TimeFrame.StopTime,
                    RouteDescription = TimeFrame.RouteDescription
                };
                return View(DeleteModel);
            }
            return RedirectToAction("TimeFrameEdit");
        }
        [HttpPost]
        public async Task<IActionResult> TimeFrameEdit(StopController EditModel)
        {
            var TimeFrames = await _contexttwo.Stops.FindAsync(EditModel.ID);
            if (TimeFrames != null)
            {
               double StopChange = EditModel.StopTime - TimeFrames.StopTime;
                TimeFrames.StopTime = EditModel.StopTime;
                EditModel.ID += 1;
                while(TimeFrames != null)
                { TimeFrames = await _contexttwo.Stops.FindAsync(EditModel.ID);
                    if (TimeFrames != null)
                    {
                        TimeFrames.StopTime += StopChange;
                        EditModel.ID += 1;
                    }
                    
                }
               await _contexttwo.SaveChangesAsync();
               return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> TimeFrameDelete (StopController DeleteModel)
        {
            var Stops = await _contexttwo.Stops.FindAsync(DeleteModel.ID);
            if (Stops != null)
            {
                _contexttwo.Stops.Remove(Stops);
                await _contexttwo.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> BusDriverDelete(BusController DeleteModel)
        {
            var Driver = await _contextthree.BusController.FindAsync(DeleteModel.BusDriverID);
            if (Driver != null)
            {
                _contextthree.BusController.Remove(Driver);
                await _contextthree.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        public IActionResult StopAdder()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> StopAdder(StopController NewStop)
        {
           var Stops = new StopController()
           {
             ID = NewStop.ID,
             RouteID = NewStop.RouteID,
             StopName  = NewStop.StopName,
             StopTime = NewStop.StopTime,
             RouteName = NewStop.RouteName,
             RouteDescription = "Test"
           };

          await _contexttwo.Stops.AddAsync(Stops);
          await _contexttwo.SaveChangesAsync();
          return RedirectToAction("Index"); 
        }
       
        public async Task<IActionResult> BusDriver()
        {
            return View(await _contextthree.BusController.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult>  BusDriverEditor(int ID)
        {
            var Buses = await _contextthree.BusController.Where(i => i.BusDriverID == ID).FirstOrDefaultAsync();
            if (Buses != null)
            {
                var BusModel = new BusController()
                {
                    BusDriverID = Buses.BusDriverID,
                    BusDriverDOB = Buses.BusDriverDOB,
                    BusDriverStartShift = Buses.BusDriverStartShift,
                    BusDriverEndShift = Buses.BusDriverEndShift,
                };
                return View(BusModel);
            }
            return View("BusDriverEditor");
        }
        [HttpPost]
        public async Task<IActionResult> BusDriverEditor(BusController BusModel)
        {
            var Buses = await _contextthree.BusController.FindAsync(BusModel.BusDriverID);
           if(Buses != null) {
                Buses.BusDriverID = BusModel.BusDriverID;
                Buses.BusDriverDOB = BusModel.BusDriverDOB;
                Buses.BusDriverStartShift = BusModel.BusDriverStartShift;
                Buses.BusDriverEndShift = BusModel.BusDriverEndShift;
                await _contextthree.SaveChangesAsync();
                return RedirectToAction("Index");
            };
            return RedirectToAction("Index");
        }
        public IActionResult AddDriver()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddDriver(BusController NewBus)
        {
            var Buses = new BusController()
            {
                BusDriverID = NewBus.BusDriverID,
                RouteID= NewBus.RouteID,
                BusDriverName = NewBus.BusDriverName,
                BusDriverDOB = NewBus.BusDriverDOB,
                BusDriverStartShift = NewBus.BusDriverStartShift,
                BusDriverEndShift = NewBus.BusDriverEndShift,
            };
            await _contextthree.AddAsync(Buses);
            await _contextthree.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult AddRoute()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddRoute(RouteController NewRoute)
        {
            var Routes = new RouteController()
            {
                RouteID = NewRoute.RouteID,
                RouteName = NewRoute.RouteName,
                RouteNumber = NewRoute.RouteNumber,
                RouteDescription = NewRoute.RouteDescription,
            };

            await _context.AddAsync(Routes);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult LogInPage()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogInPage(AccountController Accounts)
        {
            var account = await _contextfour.AccountControllers.Where(i => i.UserName == Accounts.UserName && i.Passwords == Accounts.Passwords).FirstOrDefaultAsync();
            if(account.Roles == "Admin") {
                return RedirectToAction("Index");
            }
            if (account.Roles == "Driver")
            {
                ViewData["Name"] = account.UserName;
                return RedirectToAction("BusIndex");
            }
         
            return View();
        }
        public async Task<IActionResult> BusDetails()
        {   

            return View(await _contextthree.BusController.ToListAsync());
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}