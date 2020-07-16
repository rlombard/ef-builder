using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Noot.EntityFramework.Models;

namespace Newtonsoft.EntityFramework
{
    public class Builder
    {
        private string _ConnectionString;

        public Builder(string Server, string Database, string User, string Password)
        {
            var connectionBuilder = new SqlConnectionStringBuilder()
            {
                UserID = User,
                Password = Password,
                InitialCatalog = Database,
                DataSource = Server,
                ConnectTimeout = 30
            };

            _ConnectionString = connectionBuilder.ConnectionString;
        }

        public List<TableDefinition> GetTableDefinitions()
        {
            var result = new List<TableDefinition>();
            const string query = "select obj.Object_Id AS [ID]," +
                                 "obj.Name as [Name], " + 
                                 "sch.Name as [Schema] " + 
                                 "from sys.objects obj " +
                                 "inner join sys.Schemas sch " +
                                 "on sch.schema_id = obj.schema_id " +
                                 "where obj.type = 'U' " + 
                                 "order by obj.Name ";

            using (var connection = new SqlConnection(_ConnectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(TableDefinition.Construct(reader));
                    }
                }

                return result;
            }                    
        }

        public List<ColumnDefinition> GetColumns(int TableID)
        {
            var result = new List<ColumnDefinition>();
            const string query = "select " +
                                 " c.name as [Name], " +
                                 " t.Name as [DataType], " +
                                 " c.max_length as [MaxLength], " + 
                                 " c.precision as [Precision],  " +
                                 " c.scale as [Scale], " +
                                 " c.is_nullable as [IsNullable], " +
                                 " ISNULL(i.is_primary_key, 0) as [IsPrimary], " +
                                 " c.is_identity as [IsIdentity]," +
                                 " OBJECT_NAME (f.referenced_object_id) as [ForeignTable]" +
                                 " from sys.columns c " +
                                 " inner join sys.types t " +
                                 " on c.user_type_id = t.user_type_id " +
                                 " left outer join sys.index_columns ic " +
                                 " on ic.object_id = c.object_id and ic.column_id = c.column_id " +
                                 " left outer join sys.indexes i " +
                                 " on ic.object_id = i.object_id and ic.index_id = i.index_id " +
                                 " left outer join sys.foreign_key_columns f " +
                                 " on c.object_id = f.parent_object_id and c.column_id = f.parent_column_id " +
                                 " where  c.object_id = @TableID " +
                                 " order by c.Column_id ";

            using (var connection = new SqlConnection(_ConnectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(ColumnDefinition.Construct(reader));
                    }
                }

                return result;
            }           
        }
    }
}
