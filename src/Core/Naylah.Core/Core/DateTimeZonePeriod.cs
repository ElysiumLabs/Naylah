using Naylah.Extensions;
using System;
using System.Collections.Generic;

namespace Naylah
{
    public static class DateTimeZonePeriodExtensions
    {
#if NETSTANDARD2_0
        public static void ConvertToTimeZone(this DateTimeZonePeriod durationTimeBox, string timeZoneInfoId)
        {

            var timeZoneInfoTo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneInfoId);


            durationTimeBox.Start = new DateTimeZone(
                TimeZoneInfo.ConvertTime(durationTimeBox.Start.DateTime, durationTimeBox.Start.TimeZoneInfo, timeZoneInfoTo), timeZoneInfoId
                );
            durationTimeBox.End = new DateTimeZone(
                TimeZoneInfo.ConvertTime(durationTimeBox.End.DateTime, durationTimeBox.End.TimeZoneInfo, timeZoneInfoTo), timeZoneInfoId
                );

        }

        public static void ConvertToTimeZone(this IEnumerable<DateTimeZonePeriod> durationTimeBoxes, string timeZoneInfoId)
        {
            durationTimeBoxes.ForEach(x => x.ConvertToTimeZone(timeZoneInfoId));
        }
#endif
    }

    public class DateTimeZonePeriod : IEquatable<DateTimeZonePeriod>
    {
        public DateTimeZonePeriod()
        {
            Start = new DateTimeZone();
            End = new DateTimeZone();
        }

        public DateTimeZone Start { get; set; }
        public DateTimeZone End { get; set; }

        public TimeSpan Duration
        {
            get
            {
                try
                {
                    return (End.DateTime - Start.DateTime).Duration();
                }
                catch (Exception)
                {
                    return TimeSpan.Zero;
                }
            }
        }

#region Condition operators, IEqualitable

        public static bool operator ==(DateTimeZonePeriod left,
                                     DateTimeZonePeriod right)
        {
            if (Object.ReferenceEquals(left, null)) return false;
            else return left.Equals(right);
        }

        public static bool operator !=(DateTimeZonePeriod left,
                                        DateTimeZonePeriod right)
        {
            return !(left == right);
        }

        public bool Equals(DateTimeZonePeriod other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Start, Start) && other.End == End;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DateTimeZonePeriod)) return false;
            return Equals((DateTimeZonePeriod)obj);
        }

        public override int GetHashCode()
        {
            const int prime = 397;
            int result = Start.GetHashCode() ^ End.GetHashCode();
            result = (result * prime) ^ (Start != null ? Start.GetHashCode() : 0);
            result = (result * prime) ^ (End != null ? End.GetHashCode() : 0);
            return result;
        }

#endregion Condition operators, IEqualitable
    }
}