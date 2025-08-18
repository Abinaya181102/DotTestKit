using AutoFixture;
using Moq;

namespace OMSAPI.UnitTests.Base
{
    public abstract class UnitTestBase
    {
        protected readonly Fixture Fixture;

        protected UnitTestBase()
        {
            Fixture = new Fixture();
        }

        protected Mock<T> Mock<T>() where T : class => new Mock<T>();
    }
}
