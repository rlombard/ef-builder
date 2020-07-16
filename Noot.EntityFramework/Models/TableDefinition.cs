using System;
using System.Data.SqlClient;

namespace Noot.EntityFramework.Models
{
    public class TableDefinition
    {
        public int ID {get;set;}
        public string Name {get;set;}
        public string Schema {get;set;}

        public static TableDefinition Construct(SqlDataReader Reader)
        {
            if (Reader == null) throw new ArgumentNullException(nameof(Reader));
            var result = new TableDefinition()
            {
                ID = Reader.GetInt32(Reader.GetOrdinal(nameof(TableDefinition.ID))),
                Name = Reader.GetString(Reader.GetOrdinal(nameof(TableDefinition.Name))),
                Schema = Reader.GetString(Reader.GetOrdinal(nameof(TableDefinition.Schema))),
            };
            return result;
        }
    }
}
