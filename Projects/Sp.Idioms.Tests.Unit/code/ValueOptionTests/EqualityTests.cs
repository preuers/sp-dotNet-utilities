using NUnit.Framework;

namespace Sp.Idioms.Tests.Unit.ValueOptionTests
{
  [TestFixture]
  public class EqualityTests
  {
    [Test]
    public void None_OfSameValueType_AreEqual()
    {
      Assert.AreEqual(ValueOption<int>.None, ValueOption<int>.None);
    }

    [Test]
    public void None_OfDifferentValueType_AreNotEqual()
    {
      Assert.AreNotEqual(ValueOption<int>.None, ValueOption<bool>.None);
    }

    [Test]
    public void None_IsNotEqualTo_Some()
    {
      Assert.AreNotEqual(ValueOption<int>.None, ValueOption<int>.Some(0));
    }

    [Test]
    public void Some_IsNotEqualTo_SomeOfOtherValueType()
    {
      Assert.AreNotEqual(ValueOption<int>.Some(0), ValueOption<bool>.Some(false));
    }

    [Test]
    public void Some_IsNotEqualTo_SomeWithNotEqualValue()
    {
      Assert.AreNotEqual(ValueOption<bool>.Some(false), ValueOption<bool>.Some(true));
    }

    [Test]
    public void Some_IsEqualTo_SomeWithEqualValue()
    {
      Assert.AreEqual(ValueOption<bool>.Some(true), ValueOption<bool>.Some(true));
    }
  }
}
