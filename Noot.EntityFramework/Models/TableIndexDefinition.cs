using System;
using System.Data.SqlClient;

namespace Noot.EntityFramework.Models
{
    public class TableIndexDefinition
    {
        public string Name {get;set;}
        public string Columns {get;set;}
        public bool IsUnique {get;set;}

         public static TableIndexDefinition Construct(SqlDataReader Reader)
        {
            if (Reader == null) throw new ArgumentNullException(nameof(Reader));
            var result = new TableIndexDefinition();            
            result.Name = Reader.GetString(Reader.GetOrdinal("index_name"));
            result.Columns = Reader.GetString(Reader.GetOrdinal("columns"));
            result.IsUnique = Reader.GetBoolean(Reader.GetOrdinal("unique"));
            
            return result;
        }
    }
}