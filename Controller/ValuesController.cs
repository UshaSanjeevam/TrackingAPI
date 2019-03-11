using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrackingAPI.Exceptions;
using TrackingAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrackingAPI.Controller
{
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        public ILoggerManager _loggermnager;
        public ValuesController(ILoggerManager loggermnager)
        {
            _loggermnager = loggermnager;
        }
        // GET: api/values
        [HttpGet]
        public string Get()
        {
            //int i = 0;
            //var j = 1 / i;
            //return new ObjectResult(j);
            //IList<Employee> employeeList = new List<Employee>(){
            //    new Employee() { Id = 10, Name = "Chris", City = "London" },
            //    new Employee() { Id=11, Name="Robert", City="London"},
            //    new Employee() { Id=12, Name="Mahesh", City="India"},
            //    new Employee() { Id=13, Name="Peter", City="US"},
            //    new Employee() { Id=14, Name="Chris", City="US"}
            //};
            //var result3 = employeeList.Single(e => e.Name == "Chris");
            //return result3;
            return "hgjgjdf";

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
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
    }
}
