namespace Noot.EntityFramework.Models
{
    public class ForeignKeyDefinition
    {
        public string HasOne {get;set;}
        public string WithMany {get;set;}
        public string HasForeignKey {get;set;}
        public string HasConstraintName {get;set;}
    }
}