namespace Mendham.Testing.AspNetCore.Test.SampleApp
{
    public class Dependency1 : IDependency1
    {
        public const int ExpectedValue = 17;

        public int GetValue()
        {
            return ExpectedValue;
        }
    }
}
