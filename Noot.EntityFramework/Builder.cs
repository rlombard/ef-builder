using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Noot.EntityFramework.Models;
using Noot.EntityFramework.Extensions;
using Humanizer;

namespace Noot.EntityFramework
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

        public List<TableDefinition> GetTableDefinitions(string Schema = "")
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

                return string.IsNullOrEmpty(Schema) ? result : result.Where(x => x.Schema.ToLowerInvariant().Equals(Schema.ToLowerInvariant())).ToList();
            }                    
        }

        public TableDefinition GetTableDefinition(string TableName)
        {
            TableDefinition result = null;
            const string query = "select obj.Object_Id AS [ID]," +
                                 "obj.Name as [Name], " + 
                                 "sch.Name as [Schema] " + 
                                 "from sys.objects obj " +
                                 "inner join sys.Schemas sch " +
                                 "on sch.schema_id = obj.schema_id " +
                                 "where obj.type = 'U' and LOWER(obj.Name) = @Name" + 
                                 "order by obj.Name ";

            using (var connection = new SqlConnection(_ConnectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@Name", TableName.ToLowerInvariant());

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = TableDefinition.Construct(reader);
                    }
                }

                return result;
            }                    
        }

        public List<ColumnDefinition> GetColumns(TableDefinition Table)
        {
            var result = new List<ColumnDefinition>();
            const string query = "select * from ( " +            
                                 "select c.Column_id, " +
                                 " c.name as [Name], " +
                                 " t.Name as [DataType], " +
                                 " c.max_length as [MaxLength], " + 
                                 " c.precision as [Precision],  " +
                                 " c.scale as [Scale], " +
                                 " c.is_nullable as [IsNullable], " +
                                 " ISNULL(i.is_primary_key, 0) as [IsPrimary], " +
                                 " c.is_identity as [IsIdentity]," +
                                 //" f.constraint_column_id as [ForeignTable], " +
                                 //"  left outer join sys.foreign_key_columns f "+
                                // " on c.object_id = f.parent_object_id and c.column_id = f.parent_column_id " +
                                 " ROW_NUMBER() OVER (PARTITION BY c.Name ORDER BY c.Column_id) AS RowNumber " +
                                 " from sys.columns c " +
                                 " inner join sys.types t " +
                                 " on c.user_type_id = t.user_type_id " +
                                 " left outer join sys.index_columns ic " +
                                 " on ic.object_id = c.object_id and ic.column_id = c.column_id " +
                                 " left outer join sys.indexes i " +
                                 " on ic.object_id = i.object_id and ic.index_id = i.index_id " +                                
                                 " where  c.object_id = @TableID " +
                                 ") as a where a.RowNumber = 1 " +
                                 " order by a.Column_id " ;

            using (var connection = new SqlConnection(_ConnectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@TableID", Table.ID);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(ColumnDefinition.Construct(reader));
                    }
                }                
            }   
            
            GetForeignKeys(ref result, Table.Name, Table.Schema);
            result.AddRange(GetLocalKeys(Table.Name, Table.Schema));
            return result;        
        }

        public List<ColumnDefinition> GetForeignKeys(ref List<ColumnDefinition> result, string TableName, string Schema)
        {
            var query = $"EXEC sp_fkeys @fktable_name = '{TableName}', @fktable_owner = '{Schema}'" ;

            using (var connection = new SqlConnection(_ConnectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var column = new ColumnDefinition()
                        {
                            DataType = "relationship",
                            ForeignTable = reader.GetString(reader.GetOrdinal("PKTABLE_NAME")),
                            Name = reader.GetString(reader.GetOrdinal("FKCOLUMN_NAME"))
                        };

                        var item = result.FirstOrDefault(x => x.Name.Equals(column.Name));
                        if(item != null)
                        {
                            item.ForeignTable = column.ForeignTable;
                        }
                    }
                }

                return result;
            }  
        }

        public List<ColumnDefinition> GetLocalKeys(string TableName, string Schema)
        {
            var result = new List<ColumnDefinition>();
            var query = $"EXEC sp_fkeys @pktable_name = '{TableName}', @pktable_owner = '{Schema}'" ;

            using (var connection = new SqlConnection(_ConnectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var column = new ColumnDefinition()
                        {
                            DataType = "collection",
                            ForeignTable = reader.GetString(reader.GetOrdinal("FKTABLE_NAME")),
                            Name = reader.GetString(reader.GetOrdinal("PKCOLUMN_NAME"))
                        };

                        if(!result.Any(x => x.Name.Equals(column.Name))) result.Add(column);
                    }
                }

                return result;
            }  
        }

        public string CreateDatabaseContext(string Namespace, string ContextName, string ModelNamespace, string Schema = "")
        {
            var tables = GetTableDefinitions(Schema);

            var result = new StringBuilder();       
            result.AppendLine("using Microsoft.EntityFrameworkCore;");
            result.AppendLine($"using {ModelNamespace};");
            result.AppendLine(string.Empty);                
            result.AppendLine($"namespace {Namespace}");
            result.AppendLine(("{").Indent(0));
            result.AppendLine(($"public class {ContextName} : DbContext").Indent(1));
            result.AppendLine(("{").Indent(1));

            result.AppendLine(($"public {ContextName}() : base()").Indent(2));
            result.AppendLine(("{").Indent(2));
            result.AppendLine(("}").Indent(2));
            result.AppendLine(string.Empty); 

            result.AppendLine(($"public {ContextName}(DbContextOptions<{ContextName}> Options) : base(Options)").Indent(2));
            result.AppendLine(("{").Indent(2));
            result.AppendLine(("}").Indent(2));
            result.AppendLine(string.Empty); 

            result.AppendLine(("protected override void OnConfiguring(DbContextOptionsBuilder OptionsBuilder)").Indent(2));
            result.AppendLine(("{").Indent(2));
            result.AppendLine(($"OptionsBuilder.UseSqlServer(@\"{_ConnectionString}\");").Indent(3));
            result.AppendLine(("}").Indent(2));
            result.AppendLine(string.Empty); 


            foreach (var table in tables)
            {
                result.AppendLine(($"public DbSet<{table.Name}> {table.Name.Pluralize()} " + " { get; set; }").Indent(2));
            }
            result.AppendLine(("}").Indent(1));
            result.AppendLine(("}").Indent(0));
            return result.ToString();
        }

        public List<KeyValuePair<string,string>> CreateModels(string Namespace, string IdentificationTag = "ID", string BaseClass = "", string Schema = "")
        {
            var models = new List<KeyValuePair<string,string>>();
            var tables = GetTableDefinitions(Schema);

            foreach (var table in tables)
            {   
                var (model, file) = CreateModel(table, Namespace, IdentificationTag, BaseClass);
                models.Add(new KeyValuePair<string,string>(model, file));       
            }
          
            return models;          
        }
    
        public (string, string) CreateModel(string TableName, string Namespace, string IdentificationTag = "ID", string BaseClass = "")
        {
            var table = GetTableDefinition(TableName);
            if (table == null) throw new Exception("Table does not exist in database");

            return CreateModel(table, Namespace, IdentificationTag, BaseClass);   
        }

        public (string, string) CreateModel(TableDefinition Table, string Namespace, string IdentificationTag = "ID", string BaseClass = "")
        {
            var columns = GetColumns(Table);
            if(!columns.Any(x => x.IsPrimary)) return (string.Empty, string.Empty);
            var result = new StringBuilder();     

            if (columns.Any(x => !string.IsNullOrEmpty(x.ForeignTable)))
            {
                result.AppendLine("using System.Collections.Generic;");
            }  
            
            result.AppendLine("using System;");
            result.AppendLine("using System.ComponentModel.DataAnnotations;");
            result.AppendLine("using System.Diagnostics;");
            result.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");

            result.AppendLine(string.Empty);                
            result.AppendLine($"namespace {Namespace}");
            result.AppendLine(("{").Indent(0));
            
            var primaryColumn = columns.FirstOrDefault(x => x.IsPrimary);
            if (primaryColumn != null)
            {
                result.AppendLine(($"[DebuggerDisplay(\"{primaryColumn.Name} = " + "{" + primaryColumn.Name + "}\")]").Indent(1));
            }

            result.AppendLine(($"[Table(\"{Table.Name}\", Schema = \"{Table.Schema}\")]").Indent(1));

            if (string.IsNullOrEmpty(BaseClass))
            {
                result.AppendLine(($"public class {Table.Name}").Indent(1));
            }
            else
            {
                result.AppendLine(($"public class {Table.Name} : {BaseClass}").Indent(1));
            }

            
            result.AppendLine(("{").Indent(1));
            
            
            foreach (var column in columns)
            {
               
                var dataType = column.DataType.DataType(column.IsNullable);
                if (!string.IsNullOrEmpty(dataType) && !dataType.Equals("relationship") && !dataType.Equals("collection"))
                {
                    var key = column.Name.DecorateKey(column.IsPrimary);
                    var required = column.Name.DecorateRequired(column.IsNullable, column.IsPrimary, column.IsIdentity);
                    var range = column.Name.DecorateRange(column.DataType);
                    var length = column.Name.DecorateLength(column.MaxLength);

                    if (!string.IsNullOrEmpty(key)) result.AppendLine(key.Indent(2));
                    if (!string.IsNullOrEmpty(required)) result.AppendLine(required.Indent(2));
                    if (!string.IsNullOrEmpty(range)) result.AppendLine(range.Indent(2));
                    if (!string.IsNullOrEmpty(length)) result.AppendLine(length.Indent(2));
                
                    result.AppendLine(($"public {dataType} {column.Name} " + " { get; set; }").Indent(2));
                    result.AppendLine(string.Empty); 
                }
                
                if (dataType.Equals("collection"))
                {
                    var property = column.Name.Replace(IdentificationTag, string.Empty).Pluralize();
                    
                    if(!property.Equals(Table.Name) && property.Contains(column.ForeignTable)) 
                    {
                        result.AppendLine(($"[InverseProperty(nameof({column.Name}))]").Indent(2));
                        result.AppendLine(($"public virtual List<{column.ForeignTable}> {property} " + " { get; } = new List<"+ $"{column.ForeignTable}>();").Indent(2));
                        result.AppendLine(string.Empty); 
                    }
                }

                if (!string.IsNullOrEmpty(column.ForeignTable) )
                {
                    var property = column.Name.Replace(IdentificationTag, string.Empty).Singularize();

                    if(!property.Equals(Table.Name) && property.Contains(column.ForeignTable))
                    {
                        result.AppendLine(($"[ForeignKey(nameof({column.Name}))]").Indent(2));
                        result.AppendLine(($"public virtual {column.ForeignTable} {property} " + " { get; set; }").Indent(2));
                        result.AppendLine(string.Empty); 
                    }
                }
            }
                            
            result.AppendLine(("}").Indent(1));
            result.AppendLine(("}").Indent(0)); 

            return (Table.Name, result.ToString());    
        }
    
        public void Output(string BaseNamespace, bool OutputContext = true, bool OutputModels = true, string ContextPath = "Context/", string ModelsPath = "Models/", string ContextNamespace = "Database", 
            string ContextName = "DatabaseContext", string ModelNamespace = "Models", string IdentificationTag = "ID", string BaseClass = "", string Schema = "")
        {
            if (OutputContext)
            {
                var context = CreateDatabaseContext($"{BaseNamespace}.{ContextNamespace}", ContextName, $"{BaseNamespace}.{ModelNamespace}", Schema);
                var contextDirectory = ContextPath.Directory();
                var contextFile = $"{contextDirectory}{ContextName}.cs";

                if (!Directory.Exists(contextDirectory))
                {
                    Directory.CreateDirectory(contextDirectory);
                }

                using (StreamWriter outputFile = new StreamWriter(contextFile, false))
                {
                    outputFile.WriteLine(context);
                }
            }

            if (OutputModels)
            {
                var models = CreateModels($"{BaseNamespace}.{ModelNamespace}", IdentificationTag, BaseClass);
                var modelsDirectory = ModelsPath.Directory();

                if (!Directory.Exists(modelsDirectory))
                {
                    Directory.CreateDirectory(modelsDirectory);
                }

                foreach(var model in models)
                {
                    if (string.IsNullOrEmpty(model.Key)) continue;

                    var modelFile = $"{modelsDirectory}{model.Key}.cs";

                    using (StreamWriter outputFile = new StreamWriter(modelFile, false))
                    {
                        outputFile.WriteLine(model.Value);
                    }
                }
            }
        }

        
    }
}
