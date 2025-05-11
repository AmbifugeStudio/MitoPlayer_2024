using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Helpers
{
    public static class DbReaderHelper
    {
        public static int ReadInt(this SQLiteDataReader reader, string columnName, int defaultValue = 0)
        {
            return reader.IsDBNull(reader.GetOrdinal(columnName)) ? defaultValue : Convert.ToInt32(reader[columnName]);
        }

        public static long ReadLong(this SQLiteDataReader reader, string columnName, long defaultValue = 0)
        {
            return reader.IsDBNull(reader.GetOrdinal(columnName)) ? defaultValue : Convert.ToInt64(reader[columnName]);
        }

        public static string ReadString(this SQLiteDataReader reader, string columnName, string defaultValue = "")
        {
            return reader.IsDBNull(reader.GetOrdinal(columnName)) ? defaultValue : Convert.ToString(reader[columnName]);
        }

        public static bool ReadBool(this SQLiteDataReader reader, string columnName, bool defaultValue = false)
        {
            return reader.IsDBNull(reader.GetOrdinal(columnName)) ? defaultValue : Convert.ToBoolean(reader[columnName]);
        }

        public static double ReadDouble(this SQLiteDataReader reader, string columnName, double defaultValue = 0.0)
        {
            return reader.IsDBNull(reader.GetOrdinal(columnName)) ? defaultValue : Convert.ToDouble(reader[columnName]);
        }

        public static int? ReadNullableInt(this SQLiteDataReader reader, string columnName)
        {
            return reader.IsDBNull(reader.GetOrdinal(columnName)) ? (int?)null : Convert.ToInt32(reader[columnName]);
        }

        public static string ReadNullableString(this SQLiteDataReader reader, string columnName)
        {
            return reader.IsDBNull(reader.GetOrdinal(columnName)) ? null : Convert.ToString(reader[columnName]);
        }

        public static bool? ReadNullableBool(this SQLiteDataReader reader, string columnName)
        {
            return reader.IsDBNull(reader.GetOrdinal(columnName)) ? (bool?)null : Convert.ToBoolean(reader[columnName]);
        }

        public static decimal ReadDecimal(this SQLiteDataReader reader, string columnName, decimal defaultValue = 0.0m)
        {
            return reader.IsDBNull(reader.GetOrdinal(columnName)) ? defaultValue : Convert.ToDecimal(reader[columnName]);
        }

        public static DateTime ReadDateTime(this SQLiteDataReader reader, string columnName, DateTime? defaultValue = null)
        {
            int ordinal = reader.GetOrdinal(columnName);

            if (reader.IsDBNull(ordinal))
                return defaultValue ?? DateTime.MinValue;

            object value = reader[ordinal];

            // DateTime type
            if (value is DateTime dt)
                return dt;

            // ISO 8601 string date
            if (value is string str && DateTime.TryParse(str, out DateTime parsed))
                return parsed;

            // Unix timestamp (bigint or integer)
            if (long.TryParse(value.ToString(), out long timestamp))
            {
                try
                {
                    return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
                }
                catch
                {
                    // too large or too small number
                    return defaultValue ?? DateTime.MinValue;
                }
            }

            return defaultValue ?? DateTime.MinValue;
        }
    }
}
