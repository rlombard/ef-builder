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
            var result = new ColumnDefinition()
            {
                Name = Reader.GetString(Reader.GetOrdinal(nameof(ColumnDefinition.Name))),
                DataType = Reader.GetString(Reader.GetOrdinal(nameof(ColumnDefinition.DataType))),
                ForeignTable = Reader.IsDBNull(Reader.GetOrdinal(nameof(ColumnDefinition.ForeignTable))) 
                            ? string.Empty 
                            : Reader.GetString(Reader.GetOrdinal(nameof(ColumnDefinition.ForeignTable))),
                IsIdentity = Reader.GetBoolean(Reader.GetOrdinal(nameof(ColumnDefinition.IsIdentity))),
                IsPrimary = Reader.GetBoolean(Reader.GetOrdinal(nameof(ColumnDefinition.IsPrimary))),
                IsNullable = Reader.GetBoolean(Reader.GetOrdinal(nameof(ColumnDefinition.IsNullable))),
                MaxLength = Reader.GetInt32(Reader.GetOrdinal(nameof(ColumnDefinition.MaxLength))),
                Precision = Reader.GetInt32(Reader.GetOrdinal(nameof(ColumnDefinition.Precision))),
                Scale = Reader.GetInt32(Reader.GetOrdinal(nameof(ColumnDefinition.Scale))),
            };
            return result;
        }
    }
}
