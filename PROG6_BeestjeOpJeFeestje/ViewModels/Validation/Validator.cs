using System.Diagnostics.CodeAnalysis;

namespace PROG6_BeestjeOpJeFeestje.ViewModels.Validation
{
	[ExcludeFromCodeCoverage]

	public static class Validator
    {

        public static bool ExcludeDesert(DateTime dateTime)
        {
            return dateTime.Month is >= 10 or <= 2;
        }

        public static bool ExcludeSnow(DateTime dateTime)
        {
            return dateTime.Month is >= 6 and <= 10;
        }

        public static bool IsWeekend(DateTime dateTime)
        {
            return dateTime.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
        }
        
    }
}
