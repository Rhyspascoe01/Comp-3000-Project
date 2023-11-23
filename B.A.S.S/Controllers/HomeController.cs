using B.A.S.S.Models;
using Microsoft.AspNetCore.Mvc;
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
            
            return View(await _contexttwo.StopController.ToListAsync());
        }
        public async Task<IActionResult> TimeTableEditor()
        {   
            return View(await _contexttwo.StopController.ToListAsync());
        }
        public IActionResult TimeFrameEdit()
        {
            return View();
        }
        public IActionResult StopAdder()
        {
            return View();
        }
        public IActionResult AddRoute()
        {
            return View();
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