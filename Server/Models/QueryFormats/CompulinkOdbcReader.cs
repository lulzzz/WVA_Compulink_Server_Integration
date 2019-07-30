using WVA_Connect_CSI.Errors;
using WVA_Connect_CSI.Models.Parameters.Derived;
using WVA_Connect_CSI.Models.QueryFormats;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Models.Prescriptions
{
    public class CompulinkOdbcReader
    {
        public List<Prescription> GetOpenOrders(string[] WhereObjects)
        {
            List<Prescription> listPrescriptions = new List<Prescription>();

            using (OdbcConnection conn = new OdbcConnection())
            {
                conn.ConnectionString = $"dsn={Startup.config.Dsn}";
                conn.Open();

                var comm = new OdbcCommand(PrintQuery(WhereObjects), conn);
                OdbcDataReader dr = comm.ExecuteReader();

                try
                {
                    while (dr.Read())
                    {
                        listPrescriptions.Add(new Prescription()
                        {
                            _CustomerID = new CustomerID() { Value = dr.GetValue(0).ToString() },
                            FirstName = dr.GetValue(25).ToString(),
                            LastName = dr.GetValue(26).ToString(),
                            Eye = "Right",
                            LensRx = dr.GetValue(30).ToString(),
                            IsShipToPat = dr.GetValue(31).ToString() == "True" ? true : false,
                            IsTrial = dr.GetValue(32).ToString() == "True" ? true : false,
                            Quantity = dr.GetValue(1).ToString(),
                            Product = dr.GetValue(2).ToString() + $" {dr.GetValue(33).ToString()}",
                            Vendor = dr.GetValue(3).ToString(),
                            BaseCurve = dr.GetValue(4).ToString(),
                            Diameter = dr.GetValue(5).ToString(),
                            Sphere = dr.GetValue(6).ToString(),
                            Cylinder = dr.GetValue(7).ToString(),
                            Axis = dr.GetValue(8).ToString(),
                            Add = dr.GetValue(9).ToString(),
                            Color = dr.GetValue(10).ToString(),
                            Multifocal = dr.GetValue(11).ToString(),
                            Date = $"{dr.GetValue(27).ToString()}-{dr.GetValue(28).ToString()}-{dr.GetValue(29).ToString()}"
                        });

                        listPrescriptions.Add(new Prescription()
                        {
                            _CustomerID = new CustomerID() { Value = dr.GetValue(0).ToString() },
                            FirstName = dr.GetValue(25).ToString(),
                            LastName = dr.GetValue(26).ToString(),
                            Eye = "Left",
                            LensRx = dr.GetValue(30).ToString(),
                            IsShipToPat = dr.GetValue(31).ToString() == "True" ? true : false,
                            IsTrial = dr.GetValue(32).ToString() == "True" ? true : false,
                            Quantity = dr.GetValue(12).ToString(),
                            Product = dr.GetValue(13).ToString() + $" {dr.GetValue(34).ToString()}",
                            Vendor = dr.GetValue(14).ToString(),
                            BaseCurve = dr.GetValue(15).ToString(),
                            Diameter = dr.GetValue(16).ToString(),
                            Sphere = dr.GetValue(17).ToString(),
                            Cylinder = dr.GetValue(18).ToString(),
                            Axis = dr.GetValue(19).ToString(),
                            Add = dr.GetValue(20).ToString(),
                            Color = dr.GetValue(21).ToString(),
                            Multifocal = dr.GetValue(22).ToString(),
                            Date = $"{dr.GetValue(27).ToString()}-{dr.GetValue(28).ToString()}-{dr.GetValue(29).ToString()}"
                        });
                    }
                }
                catch (Exception x)
                {
                    Error.ReportOrLog(x);
                    return null;
                }
                finally
                {
                    conn.Close();
                }

                return listPrescriptions;
            }
        }

        private string PrintQuery(string[] WhereObjects)
        {
            string[] select_objects = {
                "lrx.patunique",
                "lrx.r_boxes",
                "lrx.r_name",
                "lrx.r_mfg",
                "lrx.r_bc",
                "lrx.r_dia",
                "lrx.r_power",
                "lrx.r_cyl",
                "lrx.r_axis",
                "lrx.r_add",
                "lrx.r_color",
                "lrx.r_prism",
                "lrx.l_boxes",
                "lrx.l_name",
                "lrx.l_mfg",
                "lrx.l_bc",
                "lrx.l_dia",
                "lrx.l_power",
                "lrx.l_cyl",
                "lrx.l_axis",
                "lrx.l_add",
                "lrx.l_color",
                "lrx.l_prism",
                $"lrx.{Startup.config?.WvaInvoiceColumn}", // laborder
                $"lrx.{Startup.config?.LabSentColumn}", // labsent
                "p.first",
                "p.last",
                "EXTRACT(YEAR FROM lrx.date)",
                "EXTRACT(MONTH FROM lrx.date)",
                "EXTRACT(DAY FROM lrx.date)",
                "lrx.lensunique",
                "lrx.shiptopat",
                "lrx.trial",
                "lrx.r_type",
                "lrx.l_type"
            };
            string table = "lens_rx lrx";
            string[] joins = { "patient p on p.patunique = lrx.patunique" };
            string[] where_objects = WhereObjects;
            Query Product_query = new Query(select_objects, table, joins, where_objects);

            return Product_query.Assembled;
        }
    }
}
