using NUnit.Framework;

namespace Sp.Idioms.Tests.Unit.OptionTests
{
  [TestFixture]
  public class CreationTests : TestsBase
  {
    [Test]
    public void None_CreatesNone()
    {
      CheckIsNone(Option<string>.None);
    }

    [Test]
    public void Some_CreatesSome()
    {
      CheckIsSomeOfValue(Option<string>.Some("TestString"), "TestString");
    }

    [Test]
    public void FromValueOrNull_WithNull_CreatesNone()
    {
      CheckIsNone(Option<string>.FromValueOrNull(null));
    }

    [Test]
    public void FromValueOrNull_WithNotNull_CreatesSome()
    {
      CheckIsSomeOfValue(Option<string>.FromValueOrNull("TestString"), "TestString"); 
    }

    [Test]
    public void FromSelectOrNull_WithNullContext_CreatesNone()
    {
      CheckIsNone(Option<string>.FromSelectOrNull(NullTestClass, x => x.ToString()));
    }

    [Test]
    public void FromSelectOrNull_WithNullSelected_CreatesNone()
    {
      CheckIsNone(Option<string>.FromSelectOrNull(ArbritraryTestClass, x => null));
    }

    [Test]
    public void FromSelectOrNull_WithNotNullSelected_CreatesSome()
    {
      CheckIsSomeOfValue(Option<string>.FromSelectOrNull(ArbritraryTestClass, x => "TestString"), "TestString");
    }

    [Test]
    public void FromValueOrNullWithCondition_WithNull_CreatesNone()
    {
      CheckIsNone(Option<string>.FromValueOrNullWithCondition(null, x => true));
    }

    [Test]
    public void FromValueOrNullWithCondition_WithFalseCondition_CreatesNone()
    {
      CheckIsNone(Option<string>.FromValueOrNullWithCondition("TestString", x => false));
    }

    [Test]
    public void FromValueOrNullWithCondition_WithTrueCondition_CreatesNone()
    {
      CheckIsSomeOfValue(Option<string>.FromValueOrNullWithCondition("TestString", x => true), "TestString");
    }

    [Test]
    public void FromSelectOrNullWithCondition_WithNull_CreatesNone()
    {
      CheckIsNone(Option<string>.FromSelectOrNullWithCondition(NullTestClass, x => x.ToString(), x => true));
    }

    [Test]
    public void FromSelectOrNullWithCondition_WithNullSelected_CreatesNone()
    {
      CheckIsNone(Option<string>.FromSelectOrNullWithCondition(ArbritraryTestClass, x => null, x => true));
    }

    [Test]
    public void FromSelectOrNullWithCondition_WithFalseCondition_CreatesNone()
    {
      CheckIsNone(Option<string>.FromSelectOrNullWithCondition(ArbritraryTestClass, x => x.ToString(), x => false));
    }

    [Test]
    public void FromSelectOrNullWithCondition_WithTrueCondition_CreatesSome()
    {
      CheckIsSomeOfValue(Option<string>.FromSelectOrNullWithCondition(ArbritraryTestClass, x => "TestString", x => true), "TestString");
    }

    private static TestClass NullTestClass
    {
      get { return null; }
    }

    private static TestClass ArbritraryTestClass
    {
      get { return new TestClass(); }
    }
  }
}
