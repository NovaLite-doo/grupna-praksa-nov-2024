using Konteh.BackOffice.Api.Featuers.Questions;
using Konteh.Tests.Infrastructure;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace Konteh.BackOffice.Api.Tests
{
    public class QuestionControllerTest : BaseIntegrationFixture<Program>
    {
        [SetUp]
        public async Task SetUpAsync()
        {
            await AddQuestions();
        }

        [Test]
        public async Task Test_SearchQuestions()
        {
            var queryParameters = new Dictionary<string, string>
            {
                { "SearchText", "" }
            };

            var response = await _client.GetAsync(QueryHelpers.AddQueryString("/questions/search", queryParameters));

            Assert.That(response.IsSuccessStatusCode, Is.True);

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SearchQuestions.PagedResponse>(content);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Questions.Any(), Is.True);
        }

    }
}