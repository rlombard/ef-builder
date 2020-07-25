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
        public List<TableDefinition> Tables {get; private set;}
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
            Tables = new List<TableDefinition>();
        }

        public void SetTableDefinitions(string Schema = "")
        {
            Tables.Clear();

            const string schemaQuery = "select obj.Object_Id AS [ID]," +
                                 "obj.Name as [Name], " +
                                 "sch.Name as [Schema] " +
                                 "from sys.objects obj " +
                                 "inner join sys.Schemas sch " +
                                 "on sch.schema_id = obj.schema_id " +
                                 "where obj.type = 'U' and sch.Name = @Schema" +
                                 "order by obj.Name ";

            const string standardQuery = "select obj.Object_Id AS [ID]," +
                                 "obj.Name as [Name], " +
                                 "sch.Name as [Schema] " +
                                 "from sys.objects obj " +
                                 "inner join sys.Schemas sch " +
                                 "on sch.schema_id = obj.schema_id " +
                                 "where obj.type = 'U' " +
                                 "order by obj.Name ";

            var query = string.IsNullOrEmpty(Schema) ? standardQuery : schemaQuery;

            using (var connection = new SqlConnection(_ConnectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();
                if (!string.IsNullOrEmpty(Schema))
                {
                     command.Parameters.AddWithValue("@Schema", Schema);
                }

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Tables.Add(TableDefinition.Construct(_ConnectionString, reader));
                    }
                }
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
            var result = new StringBuilder();
            _AppendContextUsing(ref result, ModelNamespace);
            _AppendNamespace(ref result, Namespace);
            _AppendContextClass(ref result, ContextName);
            _AppendContextOverrides(ref result, _ConnectionString);
            _AppendDatasets(ref result);
            _AppendContextModelBuilding(ref result, IdentificationTag);
            _CloseClass(ref result);
            _CloseNamespace(ref result);
            return result.ToString();
        }

        private void _AppendContextUsing(ref StringBuilder result, string ModelNamespace)
        {
            result.AppendLine("using Microsoft.EntityFrameworkCore;");
            result.AppendLine($"using {ModelNamespace};");
            result.AppendLine();
        }

        private void _AppendContextClass(ref StringBuilder result, string ContextName)
        {
            result.AppendLine(($"public partial class {ContextName} : DbContext").Indent(1));
            result.AppendLine(("{").Indent(1));

            result.AppendLine(($"public {ContextName}() : base()").Indent(2));
            result.AppendLine(("{").Indent(2));
            result.AppendLine(("}").Indent(2));
            result.AppendLine();

            result.AppendLine(($"public {ContextName}(DbContextOptions<{ContextName}> Options) : base(Options)").Indent(2));
            result.AppendLine(("{").Indent(2));
            result.AppendLine(("}").Indent(2));
            result.AppendLine();
        }

        private void _AppendDatasets(ref StringBuilder result)
        {
            foreach (var table in Tables)
            {
                result.AppendLine(($"public DbSet<{table.Name}> {table.Name.Pluralize()} " + " { get; set; }").Indent(2));
            }
            result.AppendLine();
        }

        private void _AppendContextModelBuilding(ref StringBuilder result, string IdentificationTag)
        {
            result.AppendLine(("protected override void OnModelCreating(ModelBuilder ModelBuilder)").Indent(2));
            result.AppendLine(("{").Indent(2));

            foreach (var table in Tables)
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
        }
        private void _AppendContextOverrides(ref StringBuilder result, string ConnectionString)
        {
            result.AppendLine(("protected override void OnConfiguring(DbContextOptionsBuilder OptionsBuilder)").Indent(2));
            result.AppendLine(("{").Indent(2));
            result.AppendLine(($"OptionsBuilder.UseSqlServer(@\"{_ConnectionString}\");").Indent(3));
            result.AppendLine(("}").Indent(2));
            result.AppendLine();
            result.AppendLine(("partial void OnModelCreatingPartial(ModelBuilder ModelBuilder);").Indent(2));
            result.AppendLine();
        }

        public List<KeyValuePair<string,string>> CreateModels(string Namespace, string IdentificationTag = "ID", string BaseClass = "", string Schema = "")
        {
            var models = new List<KeyValuePair<string,string>>();

            foreach (var table in Tables)
            {
                var (model, file) = CreateModel(table, Tables, Namespace, IdentificationTag, BaseClass);
                models.Add(new KeyValuePair<string,string>(model, file));
            }

            return models;
        }

        public (string, string) CreateModel(TableDefinition Table, List<TableDefinition> AllTables, string Namespace, string IdentificationTag = "ID", string BaseClass = "")
        {
            var foreignKeys = AllTables.Where(t => t.ForeignKeys.Any(fk => fk.HasOne.Equals(Table.Name)))
                                       .SelectMany(t => t.ForeignKeys).GroupBy(x => x.WithMany).Select(x => x.First())
                                       .ToList();

            var result = new StringBuilder();
            _AppendModelUsing(ref result, foreignKeys.Any());
            _AppendNamespace(ref result, Namespace);
            _AppendModelDecorator(ref result, Table.Name, Table.Schema, Table.Columns.FirstOrDefault(p => p.IsKey));
            _AppendModelClass(ref result, Table.Name, BaseClass);
            _AppendModelConstructor(ref result, Table.Name, foreignKeys);
            _AppendModelProperties(ref result, Table.Columns, IdentificationTag);
            _AppendModelCollections(ref result, foreignKeys);
            _CloseClass(ref result);
            _CloseNamespace(ref result);
            return (Table.Name, result.ToString());
        }

        private void _AppendModelUsing(ref StringBuilder result, bool HasCollections)
        {
            if (HasCollections)
            {
                result.AppendLine("using System.Collections.Generic;");
            }

            result.AppendLine("using System;");
            result.AppendLine("using System.ComponentModel.DataAnnotations;");
            result.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
            result.AppendLine("using System.Diagnostics;");
            result.AppendLine(string.Empty);
        }

        private void _AppendModelDecorator(ref StringBuilder result, string Name, string Schema, ColumnDefinition primaryColumn)
        {
            if (primaryColumn != null)
            {
                result.AppendLine(($"[DebuggerDisplay(\"{primaryColumn.Name} = " + "{" + primaryColumn.Name + "}\")]").Indent(1));
            }

            result.AppendLine(($"[Table(\"{Name}\", Schema = \"{Schema}\")]").Indent(1));
        }

        private void _AppendModelClass(ref StringBuilder result, string Name, string BaseClass)
        {
            if (string.IsNullOrEmpty(BaseClass))
            {
                result.AppendLine(($"public partial class {Name}").Indent(1));
            }
            else
            {
                result.AppendLine(($"public partial class {Name} : {BaseClass}").Indent(1));
            }

            result.AppendLine(("{").Indent(1));
        }

        private void _AppendModelConstructor(ref StringBuilder result, string Name, List<ForeignKeyDefinition> ForeignKeys)
        {
            result.AppendLine(($"public {Name}()").Indent(2));
            result.AppendLine(("{").Indent(2));
            foreach (var collection in ForeignKeys)
            {
                result.AppendLine(($"{collection.WithMany.Pluralize()} = new HashSet<{collection.WithMany}>();").Indent(3));
            }
            result.AppendLine(("}").Indent(2));
            result.AppendLine();
        }

        private void _AppendModelProperties(ref StringBuilder result, List<ColumnDefinition> columns, string IdentificationTag)
        {
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
        }

        private void _AppendModelCollections(ref StringBuilder result, List<ForeignKeyDefinition> ForeignKeys)
        {
            foreach (var foreignKey in ForeignKeys)
            {
                result.AppendLine(($"public virtual ICollection<{foreignKey.WithMany}> {foreignKey.WithMany.Pluralize()}" + " { get; set; }").Indent(2));
                result.AppendLine();
            }
        }

        private void _RemoveExcludedTables(IEnumerable<string> Exclude)
        {
            if (Exclude == null) return;

            foreach (var item in Exclude)
            {
                var remove = Tables.FirstOrDefault(t => t.Name.ToLowerInvariant().Equals(item.ToLowerInvariant()));
                if (remove == null) continue;

                Tables.Remove(remove);
            }
        }

        public void Build(BuilderOptions Options)
        {
            SetTableDefinitions(Options.Schema);
            _RemoveExcludedTables(Options.ExcludedTables);

            if (Options.OutputContext)
            {
                var context = CreateDatabaseContext($"{Options.BaseNamespace}.{Options.ContextNamespace}", Options.ContextName, $"{Options.BaseNamespace}.{Options.ModelNamespace}", Options.Schema, Options.IdentificationTag);
                var contextDirectory = Options.ContextPath;
                var contextFile = $"{contextDirectory}{Options.ContextName}.cs";

                if (!Directory.Exists(contextDirectory))
                {
                    Directory.CreateDirectory(contextDirectory);
                }

                using (StreamWriter outputFile = new StreamWriter(contextFile, false))
                {
                    outputFile.WriteLine(context);
                }
            }

            if (Options.OutputModels)
            {
                var models = CreateModels($"{Options.BaseNamespace}.{Options.ModelNamespace}", Options.IdentificationTag, Options.BaseClass);
                var modelsDirectory = Options.ModelsPath;

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

            if (Options.OutputControllers)
            {
                var controllers = CreateWebApiControllers(Options.BaseNamespace, Options.ControllerNamespace, Options.ContextNamespace, Options.ContextName, Options.ModelNamespace, Options.IdentificationTag, Options.Schema);
                var controllerDirectory = Options.ControllerPath;

                if (!Directory.Exists(controllerDirectory))
                {
                    Directory.CreateDirectory(controllerDirectory);
                }

                foreach(var controller in controllers)
                {
                    if (string.IsNullOrEmpty(controller.Key)) continue;

                    var controllerFile = $"{controllerDirectory}{controller.Key}.cs";

                    using (StreamWriter outputFile = new StreamWriter(controllerFile, false))
                    {
                        outputFile.WriteLine(controller.Value);
                    }
                }
            }
        }

        public List<KeyValuePair<string,string>> CreateWebApiControllers(string BaseNamespace, string ControllerNamespace, string ContextNamespace = "Database",  string ContextName = "DatabaseContext", string ModelNamespace = "Models", string IdentificationTag = "ID", string Schema = "")
        {
            var controllers = new List<KeyValuePair<string,string>>();

            foreach (var table in Tables)
            {
                try
                {
                    var (controller, file) = CreateController(table, Tables, $"{BaseNamespace}.{ControllerNamespace}", $"{BaseNamespace}.{ContextNamespace}", ContextName, $"{BaseNamespace}.{ModelNamespace}", IdentificationTag);
                    controllers.Add(new KeyValuePair<string,string>(controller, file));
                }
                catch (System.Exception)
                {
                    continue;
                }
            }

            return controllers;
        }

        public (string, string) CreateController(TableDefinition Table, List<TableDefinition> AllTables, string ControllerNamespace, string ContextNamespace = "Database",  string ContextName = "DatabaseContext", string ModelNamespace = "Models", string IdentificationTag = "ID")
        {
            var result = new StringBuilder();

            var keyColumn = Table.Columns.FirstOrDefault(x => x.IsKey);
            if (keyColumn == null) throw new Exception("Could not determine Key Column");

            var keyProperty = keyColumn.Name;
            var keyDataType = keyColumn.DataType.DataType(keyColumn.IsNullable);

            _AppendControllerUsing(ref result, ContextNamespace, ModelNamespace);
            _AppendNamespace(ref result, ControllerNamespace);
            _AppendControllerClass(ref result, Table.Name);
            _AppendControllerPropertiesAndConstructor(ref result, Table.Name, ContextName);
            _AppendControllerPosts(ref result, Table.Name, keyProperty);
            _AppendControllerGetters(ref result, Table.Name, keyProperty, keyDataType);
            _AppendControllerPuters(ref result, Table.Name, keyProperty, keyDataType);
            _AppendControllerDeletes(ref result, Table.Name, keyProperty, keyDataType);
            _AppendControllerAdditional(ref result, Table.Name, keyProperty, keyDataType);
            _CloseClass(ref result);
            _CloseNamespace(ref result);

            return ($"{Table.Name}Controller", result.ToString());
        }

        private void _AppendControllerUsing(ref StringBuilder result, string ContextNamespace, string ModelNamespace)
        {
            result.AppendLine("using Microsoft.AspNetCore.Http;");
            result.AppendLine("using Microsoft.AspNetCore.Mvc;");
            result.AppendLine("using Microsoft.EntityFrameworkCore;");
            result.AppendLine("using System;");
            result.AppendLine("using System.Collections.Generic;");
            result.AppendLine("using System.Linq;");
            result.AppendLine("using System.Threading.Tasks;");
            result.AppendLine($"using {ContextNamespace};");
            result.AppendLine($"using {ModelNamespace};");
            result.AppendLine(string.Empty);
        }

        private void _AppendNamespace(ref StringBuilder result, string Namespace)
        {
            result.AppendLine($"namespace {Namespace}");
            result.AppendLine(("{").Indent(0));
        }

        private void _CloseNamespace(ref StringBuilder result)
        {
            result.AppendLine(("}").Indent(0));
        }

         private void _AppendControllerClass(ref StringBuilder result, string Model)
         {
            result.AppendLine(("/// <summary>").Indent(1));
            result.AppendLine(($"/// Controller for {Model} Model to return basic CRUD and other lookup methods").Indent(1));
            result.AppendLine(("/// </summary>").Indent(1));
            result.AppendLine(($"[Route(\"api/[controller]\")]").Indent(1));
            result.AppendLine(($"[ApiController]").Indent(1));
            result.AppendLine(($"public class {Model}Controller : ControllerBase").Indent(1));
            result.AppendLine(("{").Indent(1));
         }

         private void _CloseClass(ref StringBuilder result)
         {
             result.AppendLine(("}").Indent(1));
         }

         private void _AppendControllerPropertiesAndConstructor(ref StringBuilder result, string Model, string ContextName)
         {
            result.AppendLine(($"private readonly {ContextName} _Database;").Indent(2));
            result.AppendLine();
            result.AppendLine(($"/// <summary>").Indent(2));
            result.AppendLine(($"/// Default Constructor").Indent(2));
            result.AppendLine(($"/// </summary>").Indent(2));
            result.AppendLine(($"/// <param name=\"Database\">Database Context to use</param>").Indent(2));
            result.AppendLine(($"public {Model}Controller({ContextName} Database)").Indent(2));
            result.AppendLine(("{").Indent(2));
            result.AppendLine(("_Database = Database;").Indent(3));
            result.AppendLine(("}").Indent(2));
            result.AppendLine();
         }

         private void _AppendControllerGetters(ref StringBuilder result, string Model, string KeyProperty, string KeyDataType)
         {
            result.AppendLine(("/// <summary>").Indent(2));
            result.AppendLine(($"/// Get all {Model.Pluralize()}").Indent(2));
            result.AppendLine(("/// </summary>").Indent(2));
            result.AppendLine(($"/// <returns>List of {Model} Models</returns>").Indent(2));
            result.AppendLine(("/// <example>").Indent(2));
            result.AppendLine(($"/// GET: api/{Model}").Indent(2));
            result.AppendLine(("/// </example>").Indent(2));
            result.AppendLine(("[HttpGet]").Indent(2));
            result.AppendLine(($"public async Task<ActionResult<IEnumerable<{Model}>>> Get{Model.Pluralize()}()").Indent(2));
            result.AppendLine(("{").Indent(2));
            result.AppendLine(($"return await _Database.{Model.Pluralize()}.ToListAsync();").Indent(3));
            result.AppendLine(("}").Indent(2));
            result.AppendLine();

            result.AppendLine(("/// <summary>").Indent(2));
            result.AppendLine(($"/// Get specific {Model}").Indent(2));
            result.AppendLine(("/// </summary>").Indent(2));
            result.AppendLine(($"/// <param name=\"{KeyProperty}\">Identity/Key Value</param>").Indent(2));
            result.AppendLine(($"/// <returns>{Model} Model with Identity/Key Value</returns>").Indent(2));
            result.AppendLine(("/// <example>").Indent(2));
            result.AppendLine(($"/// GET: api/{Model}/5").Indent(2));
            result.AppendLine(("/// </example>").Indent(2));
            result.AppendLine(("[HttpGet(\"{"+KeyProperty+"}\")]").Indent(2));
            result.AppendLine(($"public async Task<ActionResult<{Model}>> Get{Model}({KeyDataType} {KeyProperty})").Indent(2));
            result.AppendLine(("{").Indent(2));
            result.AppendLine(($"var model = await _Database.{Model.Pluralize()}.FindAsync({KeyProperty});").Indent(3));
            result.AppendLine();
            result.AppendLine(($"if (model == null)").Indent(3));
            result.AppendLine(("{").Indent(3));
            result.AppendLine(("return NotFound();").Indent(4));
            result.AppendLine(("}").Indent(3));
            result.AppendLine();
            result.AppendLine(($"return model;").Indent(3));
            result.AppendLine(("}").Indent(2));
            result.AppendLine();
         }

         private void _AppendControllerPuters(ref StringBuilder result, string Model, string KeyProperty, string KeyDataType)
         {
            result.AppendLine(("/// <summary>").Indent(2));
            result.AppendLine(("/// Updates the specified model").Indent(2));
            result.AppendLine(("/// </summary>").Indent(2));
            result.AppendLine(($"/// <param name=\"{KeyProperty}\">Identity/Key Value of {Model} to update</param>").Indent(2));
            result.AppendLine(($"/// <param name=\"Model\">Updated {Model}</param>").Indent(2));
            result.AppendLine(("/// <returns></returns>").Indent(2));
            result.AppendLine(("/// <example>").Indent(2));
            result.AppendLine(($"/// PUT: api/{Model}/5").Indent(2));
            result.AppendLine(("/// </example>").Indent(2));
            result.AppendLine(("[HttpPut(\"{"+KeyProperty+"}\")]").Indent(2));
            result.AppendLine(($"public async Task<IActionResult> Put{Model}({KeyDataType} {KeyProperty}, {Model} Model)").Indent(2));
            result.AppendLine(("{").Indent(2));
            result.AppendLine(($"if ({KeyProperty} != Model.{KeyProperty})").Indent(3));
            result.AppendLine(("{").Indent(3));
            result.AppendLine(("return BadRequest();").Indent(4));
            result.AppendLine(("}").Indent(3));
            result.AppendLine();
            result.AppendLine(($"_Database.Entry(Model).State = EntityState.Modified;").Indent(3));
            result.AppendLine();
            result.AppendLine(("try").Indent(3));
            result.AppendLine(("{").Indent(3));
            result.AppendLine(("await _Database.SaveChangesAsync();").Indent(4));
            result.AppendLine(("}").Indent(3));
            result.AppendLine(("catch (DbUpdateConcurrencyException)").Indent(3));
            result.AppendLine(("{").Indent(3));
            result.AppendLine(($"if (!{Model}Exists({KeyProperty}))").Indent(4));
            result.AppendLine(("{").Indent(4));
            result.AppendLine(("return NotFound();").Indent(5));
            result.AppendLine(("}").Indent(4));
            result.AppendLine(("else").Indent(4));
            result.AppendLine(("{").Indent(4));
            result.AppendLine(("throw;").Indent(5));
            result.AppendLine(("}").Indent(4));
            result.AppendLine(("}").Indent(3));
            result.AppendLine();
            result.AppendLine(("return NoContent();").Indent(3));
            result.AppendLine(("}").Indent(2));
            result.AppendLine();
         }

         private void _AppendControllerPosts(ref StringBuilder result, string Model, string KeyProperty)
         {
            result.AppendLine(("/// <summary>").Indent(2));
            result.AppendLine(($"/// Inserts a new {Model}").Indent(2));
            result.AppendLine(("/// </summary>").Indent(2));
            result.AppendLine(($"/// <param name=\"Model\"></param>").Indent(2));
            result.AppendLine(($"/// <returns>{Model}</returns>").Indent(2));
            result.AppendLine(("/// <example>").Indent(2));
            result.AppendLine(($"/// POST: api/{Model}").Indent(2));
            result.AppendLine(("/// </example>").Indent(2));
            result.AppendLine(("[HttpPost]").Indent(2));
            result.AppendLine(($"public async Task<ActionResult<{Model}>> Post{Model}({Model} Model)").Indent(2));
            result.AppendLine(("{").Indent(2));
            result.AppendLine(($"_Database.{Model.Pluralize()}.Add(Model);").Indent(3));
            result.AppendLine(("await _Database.SaveChangesAsync();").Indent(3));
            result.AppendLine();
            result.AppendLine(($"return CreatedAtAction(\"Get{Model}\"," +" new { " + KeyProperty + $" = Model.{KeyProperty}" +" }, Model);").Indent(3));
            result.AppendLine(("}").Indent(2));
            result.AppendLine();
         }

         private void _AppendControllerDeletes(ref StringBuilder result, string Model, string KeyProperty, string KeyDataType)
         {
            result.AppendLine(("/// <summary>").Indent(2));
            result.AppendLine(("/// Deletes the specified model").Indent(2));
            result.AppendLine(("/// </summary>").Indent(2));
            result.AppendLine(($"/// <param name=\"{KeyProperty}\">Identity/Key Value of {Model} to delete</param>").Indent(2));
            result.AppendLine(("/// <returns></returns>").Indent(2));
            result.AppendLine(("/// <example>").Indent(2));
            result.AppendLine(($"/// DELETE: api/{Model}/5").Indent(2));
            result.AppendLine(("/// </example>").Indent(2));
            result.AppendLine(("[HttpDelete(\"{"+KeyProperty+"}\")]").Indent(2));
            result.AppendLine(($"public async Task<ActionResult<{Model}>> Delete{Model}({KeyDataType} {KeyProperty})").Indent(2));
            result.AppendLine(("{").Indent(2));
            result.AppendLine(($"var model = await _Database.{Model.Pluralize()}.FindAsync({KeyProperty});").Indent(3));
            result.AppendLine(("if (model == null)").Indent(3));
            result.AppendLine(("{").Indent(3));
            result.AppendLine(("return NotFound();").Indent(4));
            result.AppendLine(("}").Indent(3));
            result.AppendLine();
            result.AppendLine(($"_Database.{Model.Pluralize()}.Remove(model);").Indent(3));
            result.AppendLine(("await _Database.SaveChangesAsync();").Indent(3));
            result.AppendLine();
            result.AppendLine(($"return model;").Indent(3));
            result.AppendLine(("}").Indent(2));
            result.AppendLine();
         }

         private void _AppendControllerAdditional(ref StringBuilder result, string Model, string KeyProperty, string KeyDataType)
         {
            result.AppendLine(("/// <summary>").Indent(2));
            result.AppendLine(($"/// Check if {Model} exists").Indent(2));
            result.AppendLine(("/// </summary>").Indent(2));
            result.AppendLine(($"/// <param name=\"{KeyProperty}\">Identity/Key Value of {Model}</param>").Indent(2));
            result.AppendLine(($"/// <returns>True if {Model} was found, false otherwise</returns>").Indent(2));
            result.AppendLine(($"private bool {Model}Exists({KeyDataType} {KeyProperty})").Indent(2));
            result.AppendLine(("{").Indent(2));
            result.AppendLine(($"return _Database.{Model.Pluralize()}.Any(e => e.{KeyProperty} == {KeyProperty});").Indent(3));
            result.AppendLine(("}").Indent(2));
         }
    }
}
