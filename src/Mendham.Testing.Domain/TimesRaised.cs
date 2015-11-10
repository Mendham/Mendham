using Mendham;
using Mendham.Equality;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing
{
    public struct TimesRaised : IHasEqualityComponents, IEquatable<TimesRaised>
    {
        private string failureDetails;
        private int? minRaised;
        private int? maxRaised;

        private TimesRaised(string failureDetails, int? minRaised, int? maxRaised)
        {
            failureDetails.VerifyArgumentNotNullOrWhiteSpace("Failure details are required");

            this.failureDetails = failureDetails;
            this.minRaised = minRaised;
            this.maxRaised = maxRaised;
        }

        internal bool Validate(int actualTimesRaised)
        {
            if (minRaised.HasValue && minRaised.Value > actualTimesRaised)
                return false;

            if (maxRaised.HasValue && maxRaised.Value < actualTimesRaised)
                return false;

            return true;
        }

        internal string GetFailDetails()
        {
            var msg = string.Format(
                CultureInfo.CurrentCulture,
                this.failureDetails,
                this.minRaised,
                this.maxRaised);

            return msg;
        }

        IEnumerable<object> IHasEqualityComponents.EqualityComponents
        {
            get
            {
                yield return this.minRaised;
                yield return this.maxRaised;
            }
        }

        private const string FAIL_DETAILS_AT_LEAST = "at least {0} times";
        private const string FAIL_DETAILS_AT_LEAST_ONCE = "at least once";
        private const string FAIL_DETAILS_AT_MOST = "at most {1} times";
        private const string FAIL_DETAILS_AT_MOST_ONCE = "at most once";
        private const string FAIL_DETAILS_BETWEEN = "between {0} and {1} times (inclusive)";
        private const string FAIL_DETAILS_EXACTLY = "exactly {0} times";
        private const string FAIL_DETAILS_NEVER = "never";
        private const string FAIL_DETAILS_ONCE = "once and only once";

        public readonly static TimesRaised AtLeastOnce = new TimesRaised(FAIL_DETAILS_AT_LEAST_ONCE, 1, null);
        public readonly static TimesRaised AtMostOnce = new TimesRaised(FAIL_DETAILS_AT_MOST_ONCE, 1, null);
        public readonly static TimesRaised Once = new TimesRaised(FAIL_DETAILS_ONCE, 1, 1);
        public readonly static TimesRaised Never = new TimesRaised(FAIL_DETAILS_NEVER, 0, 0);

        public static TimesRaised AtLeast(int timesRaised)
        {
            timesRaised.VerifyArgumentMeetsCriteria(a => a > 0, "Times raised must be at least 1");

            if (timesRaised == 1)
                return AtLeastOnce;

            return new TimesRaised(FAIL_DETAILS_AT_LEAST, timesRaised, null);
        }

        public static TimesRaised AtMost(int timesRaised)
        {
            timesRaised.VerifyArgumentMeetsCriteria(a => a > 0, "Times raised must be at least 1");

            if (timesRaised == 1)
                return AtMostOnce;

            return new TimesRaised(FAIL_DETAILS_AT_MOST, timesRaised, null);
        }

        public static TimesRaised Between(int minTimesRaised, int maxTimesRaised)
        {
            minTimesRaised.VerifyArgumentMeetsCriteria(a => a >= 0,
                "Minimum times raised must be at least 0");
            maxTimesRaised.VerifyArgumentMeetsCriteria(a => a >= minTimesRaised,
                "Maximum time raised cannot be less than minimum times raised");

            if (minTimesRaised == maxTimesRaised)
                return Exactly(minTimesRaised);

            return new TimesRaised(FAIL_DETAILS_BETWEEN, minTimesRaised, maxTimesRaised);
        }

        public static TimesRaised Exactly(int timesRaised)
        {
            timesRaised.VerifyArgumentMeetsCriteria(a => a > 0, "Times raised must be at least 1");

            switch (timesRaised)
            {
                case 0: return Never;
                case 1: return Once;
                default: return new TimesRaised(FAIL_DETAILS_EXACTLY, timesRaised, timesRaised);
            }
        }

        public override bool Equals(object obj)
        {
            return this.EqualsFromComponents(obj);
        }

        public bool Equals(TimesRaised other)
        {
            return Equals(this, other);
        }

        public override int GetHashCode()
        {
            return this.GetHashCodeFromComponents();
        }

        public static bool operator ==(TimesRaised a, TimesRaised b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(TimesRaised a, TimesRaised b)
        {
            return !Equals(a, b);
        }

        public static implicit operator TimesRaised(int timesRaised)
        {
            return Exactly(timesRaised);
        }
    }
}
