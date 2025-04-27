using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RB.Core.Model.Enums
{
  public enum Month
  {
    YearRound = 0,
    January = 1,
    February = 2,
    March = 3,
    April = 4,
    May = 5,
    June = 6,
    July = 7,
    August = 8,
    September = 9,
    October = 10,
    November = 11,
    December = 12,
  }

  public static class MonthStrings
  {
    public static string FromEnum(Month month)
    {
      switch (month)
      {
        case Month.YearRound:
          return nameof(Month.YearRound);
        case Month.January: 
          return nameof(Month.January);
        case Month.February: 
          return nameof(Month.February);
        case Month.March: 
          return nameof(Month.March);
        case Month.April: 
          return nameof(Month.April);
        case Month.May: 
          return nameof(Month.May);
        case Month.June: 
          return nameof(Month.June);
        case Month.July: 
          return nameof(Month.July);
        case Month.August: 
          return nameof(Month.August);
        case Month.September: 
          return nameof(Month.September);
        case Month.October: 
          return nameof(Month.October);
        case Month.November: 
          return nameof(Month.November);
        case Month.December: 
          return nameof(Month.December);
        default:
          throw new ArgumentOutOfRangeException("Enum lacks an associated label");
      }
    }
  }
}
