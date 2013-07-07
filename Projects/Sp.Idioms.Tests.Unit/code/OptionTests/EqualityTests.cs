using NUnit.Framework;

namespace Sp.Idioms.Tests.Unit.OptionTests
{
  [TestFixture]
  public class EqualityTests
  {
    [Test]
    public void None_OfSameValueType_AreEqual()
    {
      Assert.AreEqual(Option<string>.None, Option<string>.None);
    }

    [Test]
    public void None_OfSameValueType_AreSame()
    {
      Assert.That(Option<string>.None == Option<string>.None);
    }

    [Test]
    public void None_OfDifferentValueType_AreNotEqual()
    {
      Assert.AreNotEqual(Option<string>.None, Option<TestClass>.None);
    }

    [Test]
    public void None_IsNotEqualTo_Some()
    {
      Assert.AreNotEqual(Option<string>.None, Option<string>.Some("Some"));
    }

    [Test]
    public void Some_IsNotEqualTo_SomeOfOtherValueType()
    {
      Assert.AreNotEqual(Option<string>.Some("Value1"), Option<TestClass>.Some(new TestClass()));
    }

    [Test]
    public void Some_IsNotEqualTo_SomeWithNotEqualValue()
    {
      Assert.AreNotEqual(Option<string>.Some("Value1"), Option<string>.Some("Value2"));
    }

    [Test]
    public void Some_IsEqualTo_SomeWithEqualValue()
    {
      Assert.AreEqual(Option<string>.Some("Value"), Option<string>.Some("Value"));
    }
  }
}
