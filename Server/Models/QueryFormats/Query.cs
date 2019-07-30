using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Models.QueryFormats
{
    public class Query
    {
        public string[] SelectObjects { get; set; }
        public string Table { get; set; }
        public string[] Joins { get; set; }
        public string[] WhereObjects { get; set; }
        public string Assembled { get; set; }
        public string Assembler
        {
            set
            {
                StringBuilder assembled_query = new StringBuilder();
                // BUILD QUERY, GET DATA, SAVE THE WORLD
                assembled_query.Append("SELECT");
                string last_select = SelectObjects.Last();
                foreach (string s in SelectObjects)
                {
                    if (!s.ToUpper().Contains("UNIQUE") && !s.ToUpper().Contains("BOXES") && !s.ToUpper().Contains("LABSENT") && !s.ToUpper().Contains("LABORDER") && !s.ToUpper().Contains("DATE") && !s.ToUpper().Contains("TRIAL") && !s.ToUpper().Contains("SHIPTOPAT"))
                        assembled_query.Append($" RTRIM(COALESCE({s},''))");
                    else
                        assembled_query.Append($" {s} ");
                    if (!s.Equals(last_select))
                        assembled_query.Append($", ");
                }
                assembled_query.Append($" FROM {Table} ");
                if (Joins.Length > 0)
                {
                    foreach (string s in Joins)
                    {
                        assembled_query.Append($" JOIN {s} ");
                    }
                }
                string last_where = WhereObjects.Last();
                if (WhereObjects.Length > 0)
                {
                    assembled_query.Append(" WHERE ");
                    foreach (string s in WhereObjects)
                    {
                        assembled_query.Append($" {s} ");
                        if (!s.Equals(last_where))
                            assembled_query.Append("AND");
                    }
                }
                this.Assembled = assembled_query.ToString();
            }
            get
            {
                return this.Assembled;
            }
        }

        public Query(string[] SelectObjects, string Table, string[] Joins, string[] WhereObjects)
        {
            this.SelectObjects = SelectObjects;
            this.Table = Table;
            this.Joins = Joins;
            this.WhereObjects = WhereObjects;
            this.Assembler = "";
        }
    }
}