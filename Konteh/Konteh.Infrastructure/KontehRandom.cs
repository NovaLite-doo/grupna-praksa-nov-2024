namespace Konteh.Infrastructure
{
    public class KontehRandom : IRandom
    {
        private readonly Random _random = new Random();

        public int NextInt() => _random.Next();
    }
}
