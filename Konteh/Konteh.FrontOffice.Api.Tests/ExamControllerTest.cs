using Argon;
using Konteh.Domain.Enumeration;
using Konteh.FrontOffice.Api.Features.Exams;
using System.Net.Http.Json;


namespace Konteh.FrontOffice.Api.Tests
{
    [Category("integration")]
    public class ExamControllerTest : FrontOfficeIntegrationFixture
    {
        [SetUp]
        public async Task SetUpAsync()
        {
            await AddQuestions();
        }

        [Test]
        public async Task GenerateExam()
        {
            var command = new CreateExam.Command
            {
                Email = "lucy@gmail.com",
                Faculty = "Ftn",
                Major = "RA",
                Name = "Lucy",
                Surname = "Bing",
                YearOfStudy = YearOfStudy.Master
            };

            var result = await _client.PostAsJsonAsync("/exams", command);

            Assert.That(result.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

            var jsoncontent = await result.Content.ReadAsStringAsync();
            var examId = JsonConvert.DeserializeObject<int>(jsoncontent);

            var examResponse = await _client.GetAsync($"/exams/{examId}");
            var examJson = await examResponse.Content.ReadAsStringAsync();
            var retrievedExam = JsonConvert.DeserializeObject<GetExamById.Response>(examJson);

            Assert.That(retrievedExam, Is.Not.Null);

            await Verify(retrievedExam).IgnoreMembers("Id");
        }

    }
}
