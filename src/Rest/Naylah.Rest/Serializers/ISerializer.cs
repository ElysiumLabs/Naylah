using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Rest.Serializers
{
    public interface ISerializer
    {
        string ContentType { get; }

        Task<string> Serialize<T>(T value);
        Task<T> Deserialize<T>(string svalue);

        Task SerializeToStream<T>(T value, StreamWriter streamWriter);
        Task<T> DeserializeFromStream<T>(StreamReader streamReader);
    }

    
}
