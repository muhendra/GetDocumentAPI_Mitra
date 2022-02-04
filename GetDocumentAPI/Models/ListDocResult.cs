using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetDocumentAPI.Models
{
    public class ListDocResult
    {
        public List<string> DocType;
        public string errCode;
        public string errMsg;

        public ListDocResult()
        {
            errCode = "0";
            DocType = new List<string>();

        }
    }
}