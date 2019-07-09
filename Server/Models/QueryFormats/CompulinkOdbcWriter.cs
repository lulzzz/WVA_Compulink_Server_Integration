using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Compulink_Server_Integration.Models.Prescriptions
{
    public class CompulinkOdbcWriter
    {
        public void UpdateLensRx(List<string> listLensRxes, string wvaOrderID)
        {
            var lensrxLookupArg = String.Join(", ", listLensRxes);
            using (var conn = new OdbcConnection())
            {
                conn.ConnectionString = $"dsn={Startup.config.Dsn}";
                conn.Open();

                string lensrxUpdateSql = $"UPDATE lens_rx " +
                                           $"SET {Startup.config?.WvaInvoiceColumn} = CAST({wvaOrderID} AS SQL_CHAR), " +
                                           $"labsent = {{d '{DateTime.Today.ToString("d")}'}}, " +
                                           $"last_mod = {{d '{DateTime.Today.ToString("d")}'}} " +
                                           $"WHERE lensunique in ({lensrxLookupArg})";

                var comm = new OdbcCommand(lensrxUpdateSql, conn);
                comm.ExecuteReader();
                conn.Close();
            }
        }
    }
}
