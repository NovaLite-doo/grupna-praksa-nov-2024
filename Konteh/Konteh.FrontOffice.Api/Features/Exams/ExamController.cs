using Konteh.Domain.Events;
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

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> GenerateExam(CreateExam.Command command)
        {
            var examId = await _mediator.Send(command);

            return Ok(examId);

        }

        [HttpGet("{examId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetExamById.Response), StatusCodes.Status200OK)]

        public async Task<ActionResult<GetExamById.Response>> GetExamById([FromRoute] int examId)
        {
            var query = new GetExamById.Query { ExamId = examId };

            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpPost("submit")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SubmitExam(SubmitExam.Command command)
        {
            await _mediator.Send(command);

            return Ok();
        }


        // temporary endpoint
        [HttpGet()]
        public void Notify()
        {
            _publishEndpoint.Publish(new ExamEvent
            {
                ExamId = 0
            });
        }
    }
}
