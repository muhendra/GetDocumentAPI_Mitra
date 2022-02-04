using GetDocumentAPI.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GetDocumentAPI.Controllers
{
    public class GetListDocumentController : ApiController
    {

        public HttpResponseMessage Post([FromBody]param value)
        {
            Logger log = LogManager.GetCurrentClassLogger();
            ListDocResult res = new ListDocResult();

            try
            {
                log.Info($"[REQ] Request Doc list for Agreement No {value.AgreementNo}");
                string connString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connString))
                using (SqlCommand cmd = new SqlCommand("dbo.SPGetListDoc", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // set up the parameters
                    cmd.Parameters.Add("@AgreementNo", SqlDbType.VarChar, 50);
                    cmd.Parameters["@AgreementNo"].Value = value.AgreementNo;
      

                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            res.DocType.Add(reader["DocType"].ToString());

                        }
                    }
                    else
                    {
                   
                        
                     
                        res.errMsg = "No kontrak tidak terdaftar";

                        log.Error(res.errMsg);

                    }



                    conn.Close();

                    log.Info($"[SCS] Request Doc list for Agreement No : {value.AgreementNo}");

                }
            }
            catch (Exception ex)
            {

                res.errCode = "1";
                res.errMsg  = ex.Message;
                log.Error(ex);
            }

            return Request.CreateResponse(HttpStatusCode.Created, res);

        }
    }
}
