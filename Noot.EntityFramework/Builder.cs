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
                        result.Add(TableDefinition.Construct(_ConnectionString, reader));
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
                        result = TableDefinition.Construct(_ConnectionString, reader);
                    }
                }

                return result;
            }                    
        }

        public string CreateDatabaseContext(string Namespace, string ContextName, string ModelNamespace, string Schema = "", string IdentificationTag = "ID")
        {
            var tables = GetTableDefinitions(Schema);

            var result = new StringBuilder();       
            result.AppendLine("using Microsoft.EntityFrameworkCore;");
            result.AppendLine($"using {ModelNamespace};");
            result.AppendLine(string.Empty);                
            result.AppendLine($"namespace {Namespace}");
            result.AppendLine(("{").Indent(0));
            result.AppendLine(($"public partial class {ContextName} : DbContext").Indent(1));
            result.AppendLine(("{").Indent(1));

            result.AppendLine(($"public {ContextName}() : base()").Indent(2));
            result.AppendLine(("{").Indent(2));
            result.AppendLine(("}").Indent(2));
            result.AppendLine(string.Empty); 

            result.AppendLine(($"public {ContextName}(DbContextOptions<{ContextName}> Options) : base(Options)").Indent(2));
            result.AppendLine(("{").Indent(2));
            result.AppendLine(("}").Indent(2));
            result.AppendLine(string.Empty); 

            foreach (var table in tables)
            {
                result.AppendLine(($"public DbSet<{table.Name}> {table.Name.Pluralize()} " + " { get; set; }").Indent(2));
            }

            result.AppendLine(("protected override void OnConfiguring(DbContextOptionsBuilder OptionsBuilder)").Indent(2));
            result.AppendLine(("{").Indent(2));
            result.AppendLine(($"OptionsBuilder.UseSqlServer(@\"{_ConnectionString}\");").Indent(3));
            result.AppendLine(("}").Indent(2));
            result.AppendLine(string.Empty); 

            result.AppendLine(("protected override void OnModelCreating(ModelBuilder ModelBuilder)").Indent(2));
            result.AppendLine(("{").Indent(2));
            
            foreach (var table in tables)
            {
                if (!table.HasKey && !table.ForeignKeys.Any() && !table.TableIndices.Any() && !table.DefaultColumns.Any()) 
                {
                    result.AppendLine(($"ModelBuilder.Entity<{table.Name}>(entity =>").Indent(3));
                    result.AppendLine(("{").Indent(3));
                    result.AppendLine(("entity.HasNoKey();").Indent(4));
                    result.AppendLine(("});").Indent(3)); 
                    result.AppendLine(string.Empty);                    
                }
                else
                {
                    result.AppendLine(($"ModelBuilder.Entity<{table.Name}>(entity =>").Indent(3));
                    result.AppendLine(("{").Indent(3));
                    if (table.TableIndices.Any())
                    {
                        foreach (var index in table.TableIndices)
                        {
                            if (index.Name.ToLowerInvariant().Contains("materialized")) continue;

                            result.AppendLine(("entity.HasIndex(e => new { "+ index.Columns +" })").Indent(4));                           
                            if (index.IsUnique)
                            {
                                result.AppendLine(($".HasName(\"{index.Name}\")").Indent(5));
                                result.AppendLine((".IsUnique();").Indent(5));
                            }
                            else
                            {
                                result.AppendLine(($".HasName(\"{index.Name}\");").Indent(5));
                            }
                        }  
                        result.AppendLine(string.Empty);                      
                    }

                    if (table.ForeignKeys.Any())
                    {
                        foreach (var foreignKey in table.ForeignKeys)
                        {
                            var column = table.Columns.FirstOrDefault(x => x.HasForeignKey && x.ForeignTable.Equals(foreignKey.HasOne));
                            var hasOne = foreignKey.HasOne;
                            if (column != null)
                            {
                                hasOne = column.Name.Replace(IdentificationTag, "");
                            }

                            result.AppendLine(($"entity.HasOne(d => d.{hasOne})").Indent(4));                           
                            result.AppendLine(($".WithMany(p => p.{foreignKey.WithMany.Pluralize()})").Indent(5));
                            result.AppendLine(($".HasForeignKey(d => d.{foreignKey.HasForeignKey})").Indent(5));
                            result.AppendLine(($".HasConstraintName(\"{foreignKey.HasConstraintName}\");").Indent(5));                            
                        }  
                        result.AppendLine(string.Empty);   
                    }

                    if (table.DefaultColumns.Any())
                    {
                        foreach (var defaultColumn in table.DefaultColumns)
                        {
                            result.AppendLine(($"entity.Property(e => e.{defaultColumn.ColumnName}).HasDefaultValueSql(\"{defaultColumn.Value}\");").Indent(4));                           
                        } 
                        result.AppendLine(string.Empty);    
                    }

                    result.AppendLine(("});").Indent(3));
                }

                result.AppendLine(string.Empty); 
            }

            result.AppendLine(("}").Indent(2));
            result.AppendLine(string.Empty); 
            result.AppendLine(("partial void OnModelCreatingPartial(ModelBuilder modelBuilder);").Indent(2));
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
                var (model, file) = CreateModel(table, tables, Namespace, IdentificationTag, BaseClass);
                models.Add(new KeyValuePair<string,string>(model, file));       
            }
          
            return models;          
        }
    
        public (string, string) CreateModel(TableDefinition Table, List<TableDefinition> AllTables, string Namespace, string IdentificationTag = "ID", string BaseClass = "")
        {
            var columns = Table.Columns;
            var fks = AllTables.Where(t => t.ForeignKeys.Any(fk => fk.HasOne.Equals(Table.Name)))
            .SelectMany(t => t.ForeignKeys).GroupBy(x => x.WithMany).Select(x => x.First())
            .ToList();

            var result = new StringBuilder();     

            if (fks.Any())
            {
                result.AppendLine("using System.Collections.Generic;");
            }  
            
            result.AppendLine("using System;");
            result.AppendLine("using System.ComponentModel.DataAnnotations;");
            result.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
            result.AppendLine("using System.Diagnostics;");          

            result.AppendLine(string.Empty);                
            result.AppendLine($"namespace {Namespace}");
            result.AppendLine(("{").Indent(0));
            
            var primaryColumn = columns.FirstOrDefault(x => x.IsKey);
            if (primaryColumn != null)
            {
                result.AppendLine(($"[DebuggerDisplay(\"{primaryColumn.Name} = " + "{" + primaryColumn.Name + "}\")]").Indent(1));
            }

            result.AppendLine(($"[Table(\"{Table.Name}\", Schema = \"{Table.Schema}\")]").Indent(1));

            if (string.IsNullOrEmpty(BaseClass))
            {
                result.AppendLine(($"public partial class {Table.Name}").Indent(1));
            }
            else
            {
                result.AppendLine(($"public partial class {Table.Name} : {BaseClass}").Indent(1));
            }
            
            result.AppendLine(("{").Indent(1));
      
            result.AppendLine(($"public {Table.Name}()").Indent(2));
            result.AppendLine(("{").Indent(2));
            foreach (var collection in fks)
            {
                result.AppendLine(($"{collection.WithMany.Pluralize()} = new HashSet<{collection.WithMany}>();").Indent(3));
            }
            result.AppendLine(("}").Indent(2));
                        
            foreach (var column in columns)
            {               
                var dataType = column.DataType.DataType(column.IsNullable);
                if (!string.IsNullOrEmpty(dataType))
                {
                    var key = column.Name.DecorateKey(column.IsKey);
                    var required = column.Name.DecorateRequired(column.IsNullable, column.IsKey);
                    var range = column.Name.DecorateRange(dataType, column.IsKey);
                    var length = column.Name.DecorateLength(column.MaxLength, dataType);

                    if (!string.IsNullOrEmpty(key)) result.AppendLine(key.Indent(2));
                    if (!string.IsNullOrEmpty(required)) result.AppendLine(required.Indent(2));
                    if (!string.IsNullOrEmpty(range)) result.AppendLine(range.Indent(2));
                    if (!string.IsNullOrEmpty(length)) result.AppendLine(length.Indent(2));

                    result.AppendLine(($"public {dataType} {column.Name}" + " { get; set; }").Indent(2));

                    if(column.HasForeignKey)
                    {
                        result.AppendLine(string.Empty); 
                        result.AppendLine(($"[ForeignKey(nameof({column.Name}))]").Indent(2));                        
                        result.AppendLine(($"public virtual {column.ForeignTable} {column.Name.Replace(IdentificationTag, "")}" + " { get; set; }").Indent(2));
                    }
                   
                    result.AppendLine(string.Empty); 
                }                                
            }

            foreach (var foreignKey in fks)
            {
                result.AppendLine(($"public virtual ICollection<{foreignKey.WithMany}> {foreignKey.WithMany.Pluralize()}" + " { get; set; }").Indent(2));
                result.AppendLine(string.Empty); 
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
                var context = CreateDatabaseContext($"{BaseNamespace}.{ContextNamespace}", ContextName, $"{BaseNamespace}.{ModelNamespace}", Schema, IdentificationTag);
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
