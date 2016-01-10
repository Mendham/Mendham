using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Concurrency
{
    public class ConcurrencyTokenAlreadyAppliedException : ConcurrencyException
    {
        public IConcurrencyToken ExistingToken { get; private set; }

        public ConcurrencyTokenAlreadyAppliedException(IHasConcurrencyToken obj, IConcurrencyToken existingToken, string message = null)
            : base(obj, message)
        {
            this.ExistingToken = existingToken;
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

                return string.Format("Concurrency token already applied on '{0}'. Existing Token Value: '{1}'. {2}",
                    this.Object.ToString(),
                    this.ExistingToken,
                    additionalInfo);
            }
        }
    }
}
