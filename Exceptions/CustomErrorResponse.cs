using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackingAPI.Exceptions
{
    public class CustomErrorResponse
    {
           public string Source { get; set; }
          public string LogTime { get; set; }
           public string Message { get; set; }
            public string StackTrace { get; set; }
            public int StatusCode { get; set; }
            public string ControllerName { get; set; }
            public string Method { get; set; }
         
         

    }
}
