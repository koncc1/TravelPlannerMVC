using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelPlannerMVC.Data;
using TravelPlannerMVC.Models;

namespace TravelPlannerMVC.Controllers
{
    /// <summary>
    /// Controller for managing travel routes (CRUD operations + details view)
    /// </summary>
    public class TravelRoutesController : Controller
    {
        #region Fields & Constructor

        private readonly ApplicationDbContext _context;

        public TravelRoutesController(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Routes - List

        /// <summary>
        /// Displays all available travel routes
        /// </summary>
        public IActionResult Index()
        {
            var routes = _context.TravelRoutes.ToList();
            return View(routes);
        }

        #endregion

        #region Route - Details

        /// <summary>
        /// Displays details of a specific travel route
        /// </summary>
        public IActionResult Details(int id)
        {
            var route = _context.TravelRoutes.Find(id);

            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

        #endregion

        #region Create Route

        /// <summary>
        /// Shows form for creating a new travel route (Admin/Manager only)
        /// </summary>
        [Authorize(Roles = "Admin,Manager")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Handles creation of a new travel route
        /// </summary>
        [Authorize(Roles = "Admin,Manager")]
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

        #endregion

        #region Edit Route

        /// <summary>
        /// Shows edit form for existing travel route (Admin/Manager only)
        /// </summary>
        [Authorize(Roles = "Admin,Manager")]
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

        /// <summary>
        /// Handles update of existing travel route
        /// </summary>
        [Authorize(Roles = "Admin,Manager")]
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

        #endregion

        #region Delete Route

        /// <summary>
        /// Shows confirmation page for deleting a route (Admin/Manager only)
        /// </summary>
        [Authorize(Roles = "Admin,Manager")]
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

        /// <summary>
        /// Confirms deletion of a travel route
        /// </summary>
        [Authorize(Roles = "Admin,Manager")]
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

        #endregion
    }
}