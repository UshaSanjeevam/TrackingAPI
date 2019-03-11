using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrackingAPI.Models
{
    public class EShipmentTracking
    {
        [Key]
        public Int32 JobID { get; set; }
        public string ConsignmentID { get; set; }
        public DateTime Created { get; set; }
        public int JobStatusID { get; set; }
        public int JobModeID { get; set; }
        public string ShipperReferences { get; set; }
        public string ConsigneeReferences { get; set; }
        public int Pieces { get; set; }

        public Double ActualWeight { get; set; }
        public Double CubicVolume { get; set; }
        public Double ActualWeightLb { get; set; }
        public Double CubicVolumeFt3 { get; set; }
        public string WeightsBaseUnit { get; set; }
        public Double TaxWeight { get; set; }
        public Double TaxWeightLb { get; set; }
        public string OtherShipmentDetails { get; set; }
        public string POD { get; set; }
        public string MasterBillReference { get; set; }
        public string HouseBillReference { get; set; }
        public string Routing { get; set; }
        public string OriginatingFlightOrVessel { get; set; }
        public string ConnectingFlightOrVessel { get; set; }
        public string ArrivingFlightOrVessel { get; set; }
        public string CargoDescription { get; set; }
        public string DocsReceived { get; set; }
        public string Docsok { get; set; }
        public Boolean ExternallyControlled { get; set; }
        public string Shipper { get; set; }
        public string Consignee { get; set; }
        public Int64 ShipperID { get; set; }
        public string ShipperClientCode { get; set; }
        public Int64 ConsigneeID { get; set; }
        public string ConsigneeClientCode { get; set; }
        public Boolean? CustomerEventText { get; set; }
        public Boolean? CustomerReasonCodes { get; set; }

        public string JobMode { get; set; }
        public string JobModeSystemName { get; set; }
        public string JobStatus { get; set; }
        public string JobStatusSystemName { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string OriginCity { get; set; }
        public string DestinationCity { get; set; }
        public string OriginCityCode { get; set; }
        public string DestinationCityCode { get; set; }
        public Int32 OriginCityID { get; set; }
        public Int32 DestinationCityID { get; set; }
        public Int32 OriginCountryID { get; set; }
        public Int32 DestinationCountryID { get; set; }
        public string OriginCountryCode { get; set; }
        public string DestinationCountryCode { get; set; }
        public DateTime JobBookingDate { get; set; }
        public string ServiceLevel { get; set; }
        public string STLEventSystemName { get; set; }
        public Int32 STLEventOrder { get; set; }

        public Boolean STLEventIsCoreEvent { get; set; }
        public DateTime STLEventDateLocal { get; set; }
        public string STLEventName { get; set; }
        public string NextSTLEventName { get; set; }
        public DateTime NextSTLEventDateLocal { get; set; }
        public string SourceSystem { get; set; }


    }
}
