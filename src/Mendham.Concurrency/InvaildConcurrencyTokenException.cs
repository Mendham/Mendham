namespace Mendham.Concurrency
{
    public class InvaildConcurrencyTokenException : ConcurrencyException
    {
        public IConcurrencyToken Expected { get; private set; }
        public IConcurrencyToken Actual { get; private set; }

        public InvaildConcurrencyTokenException(IHasConcurrencyToken obj, IConcurrencyToken expected, IConcurrencyToken actual, string message = null)
            : base(obj, message)
        {
            this.Expected = expected;
            this.Actual = actual;
        }

        public override string Message
        {
            get
            {
                string additionalInfo = string.Empty;

                if (!string.IsNullOrWhiteSpace(base.Message))
                {
                    additionalInfo = $" ADDITIONAL INFORMATION: {base.Message}";
                }

                return string.Format("Concurrency exception on '{0}'. Expected: '{1}', Actual: '{2}'.{3}",
                    this.Object.ToString(),
                    this.Expected,
                    this.Actual,
                    additionalInfo);
            }
        }
    }
}
