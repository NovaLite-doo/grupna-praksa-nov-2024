using FakeItEasy;
using Konteh.Domain;
using Konteh.Domain.Enumeration;
using Konteh.FrontOffice.Api.Features.Exams;
using Konteh.Infrastructure;
using Konteh.Infrastructure.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;


namespace Konteh.FrontOffice.Api.Tests
{
    public class ExamControllerTest
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ExamControllerTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        public async Task GenerateExam()
        {
            var client = _factory.CreateClient();

            var result = await client.PostAsync("/exams", null);

            await Verify(result);
        }

        
    }
}
