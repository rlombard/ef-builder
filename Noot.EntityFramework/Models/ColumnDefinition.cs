using System;
using System.Data.SqlClient;

namespace Noot.EntityFramework.Models
{
    public class ColumnDefinition
    {
        public string Name {get;set;}
        public string DataType {get;set;}
        public int MaxLength {get;set;}
        public int Precision {get;set;}
        public int Scale {get;set;}
        public bool IsPrimary {get;set;}
        public bool IsIdentity {get;set;}
        public bool IsNullable {get;set;}
        public string ForeignTable {get;set;}

        public static ColumnDefinition Construct(SqlDataReader Reader)
        {
            if (Reader == null) throw new ArgumentNullException(nameof(Reader));
            var result = new ColumnDefinition();
            
                result.Name = Reader.GetString(Reader.GetOrdinal(nameof(ColumnDefinition.Name)));
                result.DataType = Reader.GetString(Reader.GetOrdinal(nameof(ColumnDefinition.DataType)));
                
                result.IsIdentity = Reader.GetBoolean(Reader.GetOrdinal(nameof(ColumnDefinition.IsIdentity)));
                result.IsPrimary = Reader.GetBoolean(Reader.GetOrdinal(nameof(ColumnDefinition.IsPrimary)));
                result.IsNullable = Reader.GetBoolean(Reader.GetOrdinal(nameof(ColumnDefinition.IsNullable)));
                result.MaxLength = Reader.GetInt16(Reader.GetOrdinal(nameof(ColumnDefinition.MaxLength)));
                result.Precision = Reader.GetByte(Reader.GetOrdinal(nameof(ColumnDefinition.Precision)));
                result.Scale = Reader.GetByte(Reader.GetOrdinal(nameof(ColumnDefinition.Scale)));
            
            return result;
        }
    }
}
