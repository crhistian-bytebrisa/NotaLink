using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace NotaLink.Application.DTOs.API
{ 
    public class ErrorResponse
    {
        public string Type { get; set; } = "https://tools.ietf.org/html/rfc9110#section-15.5.1" ;

        public string Title { get; set; } = "One or more validation errors occurred.";

        public int Status { get; set; }

        public Dictionary<string, string[]> Errors { get; set; } = [];

        public string TraceId { get; set; } = string.Empty;
    }
}
