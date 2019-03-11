using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Xml.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net.Http;
using TrackingAPI.EF;
using TrackingAPI.Models;
using TrackingAPI.IRepository;

namespace TrackingAPI.Repository
{
    public class RShipmentTracking : IShipmentTracking
    {
        public readonly DataContext _datacontext;
        private readonly IHttpClientFactory _httpClientFactory;

        public RShipmentTracking(DataContext dataContext, IHttpClientFactory httpClientFactory)
        {
            _datacontext = dataContext;
            _httpClientFactory = httpClientFactory;
        }
        //get Shipment data
        public async Task<object> GetCustomerShippmentDetails(string consignmentID)
        {
            object TrackingJobDetails = _datacontext.Tracking.FromSql("WEB_stpJob_TrackTrace @ConsignmentID={0}", consignmentID).FirstOrDefault();

            if (TrackingJobDetails == null)
            {
                //Need to check FOCIS,MT DB
                var request = new HttpRequestMessage(HttpMethod.Get, "https://eilstaging.agility.com:9443/GetJobDetails?JobNumber=100000280&TransactionID=30002248");
                var client = _httpClientFactory.CreateClient();
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var XMLdata = response.Content.ReadAsStringAsync().Result;
                    //Calling  FOCIS SP and passing XML value as input parameter
                    var command = _datacontext.Database.GetDbConnection().CreateCommand();
                    command.CommandText = "[dbo].[WEB_stpFOCISJob_TrackTrace]";
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter param1 = new SqlParameter("@JobXml", XMLdata);
                    command.Parameters.Add(param1);
                    DbDataAdapter adapter = DbProviderFactories.GetFactory(_datacontext.Database.GetDbConnection()).CreateDataAdapter();
                    adapter.SelectCommand = command;
                    DataSet Focisds = new DataSet();
                    adapter.Fill(Focisds);
                    if (Focisds.Tables[0].Rows.Count > 0)
                    {
                        Focisds.Tables[0].TableName = "FocisShipmentDetails";
                    }
                    return Focisds;
                }
            }
            return TrackingJobDetails;
        }
        //get Event data
        public async Task<DataSet> GetEventDetailsAsync(Int32 JobID, Int32 customerID)
        {
            var command = _datacontext.Database.GetDbConnection().CreateCommand();
            command.CommandText = "[dbo].[WEB_stpJobEvents_Anon]";
            command.CommandType = CommandType.StoredProcedure;
            SqlParameter param1 = new SqlParameter("@JobID", JobID);
            SqlParameter param2 = new SqlParameter("@CustomerID", customerID);
            command.Parameters.Add(param1);
            command.Parameters.Add(param2);
            DbDataAdapter adapter = DbProviderFactories.GetFactory(_datacontext.Database.GetDbConnection()).CreateDataAdapter();
            adapter.SelectCommand = command;
            DataSet Trackingds = new DataSet();
            adapter.Fill(Trackingds);
            if (Trackingds.Tables[0].Rows.Count > 0)
            {
                Trackingds.Tables[0].TableName = "EEventModel";
                Trackingds.Tables[1].TableName = "EventJobModel";
            }
            else if (Trackingds.Tables[0].Rows.Count == 0)
            {
                DataSet Focisds = new DataSet();
                var request = new HttpRequestMessage(HttpMethod.Get, "https://eilstaging.agility.com:9443/GetJobDetails?JobNumber=100000280&TransactionID=30002248");
                var client = _httpClientFactory.CreateClient();
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var XMLdata = response.Content.ReadAsStringAsync().Result;
                    var commands = _datacontext.Database.GetDbConnection().CreateCommand();
                    commands.CommandText = "[dbo].[WEB_stpFOCISJobEvents_Anon]";
                    commands.CommandType = CommandType.StoredProcedure;
                    SqlParameter params1 = new SqlParameter("@JobXml", XMLdata);
                    commands.Parameters.Add(params1);
                    DbDataAdapter adapters = DbProviderFactories.GetFactory(_datacontext.Database.GetDbConnection()).CreateDataAdapter();
                    adapters.SelectCommand = commands;
                    adapters.Fill(Focisds);
                    if (Focisds.Tables.Count > 0)
                    {
                        Focisds.Tables[0].TableName = "EEventModel";
                        Focisds.Tables[1].TableName = "empty";
                        Focisds.Tables[2].TableName = "empty1";
                        Focisds.Tables[1].TableName = "EventJobModel";
                    }
                }
                return Focisds;
            }
            return Trackingds;
        }
        //Insert VGM data
        public int InsertVGMDetails(EVGMDetailsModel model)
        {

            var ConsignmentID = new SqlParameter("@ConsignmentID", SqlDbType.NVarChar, 400).Value = model.ConsignmentID;
            var JobModeID = new SqlParameter("@JobModeID", SqlDbType.Int).Value = model.JobModeID;
            var JobID = new SqlParameter("@JobID", SqlDbType.Int).Value = model.JobID;
            var VGMDeclarationFor = new SqlParameter("@VGMDeclarationFor", SqlDbType.NVarChar, 400).Value = model.VGMDeclarationFor;

            var BookingNo = new SqlParameter("@BookingNo", SqlDbType.NVarChar, 400).Value = model.BookingNo;
            var CountryCode = new SqlParameter("@CountryCode", SqlDbType.NVarChar, 100).Value = model.CountryCode;
            var Country = new SqlParameter("@Country", SqlDbType.NVarChar, 200).Value = model.Country;
            var UOM = new SqlParameter("@UOM", SqlDbType.NVarChar, 50).Value = model.UOM;
            var TotalContainers = new SqlParameter("@TotalContainers", SqlDbType.Int).Value = model.TotalContainers;
            var Name = new SqlParameter("@Name", SqlDbType.NVarChar, 200).Value = model.Name.ToUpper();
            var CompanyEmail = new SqlParameter("@CompanyEmail", SqlDbType.NVarChar, 200).Value = model.CompanyEmail;
            var Telephone = new SqlParameter("@Telephone", SqlDbType.NVarChar, 50).Value = model.Telephone;
            var ShippersCompanyName = new SqlParameter("@ShippersCompanyName", SqlDbType.NVarChar, 400).Value = model.ShippersCompanyName;
            var ShippersAddress = new SqlParameter("@ShippersAddress", SqlDbType.NVarChar, 800).Value = model.ShippersAddress;
            var Signature = new SqlParameter("@Signature", SqlDbType.Image).Value = convertstringToByte(model.Signature);
            var Chk1 = new SqlParameter("@Chk1", SqlDbType.Bit).Value = model.Chk1;
            var XmlVal = new SqlParameter("@XmlVal", SqlDbType.Xml, 10000).Value = ""; //convertListToXML(model.VGMContainerDetails);
            var VGMID = new SqlParameter("@VGMID", SqlDbType.Int).Direction = ParameterDirection.Output;

            var RVGMID = _datacontext.Database.ExecuteSqlCommand("WEB_VGMDetails_Insert @p0, @p1,@p2," +
                   "@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12," +
                   "@p13,@p14,@p15,@p16,@p17", ConsignmentID, JobModeID, JobID,
                   VGMDeclarationFor, BookingNo, CountryCode, Country, UOM, TotalContainers, Name, CompanyEmail, Telephone,
                   ShippersCompanyName, ShippersAddress, Signature, Chk1, XmlVal, VGMID);
            _datacontext.SaveChanges();
            if (RVGMID > 0)
            {
                SendEmail(model.ShipmentPdf, model.EmailTo);
            }
            return RVGMID;

        }
        //get countrys data
        public DataSet GetCountrys(string value, Int16 customerID, Int16 UserID, Int16 CustomerFilterLevelID)
        {
            DataSet Countryds = new DataSet();
            var command = _datacontext.Database.GetDbConnection().CreateCommand();
            command.CommandText = "[dbo].[WEB_stpSearch_OriginCountryVGM_Lookup]";
            command.CommandType = CommandType.StoredProcedure;
            SqlParameter param1 = new SqlParameter("@Value", "");
            SqlParameter param2 = new SqlParameter("@CustomerID", customerID = 0);
            SqlParameter param3 = new SqlParameter("@UserID", UserID = 0);
            SqlParameter param4 = new SqlParameter("@CustomerFilterLevelID", CustomerFilterLevelID = 0);
            command.Parameters.Add(param1);
            command.Parameters.Add(param2);
            command.Parameters.Add(param3);
            command.Parameters.Add(param4);
            DbDataAdapter adapter = DbProviderFactories.GetFactory(_datacontext.Database.GetDbConnection()).CreateDataAdapter();
            adapter.SelectCommand = command;
            adapter.Fill(Countryds);
            if (Countryds.Tables[0].Rows.Count > 0)
            {
                Countryds.Tables[0].TableName = "country";
                Countryds.Tables[1].TableName = "country1";
            }
            return Countryds;
        }
        //converting to list to XML
        public string convertListToXML(List<VGMContainerDetails> list)
        {
            //< row val1 = "1" val2 = "123" val3 = "1" val4 = "" val5 = "12" val6 = "20 Feb 2019" />

            string xelement = string.Empty;
            foreach (var l in list)
            {
                xelement += new XElement("row",
             new XElement("val1", l.ContainerNo),
             new XElement("val2", l.SealNo),
             new XElement("val3", l.WeighingMethod),
             new XElement("val4", l.MarksAndNumbers),
            new XElement("val5", l.NoAndTypeofPackages),
            new XElement("val6", l.DescriptionOFGoods)).ToString();
            }
            return xelement;
        }
        //converting to base64string Byte signature
        public Byte[] convertstringToByte(string signature)
        {
            byte[] array = Encoding.ASCII.GetBytes(signature);
            return array;
        }
        //generating Guild
        public string CreateGUID()
        {
            System.Guid result;

            result = System.Guid.NewGuid();

            return result.ToString();
        }
        //Send Email
        public void SendEmail(string PdfDocAsBase64, string emailTo)
        {

            PdfDocAsBase64 = PdfDocAsBase64.Replace("data:application/pdf;base64,", String.Empty);
            bool sslEnabled = true;
            byte[] PdfDocAsBytes = Convert.FromBase64String(PdfDocAsBase64);
            if (emailTo != string.Empty)
            {
                string[] Emails = emailTo.Split(';');
                if (Emails.Count() > 0)
                {
                    foreach (var email in Emails)
                    {
                        MailMessage mm = new MailMessage("apula@agility.com", email);
                        mm.Subject = "Text";
                        mm.Body = "Hi";
                        mm.IsBodyHtml = true;
                        using (MemoryStream ms = new MemoryStream(PdfDocAsBytes))
                        {
                            mm.Attachments.Add(new Attachment(ms, "Shipment.pdf"));
                        }

                        SmtpClient smtpServer = new SmtpClient();
                        smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtpServer.EnableSsl = false;
                        smtpServer.Host = "relay-hosting.secureserver.net";//"smtp.gmail.com"
                        smtpServer.Port = 465;//587
                        smtpServer.Credentials = new System.Net.NetworkCredential("apula@agility.com", "*****", "LOGISTICS") as ICredentialsByHost;
                        if (sslEnabled)
                        {
                            smtpServer.EnableSsl = true;
                            ServicePointManager.ServerCertificateValidationCallback =
                            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                            { return true; };
                        }
                        smtpServer.Send(mm);
                    }
                }
            }



        }

    }
}
