using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using mysql.Models;

namespace mysql.Controllers
{
    public class HomeController : Controller
    {
        private BowlingDbContext _context { get; set; }

        public HomeController(BowlingDbContext temp)
        {
            _context = temp;
        }
        public IActionResult Index(string team)
        {
            var coolData = _context.Bowlers
                .Include(y => y.Team)
                .Where(y => y.Team.TeamName == team || team == null)
                .ToList();
            return View(coolData);
        }

        [HttpGet]
        public IActionResult AddBowler()
        {
            ViewBag.Teams = _context.teams.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult AddBowler(Bowler b)
        {
            _context.Add(b);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditBowler(int id)
        {
            ViewBag.Teams = _context.teams.ToList();
            var bowler = _context.Bowlers.Single(y => y.BowlerID == id);

            return View(bowler);
        }
        [HttpPost]
        public IActionResult EditBowler(Bowler b)
        {
            _context.Update(b);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var bowler = _context.Bowlers.Single(y => y.BowlerID == id);
            return View(bowler);
        }

        [HttpPost]
        public IActionResult Delete(Bowler b)
        {
            var deleteBowler = _context.Bowlers.Single(y => y.BowlerID == b.BowlerID);

            _context.Bowlers.Remove(deleteBowler);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
