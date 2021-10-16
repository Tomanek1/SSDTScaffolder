using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSDTScaffolderLib.Helpers
{
    public class DataTypeHelper
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="DataTypeHelper"/> class from being created. 
        /// Created from this reference: https://msdn.microsoft.com/en-us/library/cc716729%28v=vs.110%29.aspx
        /// </summary>
        private DataTypeHelper()
        {
            LoadDataTypes();
        }

        private IDictionary<DataTypes, IDictionary<TypeFormat, string>> dataTypes;

        private static DataTypeHelper instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static DataTypeHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataTypeHelper();
                }
                return instance;
            }
        }

        /// <summary>
        /// Converts the specified source data type into the specified destination data type.
        /// </summary>
        /// <param name="sourceDataType">The source data type. For example <c>bigint</c>.</param>
        /// <param name="sourceFormat">The format of the the sourceDataType parameter.</param>
        /// <param name="destinationFormat">The format to convert the sourceDataType parameter to.</param>
        /// <returns>
        /// The converted data type.
        /// </returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public string ToDataType(string sourceDataType, TypeFormat sourceFormat, TypeFormat destinationFormat)
        {
            var map = this.GetMap(sourceFormat, sourceDataType);
            if (map == null) throw new NotSupportedException(string.Format("Could not find a {0} data type of {1}. Ensure the sourceFormat is appropriate for the specified sourceDataType.", Enum.GetName(typeof(TypeFormat), sourceFormat), sourceDataType));
            return map[destinationFormat];
        }

        /// <summary>
        /// Gets a single data type dictionary with values for each TypeFormat by finding the specified entry in the list of data type dictionaries.
        /// </summary>
        /// <param name="lookupFormat">The TypeFormat of the lookupValue parameter.</param>
        /// <param name="lookupValue">The value to identify the dictionary to return.</param>
        /// <returns>
        /// The first data type dictionary that contains the specified TypeFormat and value.
        /// </returns>
        public IDictionary<TypeFormat, string> GetMap(TypeFormat lookupFormat, string lookupValue)
        {
            return dataTypes.Values.FirstOrDefault(dic => dic.Any(entry => entry.Key == lookupFormat && entry.Value == lookupValue));
        }

        /// <summary>
        /// Gets a single data type dictionary with values for each TypeFormat 
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <returns></returns>
        public IDictionary<TypeFormat, string> GetMap(DataTypes dataType)
        {
            return dataTypes[dataType];
        }

        private void LoadDataTypes()
        {
            dataTypes = new Dictionary<DataTypes, IDictionary<TypeFormat, string>>();
            dataTypes.Add(DataTypes.@bigint, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "bigint" }, { TypeFormat.DotNetFrameworkNullableType, "long?" }, { TypeFormat.DotNetFrameworkNotNullableType, "long" }, { TypeFormat.SqlDbTypeEnum, "BigInt" }, { TypeFormat.SqlDataReaderSqlType, "GetSqlInt64" }, { TypeFormat.DbTypeEnum, "Int64" }, { TypeFormat.SqlDataReaderDbType, "GetInt64" } });
            dataTypes.Add(DataTypes.@binary, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "binary" }, { TypeFormat.DotNetFrameworkNullableType, "byte[]" }, { TypeFormat.DotNetFrameworkNotNullableType, "byte[]" }, { TypeFormat.SqlDbTypeEnum, "VarBinary" }, { TypeFormat.SqlDataReaderSqlType, "GetSqlBinary" }, { TypeFormat.DbTypeEnum, "Binary" }, { TypeFormat.SqlDataReaderDbType, "GetBytes" } });
            dataTypes.Add(DataTypes.@bit, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "bit" }, { TypeFormat.DotNetFrameworkNullableType, "bool?" }, { TypeFormat.DotNetFrameworkNotNullableType, "bool" }, { TypeFormat.SqlDbTypeEnum, "Bit" }, { TypeFormat.SqlDataReaderSqlType, "GetSqlBoolean" }, { TypeFormat.DbTypeEnum, "Boolean" }, { TypeFormat.SqlDataReaderDbType, "GetBoolean" } });
            dataTypes.Add(DataTypes.@char, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "char" }, { TypeFormat.DotNetFrameworkNullableType, "string" }, { TypeFormat.DotNetFrameworkNotNullableType, "string" }, { TypeFormat.SqlDbTypeEnum, "Char" }, { TypeFormat.SqlDataReaderSqlType, "GetSqlString" }, { TypeFormat.DbTypeEnum, "String" }, { TypeFormat.SqlDataReaderDbType, "GetString" } });
            dataTypes.Add(DataTypes.@date, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "date" }, { TypeFormat.DotNetFrameworkNullableType, "DateTime?" }, { TypeFormat.DotNetFrameworkNotNullableType, "DateTime" }, { TypeFormat.SqlDbTypeEnum, "Date" }, { TypeFormat.SqlDataReaderSqlType, "GetSqlDateTime" }, { TypeFormat.DbTypeEnum, "Date" }, { TypeFormat.SqlDataReaderDbType, "GetDateTime" } });
            dataTypes.Add(DataTypes.@datetime, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "datetime" }, { TypeFormat.DotNetFrameworkNullableType, "DateTime?" }, { TypeFormat.DotNetFrameworkNotNullableType, "DateTime" }, { TypeFormat.SqlDbTypeEnum, "DateTime" }, { TypeFormat.SqlDataReaderSqlType, "GetSqlDateTime" }, { TypeFormat.DbTypeEnum, "DateTime" }, { TypeFormat.SqlDataReaderDbType, "GetDateTime" } });
            dataTypes.Add(DataTypes.@datetime2, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "datetime2" }, { TypeFormat.DotNetFrameworkNullableType, "DateTime?" }, { TypeFormat.DotNetFrameworkNotNullableType, "DateTime" }, { TypeFormat.SqlDbTypeEnum, "DateTime2" }, { TypeFormat.SqlDataReaderSqlType, null }, { TypeFormat.DbTypeEnum, "DateTime2" }, { TypeFormat.SqlDataReaderDbType, "GetDateTime" } });
            dataTypes.Add(DataTypes.@datetimeoffset, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "datetimeoffset" }, { TypeFormat.DotNetFrameworkNullableType, "DateTimeOffset?" }, { TypeFormat.DotNetFrameworkNotNullableType, "DateTimeOffset" }, { TypeFormat.SqlDbTypeEnum, "DateTimeOffset" }, { TypeFormat.SqlDataReaderSqlType, null }, { TypeFormat.DbTypeEnum, "DateTimeOffset" }, { TypeFormat.SqlDataReaderDbType, "GetDateTimeOffset" } });
            dataTypes.Add(DataTypes.@decimal, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "decimal" }, { TypeFormat.DotNetFrameworkNullableType, "decimal?" }, { TypeFormat.DotNetFrameworkNotNullableType, "decimal" }, { TypeFormat.SqlDbTypeEnum, "Decimal" }, { TypeFormat.SqlDataReaderSqlType, "GetSqlDecimal" }, { TypeFormat.DbTypeEnum, "Decimal" }, { TypeFormat.SqlDataReaderDbType, "GetDecimal" } });
            dataTypes.Add(DataTypes.@float, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "float" }, { TypeFormat.DotNetFrameworkNullableType, "double?" }, { TypeFormat.DotNetFrameworkNotNullableType, "double" }, { TypeFormat.SqlDbTypeEnum, "Float" }, { TypeFormat.SqlDataReaderSqlType, "GetSqlDouble" }, { TypeFormat.DbTypeEnum, "Double" }, { TypeFormat.SqlDataReaderDbType, "GetDouble" } });
            dataTypes.Add(DataTypes.@image, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "image" }, { TypeFormat.DotNetFrameworkNullableType, "byte[]" }, { TypeFormat.DotNetFrameworkNotNullableType, "byte[]" }, { TypeFormat.SqlDbTypeEnum, "Binary" }, { TypeFormat.SqlDataReaderSqlType, "GetSqlBinary" }, { TypeFormat.DbTypeEnum, "Binary" }, { TypeFormat.SqlDataReaderDbType, "GetBytes" } });
            dataTypes.Add(DataTypes.@int, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "int" }, { TypeFormat.DotNetFrameworkNullableType, "int?" }, { TypeFormat.DotNetFrameworkNotNullableType, "int" }, { TypeFormat.SqlDbTypeEnum, "Int" }, { TypeFormat.SqlDataReaderSqlType, "GetSqlInt32" }, { TypeFormat.DbTypeEnum, "Int32" }, { TypeFormat.SqlDataReaderDbType, "GetInt32" } });
            dataTypes.Add(DataTypes.@money, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "money" }, { TypeFormat.DotNetFrameworkNullableType, "decimal?" }, { TypeFormat.DotNetFrameworkNotNullableType, "decimal" }, { TypeFormat.SqlDbTypeEnum, "Money" }, { TypeFormat.SqlDataReaderSqlType, "GetSqlMoney" }, { TypeFormat.DbTypeEnum, "Decimal" }, { TypeFormat.SqlDataReaderDbType, "GetDecimal" } });
            dataTypes.Add(DataTypes.@nchar, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "nchar" }, { TypeFormat.DotNetFrameworkNullableType, "string" }, { TypeFormat.DotNetFrameworkNotNullableType, "string" }, { TypeFormat.SqlDbTypeEnum, "NChar" }, { TypeFormat.SqlDataReaderSqlType, "GetSqlString" }, { TypeFormat.DbTypeEnum, "StringFixedLength" }, { TypeFormat.SqlDataReaderDbType, "GetString" } });
            dataTypes.Add(DataTypes.@ntext, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "ntext" }, { TypeFormat.DotNetFrameworkNullableType, "string" }, { TypeFormat.DotNetFrameworkNotNullableType, "string" }, { TypeFormat.SqlDbTypeEnum, "NText" }, { TypeFormat.SqlDataReaderSqlType, "GetSqlString" }, { TypeFormat.DbTypeEnum, "String" }, { TypeFormat.SqlDataReaderDbType, "GetString" } });
            dataTypes.Add(DataTypes.@numeric, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "numeric" }, { TypeFormat.DotNetFrameworkNullableType, "decimal?" }, { TypeFormat.DotNetFrameworkNotNullableType, "decimal" }, { TypeFormat.SqlDbTypeEnum, "Decimal" }, { TypeFormat.SqlDataReaderSqlType, "GetSqlDecimal" }, { TypeFormat.DbTypeEnum, "Decimal" }, { TypeFormat.SqlDataReaderDbType, "GetDecimal" } });
            dataTypes.Add(DataTypes.@nvarchar, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "nvarchar" }, { TypeFormat.DotNetFrameworkNullableType, "string" }, { TypeFormat.DotNetFrameworkNotNullableType, "string" }, { TypeFormat.SqlDbTypeEnum, "NVarChar" }, { TypeFormat.SqlDataReaderSqlType, "GetSqlString" }, { TypeFormat.DbTypeEnum, "String" }, { TypeFormat.SqlDataReaderDbType, "GetString" } });
            dataTypes.Add(DataTypes.@real, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "real" }, { TypeFormat.DotNetFrameworkNullableType, "float?" }, { TypeFormat.DotNetFrameworkNotNullableType, "float" }, { TypeFormat.SqlDbTypeEnum, "Real" }, { TypeFormat.SqlDataReaderSqlType, "GetSqlSingle" }, { TypeFormat.DbTypeEnum, "Single" }, { TypeFormat.SqlDataReaderDbType, "GetFloat" } });
            dataTypes.Add(DataTypes.@rowversion, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "rowversion" }, { TypeFormat.DotNetFrameworkNullableType, "byte[]" }, { TypeFormat.DotNetFrameworkNotNullableType, "byte[]" }, { TypeFormat.SqlDbTypeEnum, "Timestamp" }, { TypeFormat.SqlDataReaderSqlType, "GetSqlBinary" }, { TypeFormat.DbTypeEnum, "Binary" }, { TypeFormat.SqlDataReaderDbType, "GetBytes" } });
            dataTypes.Add(DataTypes.@smalldatetime, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "smalldatetime" }, { TypeFormat.DotNetFrameworkNullableType, "DateTime?" }, { TypeFormat.DotNetFrameworkNotNullableType, "DateTime" }, { TypeFormat.SqlDbTypeEnum, "DateTime" }, { TypeFormat.SqlDataReaderSqlType, "GetSqlDateTime" }, { TypeFormat.DbTypeEnum, "DateTime" }, { TypeFormat.SqlDataReaderDbType, "GetDateTime" } });
            dataTypes.Add(DataTypes.@smallint, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "smallint" }, { TypeFormat.DotNetFrameworkNullableType, "small?" }, { TypeFormat.DotNetFrameworkNotNullableType, "small" }, { TypeFormat.SqlDbTypeEnum, "SmallInt" }, { TypeFormat.SqlDataReaderSqlType, "GetSqlInt16" }, { TypeFormat.DbTypeEnum, "Int16" }, { TypeFormat.SqlDataReaderDbType, "GetInt16" } });
            dataTypes.Add(DataTypes.@smallmoney, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "smallmoney" }, { TypeFormat.DotNetFrameworkNullableType, "decimal?" }, { TypeFormat.DotNetFrameworkNotNullableType, "decimal" }, { TypeFormat.SqlDbTypeEnum, "SmallMoney" }, { TypeFormat.SqlDataReaderSqlType, "GetSqlMoney" }, { TypeFormat.DbTypeEnum, "Decimal" }, { TypeFormat.SqlDataReaderDbType, "GetDecimal" } });
            dataTypes.Add(DataTypes.@sql_variant, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "sql_variant" }, { TypeFormat.DotNetFrameworkNullableType, "object" }, { TypeFormat.DotNetFrameworkNotNullableType, "object" }, { TypeFormat.SqlDbTypeEnum, "Variant" }, { TypeFormat.SqlDataReaderSqlType, "GetSqlValue *" }, { TypeFormat.DbTypeEnum, "Object" }, { TypeFormat.SqlDataReaderDbType, "GetValue" } });
            dataTypes.Add(DataTypes.@text, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "text" }, { TypeFormat.DotNetFrameworkNullableType, "string" }, { TypeFormat.DotNetFrameworkNotNullableType, "string" }, { TypeFormat.SqlDbTypeEnum, "Text" }, { TypeFormat.SqlDataReaderSqlType, "GetSqlString" }, { TypeFormat.DbTypeEnum, "String" }, { TypeFormat.SqlDataReaderDbType, "GetString" } });
            dataTypes.Add(DataTypes.@time, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "time" }, { TypeFormat.DotNetFrameworkNullableType, "TimeSpan?" }, { TypeFormat.DotNetFrameworkNotNullableType, "TimeSpan" }, { TypeFormat.SqlDbTypeEnum, "Time" }, { TypeFormat.SqlDataReaderSqlType, "none" }, { TypeFormat.DbTypeEnum, "Time" }, { TypeFormat.SqlDataReaderDbType, "GetDateTime" } });
            dataTypes.Add(DataTypes.@timestamp, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "timestamp" }, { TypeFormat.DotNetFrameworkNullableType, "byte[]" }, { TypeFormat.DotNetFrameworkNotNullableType, "byte[]" }, { TypeFormat.SqlDbTypeEnum, "Timestamp" }, { TypeFormat.SqlDataReaderSqlType, "GetSqlBinary" }, { TypeFormat.DbTypeEnum, "Binary" }, { TypeFormat.SqlDataReaderDbType, "GetBytes" } });
            dataTypes.Add(DataTypes.@tinyint, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "tinyint" }, { TypeFormat.DotNetFrameworkNullableType, "byte?" }, { TypeFormat.DotNetFrameworkNotNullableType, "byte" }, { TypeFormat.SqlDbTypeEnum, "TinyInt" }, { TypeFormat.SqlDataReaderSqlType, "GetSqlByte" }, { TypeFormat.DbTypeEnum, "Byte" }, { TypeFormat.SqlDataReaderDbType, "GetByte" } });
            dataTypes.Add(DataTypes.@uniqueidentifier, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "uniqueidentifier" }, { TypeFormat.DotNetFrameworkNullableType, "Guid" }, { TypeFormat.DotNetFrameworkNotNullableType, "Guid" }, { TypeFormat.SqlDbTypeEnum, "UniqueIdentifier" }, { TypeFormat.SqlDataReaderSqlType, "GetSqlGuid" }, { TypeFormat.DbTypeEnum, "Guid" }, { TypeFormat.SqlDataReaderDbType, "GetGuid" } });
            dataTypes.Add(DataTypes.@varbinary, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "varbinary" }, { TypeFormat.DotNetFrameworkNullableType, "byte[]" }, { TypeFormat.DotNetFrameworkNotNullableType, "byte[]" }, { TypeFormat.SqlDbTypeEnum, "VarBinary" }, { TypeFormat.SqlDataReaderSqlType, "GetSqlBinary" }, { TypeFormat.DbTypeEnum, "Binary" }, { TypeFormat.SqlDataReaderDbType, "GetBytes" } });
            dataTypes.Add(DataTypes.@varchar, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "varchar" }, { TypeFormat.DotNetFrameworkNullableType, "string" }, { TypeFormat.DotNetFrameworkNotNullableType, "string" }, { TypeFormat.SqlDbTypeEnum, "VarChar" }, { TypeFormat.SqlDataReaderSqlType, "GetSqlString" }, { TypeFormat.DbTypeEnum, "String" }, { TypeFormat.SqlDataReaderDbType, "GetString" } });
            dataTypes.Add(DataTypes.@xml, new Dictionary<TypeFormat, string> { { TypeFormat.SqlServerDbType, "xml" }, { TypeFormat.DotNetFrameworkNullableType, "Xml" }, { TypeFormat.DotNetFrameworkNotNullableType, "Xml" }, { TypeFormat.SqlDbTypeEnum, "Xml" }, { TypeFormat.SqlDataReaderSqlType, "GetSqlXml" }, { TypeFormat.DbTypeEnum, "Xml" }, { TypeFormat.SqlDataReaderDbType, null } });
        }
    }

    /// <summary>
    /// The different types of datatypes available.
    /// </summary>
    public enum TypeFormat
    {
        /// <summary>
        /// The SQL server database type
        /// </summary>
        /// <example>"bigint"</example>
        SqlServerDbType,

        /// <summary>
        /// The dot net framework type
        /// </summary>
        /// <example>"Int64?"</example>
        DotNetFrameworkNullableType,

        /// <summary>
        /// The dot net framework type
        /// </summary>
        /// <example>"Int64?"</example>
        DotNetFrameworkNotNullableType,

        /// <summary>
        /// The SqlDbTypeEnum type
        /// </summary>
        /// <example>"BigInt"</example>
        SqlDbTypeEnum,

        /// <summary>
        /// The SQL data reader SQL type
        /// </summary>
        /// <example>"GetSqlInt64"</example>
        SqlDataReaderSqlType,

        /// <summary>
        /// The database type enum
        /// </summary>
        /// <example>"Int64"</example>
        DbTypeEnum,

        /// <summary>
        /// The SQL data reader database type
        /// </summary>
        /// <example>"GetInt64"</example>
        SqlDataReaderDbType
    }

    public enum DataTypes
    {
        @bigint,
        @binary,
        @bit,
        @char,
        @date,
        @datetime,
        @datetime2,
        @datetimeoffset,
        @decimal,
        @float,
        @image,
        @int,
        @money,
        @nchar,
        @ntext,
        @numeric,
        @nvarchar,
        @real,
        @rowversion,
        @smalldatetime,
        @smallint,
        @smallmoney,
        @sql_variant,
        @text,
        @time,
        @timestamp,
        @tinyint,
        @uniqueidentifier,
        @varbinary,
        @varchar,
        @xml
    }
}
