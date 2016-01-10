using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Concurrency
{
    public class ConcurrencyTokenNotAppliedException : ConcurrencyException
    {
        public ConcurrencyTokenNotAppliedException(IHasConcurrencyToken obj, string message = null)
            :base(obj, message)
        {
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

                return string.Format("Concurrency token is not applied on '{0}'. {1}",
                    base.Object.ToString(), additionalInfo);
            }
        }
    }
}
