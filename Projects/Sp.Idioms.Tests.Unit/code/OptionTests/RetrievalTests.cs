using System;
using NUnit.Framework;

namespace Sp.Idioms.Tests.Unit.OptionTests
{
  [TestFixture]
  public class RetrievalTests : TestsBase
  {
    private Option<TestClass> Some { get; set; }
    private TestClass SomeValue { get; set; }
    private TestClass SomeOtherValue { get; set; }

    [SetUp]
    public void Setup()
    {
      SomeValue = new TestClass();
      Some = Option<TestClass>.Some(SomeValue);
      SomeOtherValue = new TestClass();
    }

    [Test]
    public void Value_WithNone_ThrowsInvalidOperationException()
    {
      Assert.That(() => None.Value, Throws.InstanceOf<InvalidOperationException>());
    }

    [Test]
    public void Value_WithSome_ReturnsValue()
    {
      Assert.That(Some.Value, Is.SameAs(SomeValue));
    }

    [Test]
    public void ValueAs_WithNone_ThrowsInvalidOperationException()
    {
      Assert.That(() => None.ValueAs<object>(), Throws.InstanceOf<InvalidOperationException>());
    }

    [Test]
    public void ValueAs_WithSome_ReturnsValue()
    {
      Assert.That(Some.ValueAs<object>(), Is.SameAs(SomeValue));
    }

    [Test]
    public void ValueAs_WithInvalidType_ThrowsException()
    {
      Assert.That(() => Some.ValueAs<OtherClass>(), Throws.Exception);
    }

    [Test]
    public void TryGetValue_WithNone_ReturnsFalse()
    {
      TestClass value;
      Assert.That(None.TryGetValue(out value), Is.False);
      Assert.That(value, Is.Null);
    }

    [Test]
    public void TryGetValue_WithSome_ReturnsTrue()
    {
      TestClass value;
      Assert.That(Some.TryGetValue(out value));
      Assert.That(value, Is.SameAs(SomeValue));
    }

    [Test]
    public void TryGetValueAs_WithNone_ReturnsFalse()
    {
      object value;
      Assert.That(None.TryGetValueAs<object>(out value), Is.False);
      Assert.That(value, Is.Null);
    }

    [Test]
    public void TryGetValueAs_WithSome_ReturnsTrue()
    {
      object value;
      Assert.That(Some.TryGetValueAs<object>(out value));
      Assert.That(value, Is.SameAs(SomeValue));
    }

    [Test]
    public void ValueOrNull_WithNone_ReturnsNull()
    {
      Assert.That(None.ValueOrNull, Is.Null);
    }

    [Test]
    public void ValueOrNull_WithSome_ReturnsSomeValue()
    {
      Assert.That(Some.ValueOrNull, Is.SameAs(SomeValue));
    }

    [Test]
    public void ValueOr_WithNone_ReturnsDefault()
    {
      Assert.That(None.ValueOr(SomeOtherValue), Is.SameAs(SomeOtherValue));
    }

    [Test]
    public void ValueOr_WithSome_ReturnsSomeValue()
    {
      Assert.That(Some.ValueOr(SomeOtherValue), Is.SameAs(SomeValue));
    }

    [Test]
    public void SelectOrDefault_WithNone_ReturnsDefault()
    {
      Assert.That(None.SelectOrDefault(x => x.ToString()), Is.Null);
    }

    [Test]
    public void SelectOrDefault_WithDefaultSelected_ReturnsDefault()
    {
      Assert.That(Some.SelectOrDefault<string>(x => null), Is.Null);
    }

    [Test]
    public void SelectOrDefault_WithNotDefaultSelected_ReturnsNotDefault()
    {
      Assert.That(Some.SelectOrDefault(x => "TestString"), Is.SameAs("TestString"));
    }

    [Test]
    public void SelectOr_WithNone_ReturnsDefault()
    {
      Assert.That(None.SelectOr(x => x.ToString(), "TestString"), Is.SameAs("TestString"));
    }

    [Test]
    public void SelectOr_WithDefaultSelected_ReturnsDefault()
    {
      Assert.That(Some.SelectOr(x => null, "TestString"), Is.Null);
    }

    [Test]
    public void SelectOr_WithNotDefaultSelected_ReturnsNotDefault()
    {
      Assert.That(Some.SelectOr(x => "TestString", "DefaultTestString"), Is.SameAs("TestString"));
    }

    [Test]
    public void SelectOrNone_WithNone_ReturnsNone()
    {
      CheckIsNone(None.SelectOrNone(x => x.ToString()));
    }

    [Test]
    public void SelectOrNone_WithSome_ReturnsSome()
    {
      CheckIsSomeOfValue(Some.SelectOrNone(x => "TestString"), "TestString");
    }

    [Test]
    public void SelectOrNone_WithOptionSelector_WithNone_ReturnsNone()
    {
      CheckIsNone(None.SelectOrNone(x => Option<string>.Some(x.ToString())));
    }

    [Test]
    public void SelectOrNone_WithSome_ReturnsSelectedOption()
    {
      var option = Option<string>.Some("TestString");
      Assert.That(Some.SelectOrNone(x => option), Is.EqualTo(option));
    }

    [Test]
    public void SelectValueTypeOrNone_WithNone_ReturnsNone()
    {
      CheckIsNone(None.SelectValueTypeOrNone(x => 0));
    }

    [Test]
    public void SelectValueTypeOrNone_WithSome_ReturnsSome()
    {
      CheckIsSomeOfValue(Some.SelectValueTypeOrNone(x => 0), 0);
    }

    [Test]
    public void SelectValueTyoeOrNone_WithValueOptionSelector_WithNone_ReturnsNone()
    {
      CheckIsNone(None.SelectValueTypeOrNone(x => ValueOption<bool>.Some(true)));
    }

    [Test]
    public void SelectValueTypeOrNone_WithSome_ReturnsSelectedValueOption()
    {
      var valueOption = ValueOption<bool>.Some(true);
      Assert.That(Some.SelectValueTypeOrNone(x => valueOption), Is.EqualTo(valueOption));
    }

    [Test]
    public void ToOption_WithNone_ReturnsNone()
    {
      CheckIsNone(None.ToOption<object>());
    }

    [Test]
    public void ToOption_WithSome_AndProperType_ReturnsSome()
    {
      CheckIsSomeOfValue(Some.ToOption<object>(), SomeValue);
    }

    [Test]
    public void ToOption_WithSome_AndImproperType_ThrowsException()
    {
      Assert.That(() => Some.ToOption<OtherClass>(), Throws.Exception);
    }

    [Test]
    public void ValueOrThrow_WithNone_ThrowsException()
    {
      var exception = new MyException();
      Assert.That(() => None.ValueOrThrow(exception), Throws.TypeOf(exception.GetType()));
    }

    [Test]
    public void ValueOrThrow_WithSome_ReturnsValue()
    {
      Assert.That(Some.ValueOrThrow(new MyException()), Is.SameAs(SomeValue));
    }

    [Test]
    public void ValueOrThrowUsing_WithNone_ThrowsException()
    {
      var exception = new MyException();
      Action throwException = () => { throw exception; };
      Assert.That(() => None.ValueOrThrowUsing(throwException), Throws.TypeOf(exception.GetType()));
    }

    [Test]
    public void ValueOrThrowUsing_WithSome_ThrowsException()
    {
      Assert.That(Some.ValueOrThrowUsing(() => { throw new MyException(); }), Is.SameAs(SomeValue));
    }

    [Test]
    public void ValueOrThrowUsing_WithNone_AndActionNotThrowingAnException_ThrowsArgumentException()
    {
      Assert.That(() => None.ValueOrThrowUsing(() => { }), Throws.ArgumentException);
    }

    [Test]
    public void IfHasValue_WithNone_DoesNotCallAction()
    {
      var called = false;
      None.IfHasValue(x => called = true);
      Assert.That(called, Is.False);
    }

    [Test]
    public void IfHasValue_WithSome_CallsActionWithValue()
    {
      TestClass value = null;
      Some.IfHasValue(x => value = SomeValue);
      Assert.That(value, Is.SameAs(SomeValue));
    }

    private static Option<TestClass> None
    {
      get { return Option<TestClass>.None; }
    }

    private class OtherClass {}

    private class MyException : Exception {}
  }
}
