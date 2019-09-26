using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Models.Configurations
{
    public class WvaConfig
    {
        /* DSN is the name of the System DSN defined to use the Advantage ODBC
         * Driver to communicate with Compulink.
         * DEFAULT VALUE: "WVA-CSI"
         */
        public string Dsn { get; set; }

        /* ApiKey is the assigned preshared key (a UUID) used to authenticate
         * for the WVA Order API.
         * DEFAULT VALUE: test api key
         */
        public string ApiKey { get; set; }

        /* The Location Dictionary is a key value pair that links a location 
         * (WVA Account Number) to a an identifier in Compulink's lens_rx 
         * table through the LabColumn 
         */
        public Dictionary<string, string> Location { get; set; }

        /* WvaInvoiceColumn defines the column in Compulink's lens_rx table
         * that is used to indicate the WVA Invoice number or order number.
         * If this column's value is not null, the server assumes the order
         * is no longer open.
         * This column is also where we write back the WVA Order Number on 
         * successful submission to the order API.
         */
        public string WvaInvoiceColumn { get; set; }

        /* LabSentColumn defines the column that indicates the date a lens_rx
         * order was submitted for processing.
         * The date in this column is updated on successful submission to the
         * order API.
         * DEFAULT VALUE: "laborder"
         * Note: some installations have this as "ZZORDNO"
         */
        public string LabSentColumn { get; set; }


        /* LabColumn defines the column used to identify the open orders for
         * the current user's location, as defined in the Location dictionary.
         * Example: the entry in Location could be "44": "WVA-001" and a query
         * to the OpenOrder endpoint (api/openorder/44) would use the column
         * defined here in the SQL where clause 
         * (for example: WHERE lab = 'WVA-001')
         * DEFAULT VALUE: "lab"
         */
        public string LabColumn { get; set; }


        /* FilterColumn and FilterValue are a pair that define the scope to 
         * examine for open orders.
         * DEFAULT VALUE: date
         * Note: using `lensunique` allows for more granular control of the scope
         */
        public string FilterColumn { get; set; }


        /* FilterValue is the maximum value to exclude from the open order scope
         * DEFAULT VALUE: today's date
         * Note: a null or empty value in this config variable should prompt 
         * the app to retrieve the current maximum value in FilterColumn and 
         * write it to the config file for persistence.
         */
        public string FilterValue { get; set; }

        public bool OverRideDefaultQueries { get; set; } = false;
    }
}