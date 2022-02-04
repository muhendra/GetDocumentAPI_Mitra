using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using GetDocumentAPI.Models;
using NLog;

namespace GetDocumentAPI.Controllers
{
    public class GetDocumentController : ApiController
    {


        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]param value)
        {
            Logger log = LogManager.GetCurrentClassLogger();
            ApiResult res = new ApiResult();

            try
            {
                log.Info($"[REQ] Request File for Agreement No {value.AgreementNo} for Document Type {value.DocType}");
                string connString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;


                if (value.DocType == "STARTERPACK")
                {

                    string starterpackfile = HttpContext.Current.Server.MapPath("~/spack.pdf");
                    res.AgreementNo = value.AgreementNo;
                    res.DocyTpe = "STARTERPACK";
                    res.File = System.IO.File.ReadAllBytes(starterpackfile);
                    res.FileExt = ".pdf";
                    log.Info($"[SCS] Request File success for Agreement No {value.AgreementNo} for Document Type {value.DocType}");
                }

                else
                {

                    using (SqlConnection conn = new SqlConnection(connString))
                    using (SqlCommand cmd = new SqlCommand("dbo.SPGetPolisInsurance", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // set up the parameters
                        cmd.Parameters.Add("@AgreementNo", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@DocType", SqlDbType.VarChar, 50);

                        cmd.Parameters["@AgreementNo"].Value = value.AgreementNo;
                        cmd.Parameters["@DocType"].Value = value.DocType;

                        conn.Open();

                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {

                                res.AgreementNo = reader["AgreeNo"].ToString();
                                res.DocyTpe = reader["SubType"].ToString();
                                res.File = (byte[])reader["FileDoc"];
                                res.FileExt = reader["ext"].ToString();


                            }
                        }
                        else
                        {
                            res.AgreementNo = "";
                            res.DocyTpe = "";
                            res.File = null;
                            res.ErrCode = "1";
                            res.ErrMsg = "No kontrak atau Doc Type Salah";

                            log.Error(res.ErrMsg);

                        }



                        conn.Close();

                        log.Info($"[SCS] Request File success for Agreement No {value.AgreementNo} for Document Type {value.DocType}");

                    }
                }
            }
            catch (Exception ex)
            {

                res.ErrCode = "1";
                res.ErrMsg = ex.Message;
                log.Error(ex);
            }

            return Request.CreateResponse(HttpStatusCode.Created, res);

        }
    

     
    }
}