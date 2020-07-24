using System.Linq;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;

namespace Noot.EntityFramework.Models
{
    public class TableDefinition
    {
        public int ID {get;set;}
        public string Name {get;set;}
        public string Schema {get;set;}
        public string NameBySchema => $"{Schema}.{Name}";
        public List<ColumnDefinition> Columns {get; private set;}
        public List<TableIndexDefinition> TableIndices {get; private set;}
        public List<ForeignKeyDefinition> ForeignKeys {get;private set;}
         public List<DefaultColumnValueDefinition> DefaultColumns {get;private set;}
        private string _ConnectionString;
        public bool HasKey {get;set;} = false;

        public TableDefinition()
        {
            Columns = new List<ColumnDefinition>();
            TableIndices = new List<TableIndexDefinition>();
            ForeignKeys = new List<ForeignKeyDefinition>();
            DefaultColumns = new List<DefaultColumnValueDefinition>();
        }

         public TableDefinition(string ConnectionString)
        {
            _ConnectionString = ConnectionString;
            Columns = new List<ColumnDefinition>();
            TableIndices = new List<TableIndexDefinition>();
            ForeignKeys = new List<ForeignKeyDefinition>(); 
            DefaultColumns = new List<DefaultColumnValueDefinition>();

            _Set();
        }

        public static TableDefinition Construct(string ConnectionString, SqlDataReader Reader)
        {
            if (Reader == null) throw new ArgumentNullException(nameof(Reader));
            var result = new TableDefinition()
            {
                ID = Reader.GetInt32(Reader.GetOrdinal(nameof(TableDefinition.ID))),
                Name = Reader.GetString(Reader.GetOrdinal(nameof(TableDefinition.Name))),
                Schema = Reader.GetString(Reader.GetOrdinal(nameof(TableDefinition.Schema))),
            };
            result.Set(ConnectionString);
            return result;
        }

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
        
        public void Set(string ConnectionString)
        {
            _ConnectionString = ConnectionString;
            _Set();
        }

        private void _Set()
        {
            if(string.IsNullOrEmpty(_ConnectionString)) throw new Exception("Connection string not set");

            _SetColumns();
            _SetPrimaryKey();
            _SetForeignKeys();
            _SetIndices();
        }

        private void _SetColumns()
        {
            const string query = " select c.Column_id, " +
                                 " c.name as [Name], " +
                                 " t.Name as [DataType], " +
                                 " c.max_length as [MaxLength], " + 
                                 " c.precision as [Precision],  " +
                                 " c.scale as [Scale], " +
                                 " c.is_nullable as [IsNullable] " +
                                 " from sys.columns c " + 
                                 " inner join sys.types t " +
                                 " on c.user_type_id = t.user_type_id " +
                                 " where c.object_id = @TableID " +
                                 " order by c.Column_id ";

            using (var connection = new SqlConnection(_ConnectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@TableID", this.ID);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Columns.Add(ColumnDefinition.Construct(reader));
                    }
                }                
            } 
        }

        private string _GetPrimaryKey()
        {
            var result = string.Empty;
            const string query = "SELECT i.name AS IndexName, OBJECT_NAME(ic.OBJECT_ID) AS TableName, " + 
                                 "COL_NAME(ic.OBJECT_ID,ic.column_id) AS ColumnName, ic.object_id " +
                                 " FROM sys.indexes AS i " + 
                                 " INNER JOIN sys.index_columns AS ic " +
                                 " ON i.OBJECT_ID = ic.OBJECT_ID " +
                                 " AND i.index_id = ic.index_id " +
                                 " WHERE i.is_primary_key = 1 " +
                                 " AND ic.object_id = @TableID";

            using (var connection = new SqlConnection(_ConnectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@TableID", ID);
                using (var reader = command.ExecuteReader())
                {
                    var count = 0;
                    while (reader.Read())
                    {
                        count++;
                        result = reader.GetString(reader.GetOrdinal("ColumnName"));
                    }

                    //if(count > 1) throw new Exception("More than one primary key was found");

                    return result;
                }  
            }          
        }

        private void _SetPrimaryKey()
        {
            var primaryKey = _GetPrimaryKey();
            if (string.IsNullOrEmpty(primaryKey)) 
            {
                HasKey = false;
                return;
            }

            var column = Columns.FirstOrDefault(x => x.Name.Equals(primaryKey));
            if(column == null) return;
            
            column.IsKey = true;     
            HasKey = true;       
        }

        private void _GetForeignKeys()
        {
            const string query = "select fk_tab.object_id, fk_tab.name as foreign_table, " +
                                "     pk_tab.name as primary_table, "+
                                "     substring(column_names, 1, len(column_names)-1) as [fk_columns], "+
                                "     fk.name as fk_constraint_name "+
                                " from sys.foreign_keys fk "+
                                "     inner join sys.tables fk_tab "+
                                "         on fk_tab.object_id = fk.parent_object_id "+
                                "     inner join sys.tables pk_tab "+
                                "         on pk_tab.object_id = fk.referenced_object_id "+
                                "     cross apply (select col.[name] + ', ' "+
                                "                     from sys.foreign_key_columns fk_c "+
                                "                         inner join sys.columns col "+
                                "                             on fk_c.parent_object_id = col.object_id "+
                                "                             and fk_c.parent_column_id = col.column_id "+
                                "                     where fk_c.parent_object_id = fk_tab.object_id "+
                                "                     and fk_c.constraint_object_id = fk.object_id "+
                                "                             order by col.column_id "+
                                "                             for xml path ('') ) D (column_names) "+
                                " where fk_tab.object_id = @TableID";

            using (var connection = new SqlConnection(_ConnectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@TableID", ID);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var primaryTable = reader.GetString(reader.GetOrdinal("primary_table"));
                        var column = reader.GetString(reader.GetOrdinal("fk_columns"));
                        var constraintName = reader.GetString(reader.GetOrdinal("fk_constraint_name"));
                        var foreignKey = new ForeignKeyDefinition()
                        {
                            HasOne = primaryTable,
                            WithMany = this.Name,
                            HasForeignKey = column,
                            HasConstraintName = constraintName
                        };
                        ForeignKeys.Add(foreignKey);
                    }
                }  
            }          
        }

        private void _SetForeignKeys()
        {
            _GetForeignKeys();
            foreach (var foreignKey in ForeignKeys)
            {
                var column = Columns.FirstOrDefault(x => x.Name.Equals(foreignKey.HasForeignKey));
                if(column == null) continue;
                
                column.HasForeignKey = true;  
                column.ForeignTable = foreignKey.HasOne;  
                column.ForeignKeyName = foreignKey.HasConstraintName;                
            }
        }

        private void _SetIndices()
        {
            const string query = " select i.[name] as index_name," +
                                "     substring(column_names, 1, len(column_names)-1) as [columns]," +
                                "     case when i.[type] = 1 then 'Clustered index'" +
                                "         when i.[type] = 2 then 'Nonclustered unique index'" +
                                "         when i.[type] = 3 then 'XML index'" +
                                "         when i.[type] = 4 then 'Spatial index'" +
                                "         when i.[type] = 5 then 'Clustered columnstore index'" +
                                "         when i.[type] = 6 then 'Nonclustered columnstore index'" +
                                "         when i.[type] = 7 then 'Nonclustered hash index'" +
                                "         end as index_type," +
                                "     i.is_unique as [unique]," +
                                "     schema_name(t.schema_id) + '.' + t.[name] as table_view, " +
                                "     case when t.[type] = 'U' then 'Table'" +
                                "         when t.[type] = 'V' then 'View'" +
                                "         end as [object_type]" +
                                " from sys.objects t" +
                                "     inner join sys.indexes i" +
                                "         on t.object_id = i.object_id" +
                                "     cross apply (select 'e.' + col.[name] + ', '" +
                                "                     from sys.index_columns ic" +
                                "                         inner join sys.columns col" +
                                "                             on ic.object_id = col.object_id" +
                                "                             and ic.column_id = col.column_id" +
                                "                     where ic.object_id = t.object_id" +
                                "                         and ic.index_id = i.index_id" +
                                "                             order by key_ordinal" +
                                "                             for xml path ('') ) D (column_names)" +
                                " where t.is_ms_shipped <> 1" +
                                " and index_id > 0" +
                                " and i.[name] not like 'PK_%'" +
                                " and t.object_id = @TableID" +
                                " order by i.[name]";

            using (var connection = new SqlConnection(_ConnectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@TableID", ID);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TableIndices.Add(TableIndexDefinition.Construct(reader));
                    }
                }  
            }          
        }

         private void _SetDefaults()
        {
            const string query = "SELECT COLUMN_NAME, COLUMN_DEFAULT FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = OBJECT_NAME(@TableID)";

            using (var connection = new SqlConnection(_ConnectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@TableID", ID);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DefaultColumns.Add(DefaultColumnValueDefinition.Construct(reader));
                    }
                }  
            }          
        }
    }
}
