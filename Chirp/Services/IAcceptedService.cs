using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Services
{
    public interface IAcceptedService
    {
        Task<object> GetIdOfCompletedJob(string id);

        Task<bool> IsJobFinished(string id);
    }
}
