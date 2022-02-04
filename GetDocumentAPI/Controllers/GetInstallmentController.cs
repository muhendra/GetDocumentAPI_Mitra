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
    public class GetInstallmentController : ApiController
    {
        public HttpResponseMessage Post([FromBody]param value)
        {
            Logger log = LogManager.GetCurrentClassLogger();
            List<InstallmentResult> res = new List<InstallmentResult>();

            try
            {
                log.Info($"[REQ] Request installment schedule for Agreement No {value.AgreementNo}");
                string connString = ConfigurationManager.ConnectionStrings["SqlConnectionStringSMILE"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connString))
                using (SqlCommand cmd = new SqlCommand("dbo.SP_MOBILE_GET_INSTALLMENT", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // set up the parameters
                    cmd.Parameters.Add("@contract_no", SqlDbType.VarChar, 50);
                    cmd.Parameters["@contract_no"].Value = value.AgreementNo;
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            InstallmentResult x = new InstallmentResult();
                            x.LSAGREE = reader["LSAGREE"].ToString();
                            x.PERIOD  = reader["PERIOD"].ToString();
                            x.PRINCIPAL = reader["PRINCIPAL"].ToString();
                            x.INTEREST  = reader["INTEREST"].ToString();
                            x.LS_RECEIVE  = reader["LS_RECEIVE"].ToString();
                            x.RENTAL  = reader["RENTAL"].ToString();
                            x.DUEDATE  = reader["DUEDATE"].ToString();
                            x.OSPRINCIP = reader["OSPRINCIP"].ToString();
                            x.PENALTY  = reader["PENALTY"].ToString();
                            x.PAYMENT  = reader["PAYMENT"].ToString();
                            x.PAYDATE  = reader["PAYDATE"].ToString();
                            x.RESTRU  = reader["RESTRU"].ToString();
                            x.OVERDUE  = reader["OVERDUE"].ToString();

                            res.Add(x);



                        }
                    }
                    conn.Close();

                    log.Info($"[SCS] Request installment schedule for Agreement No : {value.AgreementNo}");

                }
            }
            catch (Exception ex)
            {

        
                log.Error(ex);
            }

            return Request.CreateResponse(HttpStatusCode.Created, res);


        }
    }
}
