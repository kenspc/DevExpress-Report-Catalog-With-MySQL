using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ReportCatalog.Services
{
    public class ReportStorageWebExtension1 : DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension
    {
        //readonly string ReportDirectory;
        //const string FileExtension = ".repx";

        private DataTable reportsTable = new DataTable();
        private IConfiguration Configuration;
        private MySqlDataAdapter reportsTableAdapter;

        //public ReportStorageWebExtension1(IWebHostEnvironment env)
        //{
        //    ReportDirectory = Path.Combine(env.ContentRootPath, "Reports");
        //    if (!Directory.Exists(ReportDirectory))
        //    {
        //        Directory.CreateDirectory(ReportDirectory);
        //    }
        //}

        public ReportStorageWebExtension1(IConfiguration _configuration)
        {
            Configuration = _configuration;
            reportsTableAdapter =
                new MySqlDataAdapter("Select * from rpt_setup", new MySqlConnection(Configuration.GetConnectionString("DefaultConnection").Replace("XpoProvider=MySql;", "")));
            MySqlCommandBuilder builder = new MySqlCommandBuilder(reportsTableAdapter);
            reportsTableAdapter.InsertCommand = builder.GetInsertCommand();
            reportsTableAdapter.UpdateCommand = builder.GetUpdateCommand();
            reportsTableAdapter.DeleteCommand = builder.GetDeleteCommand();
            reportsTableAdapter.Fill(reportsTable);
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = reportsTable.Columns[0];
            reportsTable.PrimaryKey = keyColumns;
        }

        public override bool CanSetData(string url)
        {
            // Determines whether or not it is possible to store a report by a given URL.
            // For instance, make the CanSetData method return false for reports that should be read-only in your storage.
            // This method is called only for valid URLs (i.e., if the IsValidUrl method returned true) before the SetData method is called.
            
            //return true;
            return GetUrls()[url].Contains("ReadOnly") ? false : true;
        }
        public override bool IsValidUrl(string url)
        {
            // Determines whether or not the URL passed to the current Report Storage is valid.
            // For instance, implement your own logic to prohibit URLs that contain white spaces or some other special characters.
            // This method is called before the CanSetData and GetData methods.
            return true;
        }
        public override byte[] GetData(string url)
        {
            // Returns report layout data stored in a Report Storage using the specified URL.
            // This method is called only for valid URLs after the IsValidUrl method is called.

            //try
            //{
            //    if (Directory.EnumerateFiles(ReportDirectory).Select(Path.GetFileNameWithoutExtension).Contains(url))
            //    {
            //        return File.ReadAllBytes(Path.Combine(ReportDirectory, url + FileExtension));
            //    }
            //    throw new DevExpress.XtraReports.Web.ClientControls.FaultException(string.Format("Could not find report '{0}'.", url));
            //}
            //catch (Exception)
            //{
            //    throw new DevExpress.XtraReports.Web.ClientControls.FaultException(string.Format("Could not find report '{0}'.", url));
            //}

            // Get the report data from the storage.
            DataRow row = reportsTable.Rows.Find(Guid.Parse(url));
            if (row == null) return null;

            byte[] reportData = (Byte[])row["ReportData"];
            return reportData;
        }
        public override Dictionary<string, string> GetUrls()
        {
            // Returns a dictionary of the existing report URLs and display names.
            // This method is called when running the Report Designer,
            // before the Open Report and Save Report dialogs are shown and after a new report is saved to a storage.

            //return Directory.GetFiles(ReportDirectory, "*" + FileExtension)
            //.Select(Path.GetFileNameWithoutExtension)
            //.ToDictionary<string, string>(x => x);

            reportsTable.Clear();
            reportsTableAdapter.Fill(reportsTable);
            // Get URLs and display names for all reports available in the storage.
            var v = reportsTable.AsEnumerable()
                  .ToDictionary<DataRow, string, string>(dataRow => ((Guid)dataRow["Id"]).ToString(),
                                                         dataRow => (string)dataRow["RptDesc"]);
            return v;
        }
        public override void SetData(XtraReport report, string url)
        {
            // Stores the specified report to a Report Storage using the specified URL.
            // This method is called only after the IsValidUrl and CanSetData methods are called.

            //report.SaveLayoutToXml(Path.Combine(ReportDirectory, url + FileExtension));

            // Write a report to the storage under the specified URL.
            DataRow row = reportsTable.Rows.Find(Guid.Parse(url));
            if (row != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    report.SaveLayoutToXml(ms);
                    row["ReportData"] = ms.GetBuffer();
                }
                reportsTableAdapter.Update(reportsTable);
            }
        }
        public override string SetNewData(XtraReport report, string defaultUrl)
        {
            // Stores the specified report using a new URL.
            // The IsValidUrl and CanSetData methods are never called before this method.
            // You can validate and correct the specified URL directly in the SetNewData method implementation
            // and return the resulting URL used to save a report in your storage.

            //SetData(report, defaultUrl);
            //return defaultUrl;

            // Append "1" if a new report name already exists.
            if (GetUrls().ContainsValue(defaultUrl)) defaultUrl = string.Concat(defaultUrl, "1");

            // Save a report to the storage with a new URL. 
            // The defaultUrl parameter is the report name that the user specifies.
            DataRow row = reportsTable.NewRow();
            row["Id"] = Guid.NewGuid();
            row["RptCode"] = defaultUrl;
            row["RptDesc"] = defaultUrl;
            row["Filename"] = defaultUrl;
            row["Disabled"] = false;
            using (MemoryStream ms = new MemoryStream())
            {
                report.SaveLayoutToXml(ms);
                row["ReportData"] = ms.GetBuffer();
            }
            reportsTable.Rows.Add(row);
            reportsTableAdapter.Update(reportsTable);
            // Refill the dataset to obtain the actual value of the new row's autoincrement key field.
            reportsTable.Clear();
            reportsTableAdapter.Fill(reportsTable);
            return reportsTable.AsEnumerable().
                FirstOrDefault(x => x["RptDesc"].ToString() == defaultUrl)["Id"].ToString();
        }
    }
}
