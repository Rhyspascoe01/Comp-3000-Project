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
        double StopChangeTwo;
        string none = "none";
        public HomeController(ILogger<HomeController> logger, RouteContexts context, StopContext contexttwo, BusContext contextthree, AccountContext contextfour)
        {
            _logger = logger;
            _context = context;
            _contexttwo = contexttwo;
            _contextthree = contextthree;
            _contextfour = contextfour;
        }


        public async Task<IActionResult> Index(int id)
        {

            if (id == 0 )
            {
                ViewData["Switch"] = 1;
                ViewData["Direction"] = "Southbound";
                return View(await _context.RouteController.Where(i => i.RouteDirection == "North").ToListAsync());
            }
            else
            {
                ViewData["Switch"] = 0;
                ViewData["Direction"] = "NorthBound";
                return View(await _context.RouteController.Where(i => i.RouteDirection == "South").ToListAsync());
            }
        }
       
        public async Task<IActionResult> UserIndex(int id)
        {

            if (id == 0)
            {
                ViewData["Switch"] = 1;
                ViewData["Direction"] = "Southbound";
                return View(await _context.RouteController.Where(i => i.RouteDirection == "North").ToListAsync());
            }
            else
            {
                ViewData["Switch"] = 0;
                ViewData["Direction"] = "NorthBound";
                return View(await _context.RouteController.Where(i => i.RouteDirection == "South").ToListAsync());
            }
        }
        public async Task<IActionResult> TimeTableDelete()
        {

            return View();
        }
        public async Task<IActionResult> BusIndex(int id)
        {
            if (id == 0)
            {
                ViewData["Switch"] = 1;
                ViewData["Direction"] = "Southbound";
                return View(await _context.RouteController.Where(i => i.RouteDirection == "North").ToListAsync());
            }
            else
            {
                ViewData["Switch"] = 0;
                ViewData["Direction"] = "NorthBound";
                return View(await _context.RouteController.Where(i => i.RouteDirection == "South").ToListAsync());
            }
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
                    Colour = TimeTable.Colour,

                };
                @ViewData["Number"] = TimeTable.RouteNumber;
                @ViewData["Name"] = TimeTable.RouteName;
                @ViewData["Colour"] = TimeTable.Colour;
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
                    RouteDescription = TimeTable.RouteDescription,
                       Colour = TimeTable.Colour,
                };
                ViewData["Name"] = TimeTable.RouteName;
                ViewData["Description"] = TimeTable.RouteDescription;
                @ViewData["Colour"] = TimeTable.Colour;
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
            var RouteFrame = await _context.RouteController.FindAsync(TimeFrames.RouteID);
            int IDStore = TimeFrames.RouteID;
            if (TimeFrames != null)
            {
                double StopChange = EditModel.StopTime - TimeFrames.StopTime;
                StopChange = Math.Round(StopChange, 2);
                TimeFrames.StopTime = Math.Round(EditModel.StopTime, 2);
                double Latetime = StopChange * 100;
                RouteFrame.LateTime = RouteFrame.LateTime + Math.Round(Latetime);
                while (TimeFrames != null)
                {
                    EditModel.ID += 1;
                    TimeFrames = await _contexttwo.Stops.FindAsync(EditModel.ID);
                    if (TimeFrames != null)
                    {
                        int IDstore2 = TimeFrames.RouteID;
                        if (IDStore == IDstore2)
                        {
                            TimeFrames.StopTime += StopChange;
                            TimeFrames.StopTime = Math.Round(TimeFrames.StopTime, 2);
                            for (int i = 0; i < 22; i++)
                            {
                                double itwo = i + 0.59;
                               itwo=  Math.Round(itwo, 2);
                                if (TimeFrames.StopTime > itwo && TimeFrames.StopTime < i + 1)
                                {

                                    if (StopChange > 0)
                                    {
                                        StopChangeTwo = 0.60 - StopChange;
                                        TimeFrames.StopTime += 1;
                                    }
                                    else
                                    {
                                        StopChangeTwo = 0.60 + StopChange;
                                    }
                                    double StopChangeThree = 0.60 - StopChangeTwo;
                                    TimeFrames.StopTime -= Math.Round(StopChangeThree);
                                }
                            }
                        }
                    }
                    TimeFrames = await _contexttwo.Stops.FindAsync(EditModel.ID);
                }

                await _contexttwo.SaveChangesAsync();
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> TimeFrameDelete(StopController DeleteModel)
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
        public async Task<IActionResult> TimeTableDelete(RouteController DeleteModel)
        {
            var Routes = await _context.RouteController.FindAsync(DeleteModel.RouteID);
            if (Routes != null)
            {
                _context.RouteController.Remove(Routes);
                await _context.SaveChangesAsync();
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
                StopName = NewStop.StopName,
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
        public async Task<IActionResult> BusDriverEditor(int ID)
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
            if (Buses != null)
            {
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
                RouteID = NewBus.RouteID,
                BusDriverName = NewBus.BusDriverName,
                BusDriverDOB = NewBus.BusDriverDOB,
                BusDriverStartShift = NewBus.BusDriverStartShift,
                BusDriverEndShift = NewBus.BusDriverEndShift,
               IncidentMessage = none
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
                RouteDirection = NewRoute.RouteDirection,
                Colour = NewRoute.Colour
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
            if (account != null)
            {
                if (account.Roles == "Admin")
                {
                    return RedirectToAction("Index");
                }
                else if (account.Roles == "Driver")
                {
                    TempData["DriverName"] = account.UserName;
                    return RedirectToAction("BusDetails");
                }
            }
            else
            {
                string errorstring = "ERROR, ACCOUNT NOT FOUND.PLEASE TRY AGAIN.";
                TempData["ErrorString"] = errorstring;
                return View();
            }
            return View();
        }
        public async Task<IActionResult> BusDetails()
        {
            var account = await _contextfour.AccountControllers.ToListAsync();

            return View(await _contextthree.BusController.Where(i => i.BusDriverName == TempData["Drivername"]).ToListAsync());
        }
        public IActionResult AbsenceReport()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AbsenceReport(StopController NewStop)
        {
            var TimeFrames = await _contexttwo.Stops.FindAsync(NewStop.ID);
            var RouteFrame = await _context.RouteController.FindAsync(TimeFrames.RouteID);
            int IDStore = TimeFrames.RouteID;
            if (TimeFrames != null)
            {

                TimeFrames.StopTime += 0.30;
                double Latetime = 0.30 * 100;
                Latetime = Math.Round(Latetime, 2);
                RouteFrame.LateTime = RouteFrame.LateTime + Latetime;
                while (TimeFrames != null)
                {
                    NewStop.ID += 1;
                    TimeFrames = await _contexttwo.Stops.FindAsync(NewStop.ID);
                    if (TimeFrames != null)
                    {
                        int IDstore2 = TimeFrames.RouteID;
                        if (IDStore == IDstore2)
                        {
                            TimeFrames.StopTime += 0.30;
                            for (int i = 0; i < 22; i++)
                            {
                                double itwo = i + 0.59;
                                if (TimeFrames.StopTime > itwo && TimeFrames.StopTime < i + 1)
                                {
                                    double StopChangeThree = 0.60 - 0.30;
                                    TimeFrames.StopTime -= StopChangeThree;
                                }
                            }
                        }
                    }

                }

                await _contexttwo.SaveChangesAsync();
                await _context.SaveChangesAsync();
                return RedirectToAction("BusIndex");
            }
            return RedirectToAction("BusIndex");
        }
        public IActionResult IncidentReporter()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> IncidentReporter(StopController IncidentStop)
        {
            var TimeFrames = await _contexttwo.Stops.FindAsync(IncidentStop.ID);
            var RouteFrame = await _context.RouteController.FindAsync(TimeFrames.RouteID);
            int IDStore = TimeFrames.RouteID;
            double TimeSave = TimeFrames.StopTime;
            if (IncidentStop.RouteID == 1)
            {
                TimeFrames.StopTime += 0.10;
            }
            else if (IncidentStop.RouteID == 2)
            {
                TimeFrames.StopTime += 0.30;
            }
            else if (IncidentStop.RouteID == 3)
            {
                TimeFrames.StopTime += 0.25;
            }
            else if (IncidentStop.RouteID == 4)
            {
                TimeFrames.StopTime += 0.40;
            }
            if (TimeFrames != null)
            {
                TimeFrames.StopTime = Math.Round(TimeFrames.StopTime, 2);
                double StopChange = TimeFrames.StopTime - TimeSave;
                double Latetime = StopChange * 100;
                RouteFrame.LateTime = RouteFrame.LateTime + Latetime;
                while (TimeFrames != null)
                {
                    IncidentStop.ID += 1;
                    TimeFrames = await _contexttwo.Stops.FindAsync(IncidentStop.ID);
                    if (TimeFrames != null)
                    {
                        int IDstore2 = TimeFrames.RouteID;
                        if (IDStore == IDstore2)
                        {
                            TimeFrames.StopTime += StopChange;
                            for (int i = 0; i < 22; i++)
                            {
                                double itwo = i + 0.59;
                                if (TimeFrames.StopTime > itwo && TimeFrames.StopTime < i + 1)
                                {

                                    if (StopChange > 0)
                                    {
                                        StopChangeTwo = 0.60 - StopChange;
                                        TimeFrames.StopTime += 1;
                                    }
                                    else
                                    {
                                        StopChangeTwo = 0.60 + StopChange;
                                    }
                                    double StopChangeThree = 0.60 - StopChangeTwo;
                                    TimeFrames.StopTime -= StopChangeThree;
                                }
                            }
                        }

                    }
                }
            }
            await _contexttwo.SaveChangesAsync();
            await _context.SaveChangesAsync();
            return RedirectToAction("BusIndex");
        }
        public IActionResult Alerts()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Alerts(BusController MessageBus)
        {
            var Buses = await _contextthree.BusController.FindAsync(MessageBus.BusDriverID);
            if (Buses != null)
            {
                Buses.IncidentMessage = MessageBus.IncidentMessage;
            }
            await _contextthree.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> BusDetails(BusController MessageBus)
        {
            
            return RedirectToAction("BusIndex");
        }
        public IActionResult Search()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Search(StopController stops)
        {   
            int i = 0;
            var TimeFrames = await _contexttwo.Stops.ToListAsync();
            while (TimeFrames != null)
            {
                if (TimeFrames[i].StopName == stops.StopName)
                {
                    TempData["RouteDescription"] = TimeFrames[i].RouteDescription.ToString();
                    return RedirectToAction("SearchIndex");

                }
            }
            return RedirectToAction("SearchIndex");
        }
        public async Task<IActionResult> SearchIndex()
        {
            string RouteID = TempData["RouteDescription"].ToString();
            return View(await _context.RouteController.Where(i => i.RouteDescription == RouteID).ToListAsync());
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