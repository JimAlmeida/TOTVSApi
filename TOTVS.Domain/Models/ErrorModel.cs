using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TOTVS.Domain
{
    public class ErrorModel
    {
        public int ErrorStatusCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}