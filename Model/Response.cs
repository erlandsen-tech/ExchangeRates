using System;
using System.Collections.Generic;

namespace Model
{
    public class Response
    {
        public bool Success { get; set; }
    }

    public class SuccessResponse : Response
    {
        public int Timestamp { get; set; }
        public string Base { get; set; }
        public string Date { get; set; }
        public Dictionary<string, string> Symbols { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }

    public class Error
    {
        public int Code { get; set; }
        public string Type { get; set; }
        public string Info { get; set; }
    }

    public class ErrorResponse : Response
    {
        public Error Error { get; set; }
    }
}