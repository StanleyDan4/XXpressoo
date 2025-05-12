using System;
using System.Collections.Generic;
using System.Text;

namespace XPressoo.Models.Dtos
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
