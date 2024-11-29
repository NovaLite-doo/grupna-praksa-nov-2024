using Konteh.Domain.Events;
using Konteh.Domain.Enumeration;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Konteh.Domain;

namespace Konteh.FrontOffice.Api.Features.Exams
{
    [ApiController]
    [Route("exams")]
    public class ExamController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;

        public ExamController(IMediator mediator, IPublishEndpoint publishEndpoint)
        {
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost()]
        public async Task<IActionResult> GenerateExam()
        {
            await _mediator.Send(new CreateExam.Command());

            return Ok();
        }


        // temporary endpoint, add new exam to list
        [HttpGet("notifications")]
        public void Notify()
        {
            _publishEndpoint.Publish(new ExamEvent
            {   Id = -100,
                IsCompleted = false,
                Candidate = new ExamEventCandidate
                {
                    Name = "Cristiano",
                    Surname = "Ronaldo",
                    Email = "cr7@gmail.com",
                    Faculty = "DIF",
                    Major = "Football",
                    YearOfStudy = YearOfStudy.Master
                }
            });
        }
        // temporary endpoint, mark existing exam as finished and show score
        [HttpGet("notifications/{id:int}")]
        public void Notify(int id)
        {
            _publishEndpoint.Publish(new ExamEvent
            {
                Id = id,
                IsCompleted = true,
                QuestionCount = 30,
                CorrectAnswerCount = 23,
                Score = (double)23 / 30 * 100
            });
        }
    }
}
