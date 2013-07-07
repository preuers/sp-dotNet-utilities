using NUnit.Framework;

namespace Sp.Idioms.Tests.Unit.ValueOptionTests
{
  public class TestsBase
  {
    protected static void CheckIsSomeOfValue<TValue>(Option<TValue> option, TValue expectedValue)
      where TValue : class
    {
      Assert.That(option.HasValue);
      Assert.That(option.Value, Is.SameAs(expectedValue));
    }

    protected static void CheckIsNone<TValue>(Option<TValue> option)
      where TValue : class
    {
      Assert.That(option.HasValue, Is.False);
    }

    protected static void CheckIsNone<TValue>(ValueOption<TValue> option)
      where TValue : struct
    {
      Assert.That(option.HasValue, Is.False);
    }

    protected static void CheckIsSomeOfValue<TValue>(ValueOption<TValue> option, TValue expectedValue)
      where TValue : struct
    {
      Assert.That(option.HasValue);
      Assert.That(option.Value, Is.EqualTo(expectedValue));
    }
  }
}
