using NUnit.Framework;

namespace Sp.Idioms.Tests.Unit.ValueOptionTests
{
  [TestFixture]
  public class CreationTests : TestsBase
  {
    [Test]
    public void None_CreatesNone()
    {
      CheckIsNone(ValueOption<int>.None);
    }

    [Test]
    public void Some_CreatesSome()
    {
      CheckIsSomeOfValue(ValueOption<int>.Some(0), 0);
    }
  }
}
