using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TrackingAPI.Models;

namespace TrackingAPI.IRepository
{
    public interface IShipmentTracking
    {
        Task<object> GetCustomerShippmentDetails(string consignmentID);
        Task<DataSet> GetEventDetailsAsync(Int32 JobID, Int32 customerID);
        int InsertVGMDetails(EVGMDetailsModel model);
        DataSet GetCountrys(string value, Int16 customerID, Int16 UserID, Int16 CustomerFilterLevelID);
        string convertListToXML(List<VGMContainerDetails> list);
        Byte[] convertstringToByte(string signature);
        void SendEmail(string PdfDocAsBase64, string emailTo);
    }
}
