using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PostsEventProcessor
{
    public interface ILogger
    {
        Task LogExceptionAsync(Exception ex);
    }
}
