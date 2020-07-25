using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using Noot.EntityFramework.Extensions;

namespace Noot.EntityFramework.Models
{
    public class BuilderOptions
    {
        public string BaseNamespace {get;set;}
        public bool OutputContext {get;set;}
        public bool OutputModels {get;set;}
        public string ContextPath {get;set;}
        public string ModelsPath {get;set;}
        public string ContextNamespace {get;set;}
        public string ContextName {get;set;}
        public string ModelNamespace{get;set;}
        public string IdentificationTag {get;set;}
        public string BaseClass{get;set;}
        public string Schema{get;set;}
        public string ControllerPath {get;set;}
        public string ControllerNamespace {get;set;}
        public bool OutputControllers {get;set;}

        public List<string> ExcludedTables {get;set;}

        public static BuilderOptions Default()
        {
            return new BuilderOptions()
            {
                BaseNamespace = "MyProgram",
                OutputContext = true,
                OutputModels = true,
                OutputControllers = true,
                BaseClass = string.Empty,
                ContextName = "Database",
                ContextNamespace = "Data",
                ContextPath = "Data/",
                ControllerNamespace = "Controllers",
                ControllerPath = "Controllers/",
                IdentificationTag = "ID",
                ModelNamespace = "Models",
                ModelsPath = "Models/",
                Schema = string.Empty
            };
        }

        public static BuilderOptions Construct()
        {
            return Default();
        }

        public static BuilderOptions Construct(string Namespace, string PathPrefix = "")
        {
            var options = Default();
            options.SetBaseNamespace(Namespace);
            options.PrefixPath(PathPrefix);
            return options;
        }

        public void SetBaseNamespace(string Namespace)
        {
            BaseNamespace = Namespace;
        }

        public void PrefixPath(string Prefix)
        {
            ContextPath = (Prefix + ContextPath).Directory();
            ModelsPath = (Prefix + ModelsPath).Directory();
            ControllerPath = (Prefix + ControllerPath).Directory();
        }

        public void ExcludeTable(string TableName)
        {
            if (ExcludedTables == null)
            {
                ExcludedTables = new List<string>();
            }

            if (!ExcludedTables.Any(t => t.Equals(TableName)))
            {
                ExcludedTables.Add(TableName);
            }
        }
    }
}