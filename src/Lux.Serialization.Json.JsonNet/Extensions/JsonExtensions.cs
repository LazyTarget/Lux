using Newtonsoft.Json.Linq;

namespace Lux.Serialization.Json.JsonNet
{
    public static class JsonExtensions
    {
        public static TToken SelectTokenOrDefault<TToken>(this JToken token, string path)
            where TToken : JToken
        {
            if (token == null)
                return null;
            var t = token.SelectToken(path);
            var res = t as TToken;
            return res;
        }


        public static TObj ToObjectOrDefault<TObj>(this JToken obj)
        {
            var res = ToObjectOrDefault<TObj>(obj, default(TObj));
            return res;
        }

        public static TObj ToObjectOrDefault<TObj>(this JToken obj, TObj defaultValue)
        {
            if (obj == null)
                return defaultValue;
            if (obj.Type == JTokenType.Null)
                return defaultValue;
            var res = obj.ToObject<TObj>();
            return res;
        }


        public static TValue GetPropertyValue<TValue>(this JObject obj, string propertyName)
        {
            if (obj == null)
                return default(TValue);
            var prop = obj.Property(propertyName);
            if (prop == null)
                return default(TValue);
            var res = ToObjectOrDefault<TValue>(prop.Value);
            return res;
        }
    }
}
