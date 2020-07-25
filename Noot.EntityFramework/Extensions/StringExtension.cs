using System.Text;
using System.Linq;
using Humanizer;

namespace Noot.EntityFramework.Extensions
{
    public static class StringExtension
    {
        private const byte _indentSize = 4;

        public static string Indent(this string originalString, int indentLevel)
        {
            StringBuilder indentedString = new StringBuilder();
            indentedString.Append("".PadLeft(indentLevel * _indentSize));
            indentedString.Append(originalString);
            return indentedString.ToString();
        }

        public static string DataType(this string dataType, bool IsNullable)
        {
            switch (dataType)
            {
                case "int":
                case "tinyint":
                case "bigint":
                    return IsNullable ? "int?" : "int";
                case "float":
                    return IsNullable ? "double?" : "double";
                case "nvarchar":
                case "varchar":
                    return "string";
                case "varbinary":
                    return"byte[]";
                case "bit":
                    return IsNullable ? "bool?" : "bool";
                case "datetime2":
                case "datetime":
                    return IsNullable ? "DateTime?" : "DateTime";
                case "relationship":
                case "collection":
                    return dataType;
                default: return string.Empty;
            }
        }

        public static string DecorateLength(this string field, int MaxLength, string DataType)
        {
            return !DataType.ToLowerInvariant().Equals("string") ? string.Empty : MaxLength > 0 ? $"[StringLength({MaxLength}, ErrorMessage = \"{field.Humanize()} cannot exceed more than {MaxLength} characters.\")]" : string.Empty;
        }

        public static string DecorateKey(this string field, bool IsIdentity)
        {
            return IsIdentity ? "[Key]" : string.Empty;
        }

        public static string DecorateRequired(this string field, bool IsNullable, bool IsKey)
        {
            return !IsNullable && !IsKey ? ($"[Required(ErrorMessage = \"{field.Humanize()} is a required field.\")]") : string.Empty;
        }

        public static string DecorateRange(this string field, string DataType, bool IsKey)
        {
            return  DataType.ToLowerInvariant() == "int" && !IsKey ? $"[Range(1, 99999999, ErrorMessage = \"Value for {field.Humanize()} must be greater than 0.\")]" : string.Empty;
        }

        public static string Directory(this string Path)
        {
            return Path.Last() == '/' || Path.Last() == '\\' ? Path : Path.Any(x => x == '\\') ? $"{Path}\\" : $"{Path}/";
        }
    }
}