using B.A.S.S.Models;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Mail;
using System.Runtime.InteropServices;



namespace B.A.S.S.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RouteContexts _context;
        private readonly StopContext _contexttwo;
        private readonly BusContext _contextthree;
        public HomeController(ILogger<HomeController> logger, RouteContexts context, StopContext contexttwo, BusContext contextthree)
        {
            _logger = logger;
            _context = context;
            _contexttwo = contexttwo;
            _contextthree = contextthree;
        }


        public async Task<IActionResult> Index()
        {
            
            return View(await _context.RouteController.ToListAsync());
        }

        public async Task<IActionResult> TimeTableViewer(string RouteID)
        {
            
            return View(await _contexttwo.Stops.ToListAsync());
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
            }
                ViewData["Name"] = TimeTable.RouteName;
                ViewData["Description"] = TimeTable.RouteDescription;
                return View(await _contexttwo.Stops.ToListAsync()); 
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
                    RouteDescription = TimeFrame.RouteDescription
                };
                ViewData["Time"] = TimeFrame.StopTime;
                ViewData["Name"] = TimeFrame.StopName;
                ViewData["ID"] = TimeFrame.ID;
                return View(EditModel);
            }
            return RedirectToAction("TimeFrameEdit");
        }
         [HttpPost]
        public async Task<IActionResult> TimeFrameEdit(StopController EditModel)
        {
            var TimeFrames = await _contexttwo.Stops.FindAsync(EditModel.ID);
            if (TimeFrames != null)
            { TimeFrames.StopTime = EditModel.StopTime; 
               await _contexttwo.SaveChangesAsync();
               return RedirectToAction("BusDriver");
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
             RouteName = "12",
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
        public IActionResult BusDriverEditor()
        {
            return View();
        }
        public IActionResult AddDriver()
        {
            return View();
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