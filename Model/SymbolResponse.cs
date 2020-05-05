using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class SymbolResponse
    {
        public bool Valid { get; set; }
        public string errorResponse { get; set; }
        public List<string> inValidSymbols { get; set; }
    }
}
