using Microsoft.AspNetCore.Mvc;
using TravelPlannerMVC.Data;
using TravelPlannerMVC.Models;

namespace TravelPlannerMVC.Controllers
{
    public class TravelRoutesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TravelRoutesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var routes = _context.TravelRoutes.ToList();

            return View(routes);
        }

        public IActionResult Details(int id)
        {
            var route = _context.TravelRoutes.Find(id);

            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TravelRoute route)
        {
            if (ModelState.IsValid)
            {
                _context.TravelRoutes.Add(route);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(route);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var route = _context.TravelRoutes.Find(id);

            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

        [HttpPost]
        public IActionResult Edit(TravelRoute route)
        {
            if (ModelState.IsValid)
            {
                _context.TravelRoutes.Update(route);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(route);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var route = _context.TravelRoutes.Find(id);

            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var route = _context.TravelRoutes.Find(id);

            if (route == null)
            {
                return NotFound();
            }

            _context.TravelRoutes.Remove(route);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}