namespace Konteh.Infrastructure.ExceptionHandling
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message) : base(message)
        {
        }
    }
}
