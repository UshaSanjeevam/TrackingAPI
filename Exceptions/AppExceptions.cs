using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace TrackingAPI.Exceptions
{
    public class AppExceptions : Exception
    {
        private int _code;
        private string _description;

        public int Code
        {
            get => _code;
        }
        public string Description
        {
            get => _description;
        }

        public AppExceptions(string message, string description, int code) : base(message)
   {
            _code = code;
            _description = description;
        }
    }
}
