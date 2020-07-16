using System;
using System.Collections.Generic;
using Noot.EntityFramework;
using Noot.DataAccess.Database;
using Noot.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Noot.Debugging
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            // var builder = new Builder("geoinventory.southafricanorth.cloudapp.azure.com", "CORDISDB", "InventAdmin", "Geonator1234");
            // builder.Output("Noot.DataAccess", 
            // ContextPath:@"D:\Code\Github\ef-builder\Noot.Debugging\DataAccess", 
            // ModelsPath:@"D:\Code\Github\ef-builder\Noot.Debugging\Models", ContextName:"CordisDatabase",
            // Schema: "com");

            using (var context = new CordisDatabase()) 
            {
                var sites = context.Sites;
                
                foreach(var site in sites)
                {
                    Console.WriteLine(site.Name);
                    
                }
            }            
        }
    }
}
