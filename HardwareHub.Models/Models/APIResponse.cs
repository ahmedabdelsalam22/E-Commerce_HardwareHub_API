using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HardwareHub.Models.Models
{
    public class APIResponse
    {
        public bool IsSuccess { get; set; }
        public List<string> Message { get; set; }
        public object? Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
