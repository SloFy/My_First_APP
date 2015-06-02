using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using My_First_APP;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
namespace My_First_APP.Controllers
{
    public class ShowController : Controller
    {
        private FITNESSEntities db = new FITNESSEntities();
          
        // GET: Show
        public async Task<ActionResult> Index()
        {
                      string login = User.Identity.GetUserName();
            if (login == "" || login == null)
                return View("~/Views/Account/Registration.cshtml");

            //var blank = db.Blank.Include(b => b.BlankType).Include(b => b.Person);
            var blank = db.Blank.Where(b => b.Person.Account.Login == login);
            return View(await blank.ToListAsync());
        }

        // GET: Show/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blank blank = await db.Blank.FindAsync(id);
                        if (blank == null)
            {
                return HttpNotFound();
            }
                        ViewBag.Title = blank.BlankType.Name;
            var CurrentCharacteristicsValue = db.CurrentCharacteristicsValue.Where(c => c.BlankId == blank.Id);
            return View(await CurrentCharacteristicsValue.ToListAsync());
        }

        // GET: Show/Create
        public ActionResult Create()
        {
            ViewBag.TypeId = new SelectList(db.BlankType, "Id", "Name");
            ViewBag.Person_Id = new SelectList(db.Person, "Id", "FirstName");
            return View();
        }

        // POST: Show/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,CreateDate,TypeId,Person_Id")] Blank blank)
        {
            if (ModelState.IsValid)
            {
                db.Blank.Add(blank);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TypeId = new SelectList(db.BlankType, "Id", "Name", blank.TypeId);
            ViewBag.Person_Id = new SelectList(db.Person, "Id", "FirstName", blank.Person_Id);
            return View(blank);
        }

        // GET: Show/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blank blank = await db.Blank.FindAsync(id);
            if (blank == null)
            {
                return HttpNotFound();
            }
            ViewBag.TypeId = new SelectList(db.BlankType, "Id", "Name", blank.TypeId);
            ViewBag.Person_Id = new SelectList(db.Person, "Id", "FirstName", blank.Person_Id);
            return View(blank);
        }
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,CreateDate,TypeId,Person_Id")] Blank blank)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blank).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.TypeId = new SelectList(db.BlankType, "Id", "Name", blank.TypeId);
            ViewBag.Person_Id = new SelectList(db.Person, "Id", "FirstName", blank.Person_Id);
            return View(blank);
        }
        public async Task<ActionResult> EditChar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CurrentCharacteristicsValue value = await db.CurrentCharacteristicsValue.FindAsync(id);
            if (value == null)
            {
                return HttpNotFound();
            }
            return View(value);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditChar(CurrentCharacteristicsValue new_value)
        {
            if (ModelState.IsValid)
            {
                db.Entry(new_value).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(new_value);
        }
        // GET: Show/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blank blank = await db.Blank.FindAsync(id);
            if (blank == null)
            {
                return HttpNotFound();
            }
            return View(blank);
        }

        // POST: Show/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Blank blank = await db.Blank.FindAsync(id);            
            db.CurrentCharacteristicsValue.RemoveRange(blank.CurrentCharacteristicsValue);
            db.Blank.Remove(blank);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
