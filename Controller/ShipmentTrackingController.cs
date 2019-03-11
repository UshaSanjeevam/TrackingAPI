using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrackingAPI.Repository;
using TrackingAPI.Models;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Text;
using TrackingAPI.IRepository;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrackingAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentTrackingController : ControllerBase
    {
        public IShipmentTracking _trackingRepository;

        public ShipmentTrackingController(IShipmentTracking trackingRepository)
        {
            _trackingRepository = trackingRepository;
        }


        // GET api/values/5
        [HttpGet]
        public async Task<object> GetCustomerShippmentDetails(string consignmentID)
        {
            return await _trackingRepository.GetCustomerShippmentDetails(consignmentID);
        }
        [Route("GetEventDetails")]
        [HttpGet]
        public async Task<DataSet> GetEventDetails(Int32 JobID, Int32 customerID)
        {
            return await _trackingRepository.GetEventDetailsAsync(JobID, customerID);
        }
        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]EVGMDetailsModel value)
        {
            var status = _trackingRepository.InsertVGMDetails(value);
            if (status > 0)
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK);
            }
            else
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError);
            }

        }


        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        [Route("GetCountry")]
        [HttpGet]

        public DataSet GetCountry(string value, Int16 customerID, Int16 UserID, Int16 CustomerFilterLevelID)
        {
            return _trackingRepository.GetCountrys(value, customerID, UserID, CustomerFilterLevelID);
        }
    }
}
