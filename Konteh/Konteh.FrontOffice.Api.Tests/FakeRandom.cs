using Konteh.Infrastructure;

namespace Konteh.FrontOffice.Api.Tests
{
    public class FakeRandom : IRandom
    {
        private int number = 0;

        public int NextInt() => number++;
    }
}
