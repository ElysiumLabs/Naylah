using System;

namespace Naylah
{
    public class DateTimeZone : IEquatable<DateTimeZone>, IEquatable<DateTime>
    {
        private const string defaultTimeZoneInfoId = "UTC";

        public DateTimeZone(DateTime dateTime, string timeZoneInfoId = defaultTimeZoneInfoId)
        {
            if (timeZoneInfoId == defaultTimeZoneInfoId)
            {
                DateTime = new DateTime(dateTime.Ticks, DateTimeKind.Utc);
            }
            else
            {
                DateTime = new DateTime(dateTime.Ticks, DateTimeKind.Unspecified);
            }

            TimeZoneInfoId = timeZoneInfoId;
        }

        public DateTimeZone()
        {
            TimeZoneInfoId = defaultTimeZoneInfoId;
        }

        //[JsonProperty]
        public DateTime DateTime { get; protected set; }

        public string TimeZoneInfoId { get; protected set; }

        public TimeZoneInfo TimeZoneInfo 
        { 
            get 
            {
#if NETSTANDARD2_0
                return TimeZoneInfo.FindSystemTimeZoneById(TimeZoneInfoId); 
#else
                return null; // TODO FIX;
#endif
            } 
        }

#region Cast operators

        public static implicit operator DateTimeZone(DateTime dateTime)
        {
            return new DateTimeZone(dateTime);
        }

        public static implicit operator DateTime(DateTimeZone dateTimeWithZone)
        {
            return dateTimeWithZone.DateTime;
        }

#endregion Cast operators

#region Condition operators, IEqualitable

        public static bool operator ==(DateTimeZone left,
                                     DateTimeZone right)
        {
            if (Object.ReferenceEquals(left, null)) return false;
            else return left.Equals(right);
        }

        public static bool operator !=(DateTimeZone left,
                                        DateTimeZone right)
        {
            return !(left == right);
        }

        public bool Equals(DateTimeZone other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.DateTime, DateTime) && other.TimeZoneInfoId == TimeZoneInfoId;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DateTimeZone)) return false;
            return Equals((DateTimeZone)obj);
        }

        public override int GetHashCode()
        {
            const int prime = 397;
            int result = DateTime.GetHashCode();
            result = (result * prime) ^ (TimeZoneInfoId != null ? TimeZoneInfoId.GetHashCode() : 0);
            return result;
        }

#region DateTime comparation

        public static bool operator ==(DateTimeZone left,
                                    DateTime right)
        {
            if (Object.ReferenceEquals(left, null)) return false;
            else return left.Equals(right);
        }

        public static bool operator !=(DateTimeZone left,
                                        DateTime right)
        {
            return !(left == right);
        }

        public static bool operator ==(DateTime left,
                                    DateTimeZone right)
        {
            if (Object.ReferenceEquals(left, null)) return false;
            else return right.Equals(left);
        }

        public static bool operator !=(DateTime left,
                                        DateTimeZone right)
        {
            return !(left == right);
        }

        public bool Equals(DateTime other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other, DateTime);
        }

#endregion DateTime comparation

#endregion Condition operators, IEqualitable
    }
}