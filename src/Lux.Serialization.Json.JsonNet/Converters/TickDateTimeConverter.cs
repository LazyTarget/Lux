using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lux.Serialization.Json.JsonNet
{
    /// <summary>
    /// Converts a <see cref="DateTime"/> to and from a JavaScript tick integer value (e.g. 52231943).
    /// </summary>
    public class TickDateTimeConverter : DateTimeConverterBase
    {
        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            long ticks;

            if (value is DateTime)
            {
                DateTime dateTime = (DateTime)value;
                DateTime utcDateTime = dateTime.ToUniversalTime();
                ticks = DateTimeUtils.ConvertDateTimeToJavaScriptTicks(utcDateTime);
            }
#if !NET20
            else if (value is DateTimeOffset)
            {
                DateTimeOffset dateTimeOffset = (DateTimeOffset)value;
                DateTimeOffset utcDateTimeOffset = dateTimeOffset.ToUniversalTime();
                ticks = DateTimeUtils.ConvertDateTimeToJavaScriptTicks(utcDateTimeOffset.UtcDateTime);
            }
#endif
            else
            {
                throw new JsonSerializationException("Expected date object value.");
            }

            //writer.WriteStartConstructor("Date");
            writer.WriteValue(ticks);
            //writer.WriteEndConstructor();
        }


        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing property value of the JSON that is being converted.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
#if !NET20
            Type t = (ReflectionUtils.IsNullableType(objectType))
                ? Nullable.GetUnderlyingType(objectType)
                : objectType;
#endif

            if (reader.TokenType == JsonToken.Null)
            {
                if (!ReflectionUtils.IsNullable(objectType))
                    throw new JsonSerializationException(string.Format("Cannot convert null value to {0}", objectType));
                //throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, objectType));

                return null;
            }

            //if (reader.TokenType != JsonToken.StartConstructor || !string.Equals(reader.Value.ToString(), "Date", StringComparison.Ordinal))
            //    throw new JsonSerializationException(string.Format("Unexpected token or value when parsing data. Token: {0}, Value: {1}", reader.TokenType, reader.Value));
            //    //throw JsonSerializationException.Create(reader, "Unexpected token or value when parsing date. Token: {0}, Value: {1}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType, reader.Value));

            //reader.Read();

            if (reader.TokenType != JsonToken.Integer)
                throw new JsonSerializationException(string.Format("Unexpected token parsing date. Expected Integer, got {0}.", reader.TokenType));
            //throw JsonSerializationException.Create(reader, "Unexpected token parsing date. Expected Integer, got {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));

            long ticks = (long)reader.Value;

            DateTime d = DateTimeUtils.ConvertJavaScriptTicksToDateTime(ticks);

            //reader.Read();

            //if (reader.TokenType != JsonToken.EndConstructor)
            //    throw new JsonSerializationException(string.Format("Unexpected token parsing date. Expected EndConstructor, got {0}.", reader.TokenType));
            //    //throw JsonSerializationException.Create(reader, "Unexpected token parsing date. Expected EndConstructor, got {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));

#if !NET20
            if (t == typeof(DateTimeOffset))
                return new DateTimeOffset(d);
#endif
            return d;
        }




        internal static class DateTimeUtils
        {
            internal static readonly long InitialJavaScriptDateTicks = 621355968000000000;

            static DateTimeUtils()
            {

            }


            public static TimeSpan GetUtcOffset(DateTime d)
            {
#if NET20
            return TimeZone.CurrentTimeZone.GetUtcOffset(d);
#else
                return TimeZoneInfo.Local.GetUtcOffset(d);
#endif
            }

            internal static DateTime EnsureDateTime(DateTime value, DateTimeZoneHandling timeZone)
            {
                switch (timeZone)
                {
                    case DateTimeZoneHandling.Local:
                        value = SwitchToLocalTime(value);
                        break;
                    case DateTimeZoneHandling.Utc:
                        value = SwitchToUtcTime(value);
                        break;
                    case DateTimeZoneHandling.Unspecified:
                        value = new DateTime(value.Ticks, DateTimeKind.Unspecified);
                        break;
                    case DateTimeZoneHandling.RoundtripKind:
                        break;
                    default:
                        throw new ArgumentException("Invalid date time handling value.");
                }

                return value;
            }

            private static DateTime SwitchToLocalTime(DateTime value)
            {
                switch (value.Kind)
                {
                    case DateTimeKind.Unspecified:
                        return new DateTime(value.Ticks, DateTimeKind.Local);

                    case DateTimeKind.Utc:
                        return value.ToLocalTime();

                    case DateTimeKind.Local:
                        return value;
                }
                return value;
            }

            private static DateTime SwitchToUtcTime(DateTime value)
            {
                switch (value.Kind)
                {
                    case DateTimeKind.Unspecified:
                        return new DateTime(value.Ticks, DateTimeKind.Utc);

                    case DateTimeKind.Utc:
                        return value;

                    case DateTimeKind.Local:
                        return value.ToUniversalTime();
                }
                return value;
            }

            private static long ToUniversalTicks(DateTime dateTime)
            {
                if (dateTime.Kind == DateTimeKind.Utc)
                    return dateTime.Ticks;

                return ToUniversalTicks(dateTime, GetUtcOffset(dateTime));
            }

            private static long ToUniversalTicks(DateTime dateTime, TimeSpan offset)
            {
                // special case min and max value
                // they never have a timezone appended to avoid issues
                if (dateTime.Kind == DateTimeKind.Utc || dateTime == DateTime.MaxValue || dateTime == DateTime.MinValue)
                    return dateTime.Ticks;

                long ticks = dateTime.Ticks - offset.Ticks;
                if (ticks > 3155378975999999999L)
                    return 3155378975999999999L;

                if (ticks < 0L)
                    return 0L;

                return ticks;
            }

            internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime, TimeSpan offset)
            {
                long universialTicks = ToUniversalTicks(dateTime, offset);
                return UniversialTicksToJavaScriptTicks(universialTicks);
            }

            internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime)
            {
                return ConvertDateTimeToJavaScriptTicks(dateTime, true);
            }

            internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime, bool convertToUtc)
            {
                long ticks = (convertToUtc) ? ToUniversalTicks(dateTime) : dateTime.Ticks;
                return UniversialTicksToJavaScriptTicks(ticks);
            }

            private static long UniversialTicksToJavaScriptTicks(long universialTicks)
            {
                long javaScriptTicks = (universialTicks - InitialJavaScriptDateTicks) / 10000;
                return javaScriptTicks;
            }

            internal static DateTime ConvertJavaScriptTicksToDateTime(long javaScriptTicks)
            {
                DateTime dateTime = new DateTime((javaScriptTicks * 10000) + InitialJavaScriptDateTicks, DateTimeKind.Utc);
                return dateTime;
            }

        }


        internal static class ReflectionUtils
        {
            public static Type GetObjectType(object v)
            {
                return (v != null) ? v.GetType() : null;
            }

            public static bool IsNullable(Type t)
            {
                if (t == null)
                    throw new ArgumentNullException("t");
                if (t.IsValueType)
                    return IsNullableType(t);
                return true;
            }

            public static bool IsNullableType(Type t)
            {
                if (t == null)
                    throw new ArgumentNullException("t");
                return (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
            }

        }

    }
}
