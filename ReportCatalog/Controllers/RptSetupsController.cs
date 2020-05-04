using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReportCatalog.Data;
using ReportCatalog.Models;

namespace ReportCatalog.Controllers
{
    public class RptSetupsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RptSetupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult ReportList()
        {
            ViewData["ReportId"] = new SelectList(_context.RptSetups, "Id", "RptDesc");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ViewReport(Guid id)
        {
            DesignerTask ADT = new DesignerTask
            {
                reportID = id
            };
            HttpContext.Session.Set<DesignerTask>("DesignerTask", ADT);

            return RedirectToAction("Viewer", "Home");
        }

        public IActionResult NewReport()
        {
            DesignerTask ADT = new DesignerTask
            {
                mode = ReportEdditingMode.NewReport,
            };
            HttpContext.Session.Set<DesignerTask>("DesignerTask", ADT);

            return RedirectToAction("Designer", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditReport(Guid id)
        {
            DesignerTask ADT = new DesignerTask
            {
                mode = ReportEdditingMode.ModifyReport,
                reportID = id
            };
            HttpContext.Session.Set<DesignerTask>("DesignerTask", ADT);

            return RedirectToAction("Designer", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReport(Guid id)
        {
            var rptsetup = await _context.RptSetups.FindAsync(id);
            _context.RptSetups.Remove(rptsetup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ReportList));
        }
    }
}
