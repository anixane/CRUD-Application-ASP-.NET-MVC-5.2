using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnimeshDB.Models;
using System.Data.Entity.Validation;
using System.Data.Entity;

namespace AnimeshDB.Controllers
{
    public class MovieController : Controller
    {
        // GET: Movie
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData()
        {
            using (DBModel db = new DBModel())
            {
                List<Animesh> movList = db.Animeshes.ToList<Animesh>();
                return Json(new { data = movList }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0) 
                 return View(new Animesh());
            else
            {
                using (DBModel db = new DBModel())
                {
                    return View(db.Animeshes.Where(x => x.ID == id).FirstOrDefault<Animesh>());
                }
                   
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(Animesh mov)
        {
            using (DBModel db = new DBModel())
            {
                try
                {
                    if (mov.ID == 0)
                    {
                        db.Animeshes.Add(mov);
                        db.SaveChanges();
                        return Json(new { success = true, message = "Saved Successfully!!" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        db.Entry(mov).State = EntityState.Modified;
                        db.SaveChanges();
                        return Json(new { success = true, message = "Updated Successfully!!" }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }

        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (DBModel db = new DBModel())
            {
                Animesh mov = db.Animeshes.Where(x => x.ID == id).FirstOrDefault<Animesh>();
                db.Animeshes.Remove(mov);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully!!" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}