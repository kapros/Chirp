using System.Threading.Tasks;

namespace PostsEventProcessor
{
    public interface IEventConsumerService
    {
        Task Subscribe();
    }
}