using Konteh.Infrastructure;
using Konteh.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Konteh.FrontOffice.Api.Tests
{
    public class FrontOfficeIntegrationFixture : BaseIntegrationFixture<Program>
    {
        public FrontOfficeIntegrationFixture() : base(Action)
        {
        }

        private static void Action(IServiceCollection services)
        {
            services.AddScoped<IRandom, FakeRandom>();
        }
    }
}
