using CrystalDecisions.CrystalReports.Engine;

using EIMS.Models;
using EIMS.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Mvc;

namespace EIMS.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Reporting()
        {
            InternDAL intObj = new InternDAL();
            ProjTaskDAL projObj = new ProjTaskDAL();
            ViewBag.projectList = projObj.GetAllProject().AsEnumerable().Select(row =>
            new SelectListItem
            {
                Selected = false,
                Text = row.ProjectName,
                Value = row.ProjId.ToString()
            });
            ViewBag.data = intObj.GetAllPreferences().AsEnumerable().Select(row =>
                new SelectListItem
                {
                    Selected = false,
                    Text = row["PreferenceCategory"].ToString(),
                    Value = row["PreId"].ToString()
                });
            return View("Reporting");
        }
        public ActionResult ViewReport(ReportModel objModel, FormCollection form)
        {
            if (!string.IsNullOrEmpty(form["ddlProject"].ToString()))
            {
                objModel.ProjectId = int.Parse(form["ddlProject"].ToString().Trim());
            }
            if (!string.IsNullOrEmpty(form["ddlCategory"].ToString()))
            {

                objModel.PrefId = int.Parse(form["ddlCategory"].ToString().Trim());
            }

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "Certificate.rpt"));
            rd.RecordSelectionFormula = "";
            if (objModel.ProjectId != null)
            {
                rd.RecordSelectionFormula = "{tblInternInfo.ProjId} = " + objModel.ProjectId;
            }
            if (objModel.InternId != null)
            {
                if (rd.RecordSelectionFormula != "") { rd.RecordSelectionFormula += "and "; }
                rd.RecordSelectionFormula += "{tblProj_Task.InternId} = " + objModel.InternId;
            }
            if (objModel.WeekId != null)
            {
                if (rd.RecordSelectionFormula != "") { rd.RecordSelectionFormula += "and "; }
                rd.RecordSelectionFormula += "{tblProj_Task.WeekId} = " + objModel.WeekId;
            }
            if (objModel.PrefId != null)
            {
                if (rd.RecordSelectionFormula != "") { rd.RecordSelectionFormula += "and "; }
                rd.RecordSelectionFormula += "{tblInternInfo.PreId} = " + objModel.PrefId;
            }
            Response.Buffer = false;
            Response.ClearHeaders();
            Response.ClearContent();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "Report.pdf");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}