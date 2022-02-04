using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetDocumentAPI.Models
{
    public class ApiResult
    {
        public string AgreementNo;
        public string DocyTpe;
        public byte[] File;
        public string ErrMsg;
        public string ErrCode;
        public string FileExt;

        public ApiResult()
        {
            ErrCode = "0";
            ErrMsg = "";
        }

    }
}