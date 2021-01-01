using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeroFormatter;

namespace Chirp.Services
{
    public class ZeroFormatterSerializationService : ISerializationService
    {
        public async Task<T> Deserialize<T>(string serialized)
        {
            var bytes = ISerializationService.Encoding.GetBytes(serialized);
            var deserialized = ZeroFormatterSerializer.Deserialize<T>(bytes);
            return deserialized;
        }

        public async Task<string> GetString(string obj)
        {
            var encoding = ISerializationService.Encoding;
            var bytes = encoding.GetBytes(obj);
            return encoding.GetString(bytes);
        }

        public async Task<string> Serialize<T>(T data)
        {
            var serialized = ZeroFormatterSerializer.Serialize(data);
            var stringified = ISerializationService.Encoding.GetString(serialized);
            return stringified;
        }
    }
}
