using Konteh.Domain.Events;
using Konteh.Domain.Enumeration;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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


        // temporary endpoint
        [HttpGet()]
        public void Notify()
        {
            _publishEndpoint.Publish(new ExamEvent
            {   ExamId = 0
            });
        }
    }
}
