using System;
using System.Data.SqlClient;

namespace Noot.EntityFramework.Models
{
    public class DefaultColumnValueDefinition
    {
        public string ColumnName {get;set;}
        public string Value {get;set;}

        public static DefaultColumnValueDefinition Construct(SqlDataReader Reader)
        {
            if (Reader == null) throw new ArgumentNullException(nameof(Reader));
            var result = new DefaultColumnValueDefinition();
            
            result.ColumnName = Reader.GetString(Reader.GetOrdinal("COLUMN_NAME"));
            result.Value = Reader.GetString(Reader.GetOrdinal("COLUMN_DEFAULT")); 
            
            return result;
        }
    }
}