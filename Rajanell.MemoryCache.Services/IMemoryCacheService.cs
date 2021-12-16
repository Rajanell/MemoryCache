using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rajanell.MemoryCache.Services
{
    public interface IMemoryCacheService
    {
        void AddRecord<T>(string recordId, T data);
        T GetRecord<T>(string recordId);
    }
}
