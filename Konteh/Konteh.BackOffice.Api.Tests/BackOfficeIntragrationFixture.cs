using Konteh.Tests.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Konteh.BackOffice.Api.Tests
{
    public class BackOfficeIntragrationFixture : BaseIntegrationFixture<Program>
    {
        public BackOfficeIntragrationFixture() : base(Action)
        {
        }

        private static void Action(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "TestScheme";
                options.DefaultChallengeScheme = "TestScheme";
            }).AddScheme<AuthenticationSchemeOptions, FakeAuthenticationHandler>("TestScheme", options => { });
        }
    }
}
