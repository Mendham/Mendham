namespace Mendham.Events.DependencyInjection.TestObjects
{
    public interface IVerifiableEventHandlerLogger
    {
        bool CompleteCalled { get; }
        bool StartCalled { get; }
    }
}