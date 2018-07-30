
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace WeddingPlanner.Controllers{
    public class HomeController : Controller{

        private WeddingPlannerContext _context;
        public HomeController(WeddingPlannerContext context){
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index(){
            return View();
        }

        [HttpGet]
        [Route("Dashboard")]
        public IActionResult AllWeddings(){
            List<Wedd> allWeddings = _context.weddings.Include(wedding => wedding.user).ToList();
            ViewBag.weddings = allWeddings;

             int? userId = HttpContext.Session.GetInt32("userId");
            Users userLog = _context.users.SingleOrDefault(user => user.id == userId);
            ViewBag.currentUser = userLog;

            List<WedPlan> allConnections = _context.weddingplan.Include(wp => wp.user).Include(wp => wp.wedding).ToList();
            ViewBag.allConnections = allConnections;
            
            return View("AllWeddings");
        }

        [HttpGet]
        [Route("Planner")]
        public IActionResult Planner(){
            return View("Planner");
        }

        [HttpGet]
        [Route("Wedding/{id}")]
        public IActionResult Details(int id){
            if(id > 0){
                Wedd weddLog = _context.weddings.SingleOrDefault(wedding => wedding.id == id);
                List<WedPlan> weddToUsers = _context.weddingplan.Where(wp => wp.weddingId == id).Include(wp => wp.user).ToList();
                ViewBag.wedding = weddLog;
                ViewBag.weddings = weddToUsers;
                return View("Details");
            }
            return RedirectToAction("Planner");
        }

        // ****************** Log in Method ************************
        [HttpPost]
        public IActionResult LoginMethod(string email, string password){
            Users userLog = _context.users.SingleOrDefault(user => user.email == email);

            if(userLog != null && password != null){
                var hasher = new PasswordHasher<Users>();

                if(0 != hasher.VerifyHashedPassword(userLog, userLog.password, password)){
                    HttpContext.Session.SetInt32("userId", userLog.id);
                    return RedirectToAction("AllWeddings");
                }
            }
            return RedirectToAction("Index");
        }

        // ********************* Register Method **********************
        [HttpPost]
        public IActionResult Create(Users userRegister, string password){
            Users userLog = _context.users.SingleOrDefault(user => user.email == userRegister.email);
            if(userLog != null){
                return RedirectToAction("Index");
            }
            else if(ModelState.IsValid && userRegister.password == password){
              
                PasswordHasher<Users> hasher = new PasswordHasher<Users>();
                userRegister.password = hasher.HashPassword(userRegister, userRegister.password);
                userRegister.created_at = DateTime.Now;
                _context.Add(userRegister);
                _context.SaveChanges();
                userLog = _context.users.SingleOrDefault(user => user.email == userRegister.email);

                HttpContext.Session.SetInt32("userId", userLog.id);
                return RedirectToAction("AllWeddings");
            }
            else{
                return RedirectToAction("Index");
            }
        }

        // ************************ create New Wedding **************************** 
        [HttpPost]
        public IActionResult NewWedding(Wedd wedd){
            // get user id from session
            int? userId = HttpContext.Session.GetInt32("userId");
             // find user by user id
            Users userLog = _context.users.SingleOrDefault(user => user.id == userId);

            if(userLog != null){
                wedd.userId = userLog.id;
                if(ModelState.IsValid && (wedd.created_at > DateTime.Now)){
                    userLog.wedd.Add(wedd);
                    _context.Add(wedd);
                    // HttpContext.Session.SetInt32("weddId", wedd.id);
                    _context.SaveChanges();
                    return RedirectToAction("Details", new{id = wedd.id});
                }
                return RedirectToAction("Planner");
            }
            return RedirectToAction("Index");
        }

        // ***************************** Delete wedding**********************************
        public IActionResult Delete(int id){
            Wedd weddLog = _context.weddings.SingleOrDefault(wed => wed.id == id);
            _context.Remove(weddLog);
            _context.SaveChanges();
            return RedirectToAction("AllWeddings");
        }

        // add loged user to a selected wedding
        public IActionResult Rsvp(int id){
            int? userId = HttpContext.Session.GetInt32("userId");
             // find user by user id
            Users userLog = _context.users.SingleOrDefault(user => user.id == userId);
            Wedd weddLog = _context.weddings.SingleOrDefault(wed => wed.id == id);
            WedPlan wedPlan = new WedPlan(){
                wedding = weddLog,
                user = userLog
            };
            // userLog.wedPlan.
            _context.Add(wedPlan);
            _context.SaveChanges();
            return RedirectToAction("AllWeddings");
        }

        // remove user from a sepecific wedding
        public IActionResult UnRsvp (int id){
            WedPlan connection = _context.weddingplan.SingleOrDefault(wp => wp.id == id);
            if(connection != null){
                _context.Remove(connection);
                _context.SaveChanges();
            }
            return RedirectToAction("AllWeddings");
        }

        public IActionResult LogOut() {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

      


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(){
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
