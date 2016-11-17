using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using SmartBI.Models;
using PagedList;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;
using ClosedXML.Excel;
using System.Data;
using System.Reflection;
using OfficeOpenXml;
using SmartBI.Classes;
using System.Data.Entity.Core.Objects;
using System.Threading;

namespace SmartBI.Controllers
{
    public class HomeController : Controller
    {
        Entities db = new Entities();
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult GetZni(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.ALL_ZNI_BUGS_JIRA select i;
            switch (sortOrder)
            {
                case "name_desc":
                    items=items.OrderByDescending(s=>s.ISSUE);
                    break;
                case "Date":
                    items = items.OrderBy(s => s.RESOLUTIONDATE);
                    break;
                case "date_desc":
                    items = items.OrderByDescending(s => s.RESOLUTIONDATE);
                    break;
                default:
                    items = items.OrderBy(s => s.ISSUE);
                    break;
            }
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult GetLabor(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.CONF_REP_LABOR_JIRA_HPSM select i;
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.КОДЗАДАЧИ);
                    break;
                case "Date":
                    items = items.OrderBy(s => s.ДАТА_РАБОТЫ);
                    break;
                case "date_desc":
                    items = items.OrderByDescending(s => s.ДАТА_РАБОТЫ);
                    break;
                default:
                    items = items.OrderBy(s => s.КОДЗАДАЧИ);
                    break;
            }
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult GetActSpecASBPS()
        {
            return View(Oracle_ADO_Worker.GetActSpecASBPSContext());
        }

        public ActionResult GetActSpecASBPS_OCT(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.ASBPS_ACT_SPEC_OCT select i;
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.КОДЗАДАЧИ);
                    break;
                case "Date":
                    items = items.OrderBy(s => s.ДАТА_РАБОТЫ);
                    break;
                case "date_desc":
                    items = items.OrderByDescending(s => s.ДАТА_РАБОТЫ);
                    break;
                default:
                    items = items.OrderBy(s => s.КОДЗАДАЧИ);
                    break;
            }
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult GetActSpecNKFO2_OCT(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.NKFO2_ACT_SPEC_OCT select i;
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.КОДЗАДАЧИ);
                    break;
                case "Date":
                    items = items.OrderBy(s => s.ДАТА_РАБОТЫ);
                    break;
                case "date_desc":
                    items = items.OrderByDescending(s => s.ДАТА_РАБОТЫ);
                    break;
                default:
                    items = items.OrderBy(s => s.КОДЗАДАЧИ);
                    break;
            }
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult GetActSpecSDBO_OCT(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.SDBO_ACT_SPEC_OCT select i;
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.КОДЗАДАЧИ);
                    break;
                case "Date":
                    items = items.OrderBy(s => s.ДАТА_РАБОТЫ);
                    break;
                case "date_desc":
                    items = items.OrderByDescending(s => s.ДАТА_РАБОТЫ);
                    break;
                default:
                    items = items.OrderBy(s => s.КОДЗАДАЧИ);
                    break;
            }
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult GetZNI_BPS_SUM_PROJECT_ACTIVITY()
        {
            var items = from i in db.SUM_PROJECT_ACTIVITY select i;
            return View(items);
        }

        public ActionResult GetZNI_BPS_SUM_TASK_NOT_LINKED_ASBPS()
        {
            var items = from i in db.SUM_TASK_NOT_LINKED_ASBPS select i;
            return View(items);
        }

        public ActionResult GetZNI_BPS_SUM_ZNI_TASK_ASBPS()
        {
            var items = from i in db.SUM_ZNI_TASK_ASBPS select i;
            return View(items);
        }

        public ActionResult GetZNI_BPS()
        {
            return View(new ZNI_BPS(db));
        }
        
        public void ExportToExcel_closeXML()
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                var items = (from i in db.ALL_ZNI_BUGS_JIRA select new { 

                    ID=i.ID,
                    ISSUE=i.ISSUE,
                    PROJECT=i.PROJECT,
                    COMPONENT=i.COMPONENT,
                    REPORTER=i.REPORTER,
                    ASSIGNEE=i.ASSIGNEE,
                    CREATOR=i.CREATOR,
                    ISSUETYPE=i.ISSUETYPE,
                    SUMMARY=i.SUMMARY,
                    DESCRIPTION=i.DESCRIPTION,
                    ENVIRONMENT=i.ENVIRONMENT,
                    PRIORITY=i.PRIORITY,
                    RESOLUTION=i.RESOLUTION,
                    STATUS=i.STATUS,
                    CREATED=EntityFunctions.TruncateTime(i.CREATED),
                    UPDATED=EntityFunctions.TruncateTime(i.UPDATED),
                    DUEDATE=EntityFunctions.TruncateTime(i.DUEDATE),
                    RESOLUTIONDATE=EntityFunctions.TruncateTime(i.RESOLUTIONDATE)}).ToList();

                DataTable dt = ConvertListToDataTable(items);
                dt.TableName = "zni";
                var ws = wb.Worksheets.Add(dt);
                var dateColumns = from DataColumn d in dt.Columns
                                  where d.DataType == typeof(DateTime)
                                  select d.Ordinal + 1;

                foreach (var dr in ws.Rows(2,ws.Rows().Count()))
                {
                    foreach (var dc in dateColumns)
                    {
                        ws.Cell(dr.RowNumber(), dc).Style.NumberFormat.Format = "DD.MM.yyyy";
                    }
                }

                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= Report.xlsx");
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }

        public void ExportToExcel_epplus_LABOR_JIRA_HPSM()
        {
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode("Labor.xlsx", System.Text.Encoding.UTF8));

            using (ExcelPackage pck = new ExcelPackage())
            {
                var items = (from i in db.CONF_REP_LABOR_JIRA_HPSM select new {
                    СИСТЕМА = i.СИСТЕМА,
                    ПОДРАЗДНИЗ = i.ПОДРАЗДНИЗ,
                    ПОДРАЗДВЕРХ = i.ПОДРАЗДВЕРХ,
                    КАТЕГОРИЯ = i.Категория,
                    PROJECT = i.PROJECT,
                    КОДЗНИ = i.КОДЗНИ,
                    НОМЕР_ЗНИ = i.НОМЕР_ЗНИ,
                    ДАТА_РЕГ = EntityFunctions.TruncateTime(i.ДАТА_РЕГ),
                    СТАТУСЗНИ = i.СТАТУСЗНИ,
                    ДАТАРЕЗОЛЗНИ = EntityFunctions.TruncateTime(i.ДАТАРЕЗОЛЗНИ),
                    КОДЗАДАЧИ = i.КОДЗАДАЧИ,
                    ТИПЗАПРОСА = i.ТИПЗАПРОСА,
                    СТАТУСЗАДАЧИ = i.СТАТУСЗАДАЧИ,
                    ИСПОЛНИТЕЛЬ = i.ИСПОЛНИТЕЛЬ,
                    ДАТАСОЗДАНИЯ = EntityFunctions.TruncateTime(i.ДАТАСОЗДАНИЯ),
                    ДАТАРЕЗОЛЗАДАЧИ = EntityFunctions.TruncateTime(i.ДАТАРЕЗОЛЗАДАЧИ),
                    ДАТА_РАБОТЫ = EntityFunctions.TruncateTime(i.ДАТА_РАБОТЫ),
                    АС_НОМЕР = i.АС_НОМЕР,
                    АС_НАЗВАНИЕ = i.АС_НАЗВАНИЕ,
                    СУМТРУД = i.СУМТРУД,
                    ПОЛНОЕ_ИМЯ = i.ПОЛНОЕ_ИМЯ,
                    GRADE = i.GRADE,
                    CATEGORY = i.CATEGORY,
                    ТРУДОЗАТРАТЫ = i.ТРУДОЗАТРАТЫ,
                    WORK_TYPE_VAL = i.WORK_TYPE_VAL,
                    ACCOUNT_VAL = i.ACCOUNT_VAL,
                    ID = i.ID,
                    ПРИОРИТЕТ = i.ПРИОРИТЕТ,
                    СТАТУС_СОТР = i.СТАТУС_СОТР}).ToList();

                DataTable dt = ConvertListToDataTable(items);


                var dateColumns = from DataColumn d in dt.Columns
                                  where d.DataType == typeof(DateTime) || d.ColumnName.Contains("ДАТА")
                                  select d.Ordinal + 1;

                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Трудозатраты");
                
                foreach (var dc in dateColumns)
                {
                    ws.Cells[2, dc, dt.Rows.Count + 2, dc].Style.Numberformat.Format = "DD.MM.yyyy";
                }
                                
                ws.Cells["A1"].LoadFromDataTable(dt, true);
                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                var ms = new System.IO.MemoryStream();
                pck.SaveAs(ms);
                ms.WriteTo(Response.OutputStream);
            }
        }

        public void ExportToExcel_epplus_act_spec_asbps()
        {
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode("Labor.xlsx", System.Text.Encoding.UTF8));
            using (ExcelPackage pck = new ExcelPackage())
            {
                var items = (from i in db.ASBPS_ACT_SPEC_OCT
                             select new
                             {
                                 ПОДРАЗДНИЗ = i.ПОДРАЗДНИЗ,
                                 ПОДРАЗДВЕРХ = i.ПОДРАЗДВЕРХ,
                                 КОДЗНИ = i.КОДЗНИ,
                                 НОМЕР_ЗНИ = i.НОМЕР_ЗНИ,
                                 ДАТА_РЕГ = EntityFunctions.TruncateTime(i.ДАТА_РЕГ),
                                 МЕНЕДЖЕР_ДИТ = i.МЕНЕДЖЕР_ДИТ,
                                 ИСТОЧНИК_ЗНИ = i.ИСТОЧНИК_ЗНИ,
                                 ДАТА_РЕГ_ЗНИ = EntityFunctions.TruncateTime(i.ДАТА_РЕГ_ЗНИ),
                                 СТАТУСЗНИ = i.СТАТУСЗНИ,
                                 ДАТАРЕЗОЛЗНИ = EntityFunctions.TruncateTime(i.ДАТАРЕЗОЛЗНИ),
                                 КОДЗАДАЧИ = i.КОДЗАДАЧИ,
                                 ОПИСАНИЕ_РАБОТЫ = i.ОПИСАНИЕ_РАБОТЫ,
                                 СТАТУСЗАДАЧИ = i.СТАТУСЗАДАЧИ,
                                 ДАТАРЕЗОЛЗАДАЧИ = EntityFunctions.TruncateTime(i.ДАТАРЕЗОЛЗАДАЧИ),
                                 ДАТАСОЗДЗАДАЧИ = EntityFunctions.TruncateTime(i.ДАТАСОЗДЗАДАЧИ),
                                 ДАТА_РАБОТЫ = EntityFunctions.TruncateTime(i.ДАТА_РАБОТЫ),
                                 АС_НОМЕР = i.АС_НОМЕР,
                                 АС_НАЗВАНИЕ = i.АС_НАЗВАНИЕ,
                                 СУМТРУД = i.СУМТРУД,
                                 ПОЛНОЕ_ИМЯ = i.ПОЛНОЕ_ИМЯ,
                                 GRADE = i.GRADE,
                                 ТРУДОЗАТРАТЫ = i.ТРУДОЗАТРАТЫ,
                                 WORK_TYPE_VAL = i.WORK_TYPE_VAL
                             }).ToList();

                DataTable dt = ConvertListToDataTable(items);


                var dateColumns = from DataColumn d in dt.Columns
                                  where d.DataType == typeof(DateTime) || d.ColumnName.Contains("ДАТА")
                                  select d.Ordinal + 1;

                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Трудозатраты");

                foreach (var dc in dateColumns)
                {
                    ws.Cells[2, dc, dt.Rows.Count + 2, dc].Style.Numberformat.Format = "DD.MM.yyyy";
                }

                ws.Cells["A1"].LoadFromDataTable(dt, true);
                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                var ms = new System.IO.MemoryStream();
                pck.SaveAs(ms);
                ms.WriteTo(Response.OutputStream);
            }
        }

        public void ExportToExcel_epplus_act_spec_nkfo2()
        {
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode("Labor.xlsx", System.Text.Encoding.UTF8));

            using (ExcelPackage pck = new ExcelPackage())
            {
                var items = (from i in db.NKFO2_ACT_SPEC_OCT
                             select new
                             {
                                 ПОДРАЗДНИЗ = i.ПОДРАЗДНИЗ,
                                 ПОДРАЗДВЕРХ = i.ПОДРАЗДВЕРХ,
                                 КОДЗНИ = i.КОДЗНИ,
                                 НОМЕР_ЗНИ = i.НОМЕР_ЗНИ,
                                 ДАТА_РЕГ = EntityFunctions.TruncateTime(i.ДАТА_РЕГ),
                                 МЕНЕДЖЕР_ДИТ = i.МЕНЕДЖЕР_ДИТ,
                                 ИСТОЧНИК_ЗНИ = i.ИСТОЧНИК_ЗНИ,
                                 ДАТА_РЕГ_ЗНИ = EntityFunctions.TruncateTime(i.ДАТА_РЕГ_ЗНИ),
                                 СТАТУСЗНИ = i.СТАТУСЗНИ,
                                 ДАТАРЕЗОЛЗНИ = i.ДАТАРЕЗОЛЗНИ,
                                 КОДЗАДАЧИ = i.КОДЗАДАЧИ,
                                 ОПИСАНИЕ_РАБОТЫ = i.ОПИСАНИЕ_РАБОТЫ,
                                 СТАТУСЗАДАЧИ = i.СТАТУСЗАДАЧИ,
                                 ДАТАРЕЗОЛЗАДАЧИ = EntityFunctions.TruncateTime(i.ДАТАРЕЗОЛЗАДАЧИ),
                                 ДАТАСОЗДЗАДАЧИ = EntityFunctions.TruncateTime(i.ДАТАСОЗДЗАДАЧИ),
                                 ДАТА_РАБОТЫ = EntityFunctions.TruncateTime(i.ДАТА_РАБОТЫ),
                                 АС_НОМЕР = i.АС_НОМЕР,
                                 АС_НАЗВАНИЕ = i.АС_НАЗВАНИЕ,
                                 СУМТРУД = i.СУМТРУД,
                                 ПОЛНОЕ_ИМЯ = i.ПОЛНОЕ_ИМЯ,
                                 GRADE = i.GRADE,
                                 ТРУДОЗАТРАТЫ = i.ТРУДОЗАТРАТЫ,
                                 WORK_TYPE_VAL = i.WORK_TYPE_VAL}).ToList();

                DataTable dt = ConvertListToDataTable(items);


                var dateColumns = from DataColumn d in dt.Columns
                                  where d.DataType == typeof(DateTime) || d.ColumnName.Contains("ДАТА")
                                  select d.Ordinal + 1;

                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Трудозатраты NKFO2");

                foreach (var dc in dateColumns)
                {
                    ws.Cells[2, dc, dt.Rows.Count + 2, dc].Style.Numberformat.Format = "DD.MM.yyyy";
                }

                ws.Cells["A1"].LoadFromDataTable(dt, true);
                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                var ms = new System.IO.MemoryStream();
                pck.SaveAs(ms);
                ms.WriteTo(Response.OutputStream);
            }
        }

        public void ExportToExcel_epplus_act_spec_sdbo()
        {
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode("Labor.xlsx", System.Text.Encoding.UTF8));

            using (ExcelPackage pck = new ExcelPackage())
            {
                var items = (from i in db.SDBO_ACT_SPEC_OCT
                             select new
                             {
                                 ПОДРАЗДНИЗ = i.ПОДРАЗДНИЗ,
                                 ПОДРАЗДВЕРХ = i.ПОДРАЗДВЕРХ,
                                 КОДЗНИ = i.КОДЗНИ,
                                 НОМЕР_ЗНИ = i.НОМЕР_ЗНИ,
                                 ДАТА_РЕГ = EntityFunctions.TruncateTime(i.ДАТА_РЕГ),
                                 МЕНЕДЖЕР_ДИТ = i.МЕНЕДЖЕР_ДИТ,
                                 ИСТОЧНИК_ЗНИ = i.ИСТОЧНИК_ЗНИ,
                                 ДАТА_РЕГ_ЗНИ = EntityFunctions.TruncateTime(i.ДАТА_РЕГ_ЗНИ),
                                 СТАТУСЗНИ = i.СТАТУСЗНИ,
                                 ДАТАРЕЗОЛЗНИ = i.ДАТАРЕЗОЛЗНИ,
                                 КОДЗАДАЧИ = i.КОДЗАДАЧИ,
                                 ОПИСАНИЕ_РАБОТЫ = i.ОПИСАНИЕ_РАБОТЫ,
                                 СТАТУСЗАДАЧИ = i.СТАТУСЗАДАЧИ,
                                 ДАТАРЕЗОЛЗАДАЧИ = EntityFunctions.TruncateTime(i.ДАТАРЕЗОЛЗАДАЧИ),
                                 ДАТАСОЗДЗАДАЧИ = EntityFunctions.TruncateTime(i.ДАТАСОЗДЗАДАЧИ),
                                 ДАТА_РАБОТЫ = EntityFunctions.TruncateTime(i.ДАТА_РАБОТЫ),
                                 АС_НОМЕР = i.АС_НОМЕР,
                                 АС_НАЗВАНИЕ = i.АС_НАЗВАНИЕ,
                                 СУМТРУД = i.СУМТРУД,
                                 ПОЛНОЕ_ИМЯ = i.ПОЛНОЕ_ИМЯ,
                                 GRADE = i.GRADE,
                                 ТРУДОЗАТРАТЫ = i.ТРУДОЗАТРАТЫ,
                                 WORK_TYPE_VAL = i.WORK_TYPE_VAL
                             }).ToList();

                DataTable dt = ConvertListToDataTable(items);


                var dateColumns = from DataColumn d in dt.Columns
                                  where d.DataType == typeof(DateTime) || d.ColumnName.Contains("ДАТА")
                                  select d.Ordinal + 1;

                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Трудозатраты SDBO");

                foreach (var dc in dateColumns)
                {
                    ws.Cells[2, dc, dt.Rows.Count + 2, dc].Style.Numberformat.Format = "DD.MM.yyyy";
                }

                ws.Cells["A1"].LoadFromDataTable(dt, true);
                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                var ms = new System.IO.MemoryStream();
                pck.SaveAs(ms);
                ms.WriteTo(Response.OutputStream);
            }
        }

        static DataTable ConvertListToDataTable<T>(List<T> list)
        {
            DataTable table = new DataTable();
            PropertyInfo[] oProps = null;
            if (list == null) return table;
            foreach (T item in list)
            {
                if (oProps == null)
                {
                    oProps = ((Type)item.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;
                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }
                        table.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }
                DataRow dr = table.NewRow();
                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(item, null) == null ? DBNull.Value : pi.GetValue(item, null);
                }
                table.Rows.Add(dr);
            }
            return table;
        }

    }
}