namespace Mendham.Events.DependencyInjection.SharedHandlerTestObjects
{
    public class SharedHandlerTracker
    {
        public bool WasEvent1Called { get; private set; } = false;
        public bool WasEvent2Called { get; private set; } = false;

        public void Event1Called()
        {
            WasEvent1Called = true;
        }

        public void Event2Called()
        {
            WasEvent2Called = true;
        }
    }
}