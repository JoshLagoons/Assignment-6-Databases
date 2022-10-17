using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment6_DD
{
    class Error
    {
        public Error(string msg, string info)
        {
            Message = msg;
            Info = info;
        }
        public string Message { get; set; }
        public string Info { get; set; }
    }
}
