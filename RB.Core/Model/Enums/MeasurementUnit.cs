namespace RB.Core.Model.Enums
{
  public enum MeasurementUnit
  {
    Celsius = 0,
    Gram = 1,
    Liter = 2,
    Pinch = 3,
    Dash = 4,
    Unspecified = 5,
  }

  public static class MeasurementUnitString
  {
    public static string FromEnum(MeasurementUnit unit)
    {
      switch (unit)
      {
        case MeasurementUnit.Celsius:
          return nameof(MeasurementUnit.Celsius);
        case MeasurementUnit.Gram:
          return nameof(MeasurementUnit.Gram);
        case MeasurementUnit.Liter:
          return nameof(MeasurementUnit.Liter);
        case MeasurementUnit.Pinch:
          return nameof(MeasurementUnit.Pinch);
        case MeasurementUnit.Dash:
          return nameof(MeasurementUnit.Dash);
        case MeasurementUnit.Unspecified:
          return nameof(string.Empty);
        default:
          throw new ArgumentOutOfRangeException("Enum lacks an associated label");
      }
    }
  }
}
