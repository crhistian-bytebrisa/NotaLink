using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotaLink.Application.DTOs.API
{
    public class ErrorResponse
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public List<ErrorItem> Errors { get; set; }
        public string TraceId { get; set; }

    }

    public class ErrorItem
    {
        public string Property { get; set; }
        public List<string> Errores { get; set; }
    }
}
