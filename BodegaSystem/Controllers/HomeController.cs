using System.Diagnostics;
using BodegaSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BodegaSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BodegaDBContext _context; 

        public HomeController(ILogger<HomeController> logger, BodegaDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // Contar registros en cada tabla
            ViewBag.TotalProductos = _context.Productos.Count();
            ViewBag.TotalCategorias = _context.Categorias.Count();
            ViewBag.TotalProveedores = _context.Proveedores.Count();
            ViewBag.TotalMovimientos = _context.Movimientos.Count();

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
