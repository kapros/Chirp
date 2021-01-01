using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chirp.Services
{
    public interface ISerializationService
    {
        static Encoding Encoding => Encoding.UTF8;

        Task<string> Serialize<T>(T data);

        Task<T> Deserialize<T>(string serialized);

        Task<string> GetString(string obj);
    }
}
