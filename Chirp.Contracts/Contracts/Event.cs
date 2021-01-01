namespace Chirp.Contracts
{
    public class Event<T>
    {
        public Event() { }

        public string Version { get; set; }

        public string EventName { get; set; }

        public T Message { get; set; }
    }
}