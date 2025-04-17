using System.Text.Json.Serialization;
using System.Text.Json;

namespace ShopComputer.Extensions
{
    public static class MySessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true,
            };
            string srt = JsonSerializer.Serialize(value, options);
            session.SetString(key, srt);
        }


        public static T? Get<T>(this ISession session, string key)
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true,
            };
            T? item = default;
            string? str = session.GetString(key);
            if (str != null)
                item = JsonSerializer.Deserialize<T>(str, options);
            return item;
        }
    }
}
