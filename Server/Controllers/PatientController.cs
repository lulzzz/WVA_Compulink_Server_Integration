using WVA_Connect_CSI.Models.Patients;
using WVA_Connect_CSI.Models.QueryFormats;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Odbc;

namespace WVA_Connect_CSI.Controllers
{
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly static string DSN = $"{Startup.config.Dsn}";

        [HttpGet]
        public List<Patient> GetAll()
        {
            return GetPatients(new string[] { });
        }

        [HttpGet("{patunique}", Name = "GetPatient")]
        public Patient GetByPatunique(long patunique)
        {
            return GetPatients(new string[] { $"patunique = {patunique}" })[0];
        }

        public static List<Patient> GetPatients(string[] WhereObjects)
        {
            List<Patient> Patients = new List<Patient>();

            using (OdbcConnection conn = new OdbcConnection())
            {
                try
                {
                    conn.ConnectionString = $"dsn={DSN}";
                    conn.Open();

                    OdbcCommand command = new OdbcCommand(PrintQuery(WhereObjects), conn);
                    OdbcDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        Patients.Add(new Patient
                        {
                            PatientID = dr.GetValue(0).ToString(),
                            FirstName = dr.GetValue(1).ToString(),
                            LastName = dr.GetValue(2).ToString(),
                            Street = dr.GetValue(3).ToString(),
                            City = dr.GetValue(4).ToString(),
                            State = dr.GetValue(5).ToString(),
                            Zip = dr.GetValue(6).ToString(),
                            Phone = dr.GetValue(7).ToString(),
                            //DoB         = dr.GetValue(8).ToString()
                        });
                    }

                    return Patients;
                }
                catch
                {
                    return null;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public static string PrintQuery(string[] WhereObjects)
        {
            string[] select_objects = { "p.patunique", "p.last", "p.first", "b.street", "b.city", "b.state", "b.zip", "b.hphone" };
            string table = "Patient p";
            string[] joins = { "bill b ON p.billunique = b.billunique" };

            return new Query(select_objects, table, joins, WhereObjects).Assembled;
        }
    }
}
