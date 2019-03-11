using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrackingAPI.Models
{
    public class EVGMDetailsModel
    {
        [Key]
        public string ConsignmentID { get; set; }
        public int JobModeID { get; set; }
        public int JobID { get; set; }
        public string VGMDeclarationFor { get; set; }
        public string BookingNo { get; set; }

        public string CountryCode { get; set; }
        public string Country { get; set; }
        public string UOM { get; set; }
        public int TotalContainers { get; set; }
        public string Name { get; set; }
        public string CompanyEmail { get; set; }
        public string Telephone { get; set; }
        public string ShippersCompanyName { get; set; }
        public string ShippersAddress { get; set; }
        public Boolean Chk1 { get; set; }
        public string Signature { get; set; }
        public string ShipmentPdf { get; set; }
        public string EmailTo { get; set; }
        public List<VGMContainerDetails> VGMContainerDetails { get; set; }

    }
    public class VGMContainerDetails
    {
        [Key]
        public string ContainerNo { get; set; }
        public string SealNo { get; set; }
        public string WeighingMethod { get; set; }
        public string MarksAndNumbers { get; set; }
        public string NoAndTypeofPackages { get; set; }
        public string DescriptionOFGoods { get; set; }
        public string VGM { get; set; }
        public string DateOFWeighing { get; set; }

    }
    public class country
    {
        [Key]
        public string CountryCode { get; set; }
        public string Country { get; set; }

    }
}

