using System;
using NUnit.Framework;

namespace Sp.Idioms.Tests.Unit.ValueOptionTests
{
  [TestFixture]
  public class RetrievalTests : TestsBase
  {
    private ValueOption<int> Some { get; set; }
    private int SomeValue { get; set; }
    private int SomeOtherValue { get; set; }

    [SetUp]
    public void Setup()
    {
      SomeValue = 1;
      Some = ValueOption<int>.Some(SomeValue);
      SomeOtherValue = 2;
    }

    [Test]
    public void Value_WithNone_ThrowsInvalidOperationException()
    {
      Assert.That(() => None.Value, Throws.InstanceOf<InvalidOperationException>());
    }

    [Test]
    public void Value_WithSome_ReturnsValue()
    {
      Assert.That(Some.Value, Is.EqualTo(SomeValue));
    }

    [Test]
    public void TryGetValue_WithNone_ReturnsFalse()
    {
      int value;
      Assert.That(None.TryGetValue(out value), Is.False);
      Assert.That(value, Is.EqualTo(0));
    }

    [Test]
    public void TryGetValue_WithSome_ReturnsTrue()
    {
      int value;
      Assert.That(Some.TryGetValue(out value));
      Assert.That(value, Is.EqualTo(SomeValue));
    }

    [Test]
    public void ValueOrDefault_WithNone_ReturnsDefault()
    {
      Assert.That(None.ValueOrDefault, Is.EqualTo(0));
    }

    [Test]
    public void ValueOrDefault_WithSome_ReturnsSomeValue()
    {
      Assert.That(Some.ValueOrDefault, Is.EqualTo(SomeValue));
    }

    [Test]
    public void ValueOr_WithNone_ReturnsDefault()
    {
      Assert.That(None.ValueOr(SomeOtherValue), Is.EqualTo(SomeOtherValue));
    }

    [Test]
    public void ValueOr_WithSome_ReturnsSomeValue()
    {
      Assert.That(Some.ValueOr(SomeOtherValue), Is.EqualTo(SomeValue));
    }

    [Test]
    public void SelectOrDefault_WithNone_ReturnsDefault()
    {
      Assert.That(None.SelectOrDefault(x => true), Is.EqualTo(false));
    }

    [Test]
    public void SelectOrDefault_WithDefaultSelected_ReturnsDefault()
    {
      Assert.That(Some.SelectOrDefault(x => false), Is.EqualTo(false));
    }

    [Test]
    public void SelectOrDefault_WithNotDefaultSelected_ReturnsNotDefault()
    {
      Assert.That(Some.SelectOrDefault(x => true), Is.EqualTo(true));
    }

    [Test]
    public void SelectOr_WithNone_ReturnsDefault()
    {
      Assert.That(None.SelectOr(x => 'a', 'b'), Is.EqualTo('b'));
    }

    [Test]
    public void SelectOr_WithDefaultSelected_ReturnsDefault()
    {
      Assert.That(Some.SelectOr(x => false, false), Is.EqualTo(false));
    }

    [Test]
    public void SelectOr_WithNotDefaultSelected_ReturnsNotDefault()
    {
      Assert.That(Some.SelectOr(x => 'a', 'b'), Is.EqualTo('a'));
    }

    [Test]
    public void SelectOrNone_WithNone_ReturnsNone()
    {
      CheckIsNone(None.SelectOrNone(x => "TestString"));
    }

    [Test]
    public void SelectOrNone_WithSome_ReturnsSome()
    {
      CheckIsSomeOfValue(Some.SelectOrNone(x => "TestString"), "TestString");
    }

    [Test]
    public void SelectOrNone_WithOptionSelector_WithNone_ReturnsNone()
    {
      CheckIsNone(None.SelectOrNone(x => Option<string>.Some("TestString")));
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
      CheckIsNone(None.SelectValueTypeOrNone(x => true));
    }

    [Test]
    public void SelectValueTypeOrNone_WithSome_ReturnsSome()
    {
      CheckIsSomeOfValue(Some.SelectValueTypeOrNone(x => true), true);
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
    public void ValueOrThrow_WithNone_ThrowsException()
    {
      var exception = new MyException();
      Assert.That(() => None.ValueOrThrow(exception), Throws.TypeOf(exception.GetType()));
    }

    [Test]
    public void ValueOrThrow_WithSome_ReturnsValue()
    {
      Assert.That(Some.ValueOrThrow(new MyException()), Is.EqualTo(SomeValue));
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
      Assert.That(Some.ValueOrThrowUsing(() => { throw new MyException(); }), Is.EqualTo(SomeValue));
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
      var value = SomeValue - 1;
      Some.IfHasValue(x => value = SomeValue);
      Assert.That(value, Is.EqualTo(SomeValue));
    }

    private static ValueOption<int> None
    {
      get { return ValueOption<int>.None; }
    }

    private class MyException : Exception { }
  }
}
