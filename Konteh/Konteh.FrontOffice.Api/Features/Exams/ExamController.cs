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

        [HttpPost]
        public async Task<IActionResult> GenerateExam(CreateExam.Command command)
        {
            var exam = await _mediator.Send(command);

            return Ok(new { examId = exam.Id });
        }

        [HttpGet("{examId}")]
        public async Task<ActionResult<GetExamById.Response>> GetExamById([FromRoute] int examId)
        {
            try
            {
                var query = new GetExamById.Query { ExamId = examId };

                var response = await _mediator.Send(query);

                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitExam(SubmitExam.Command command)
        {
            try
            {
                await _mediator.Send(command);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
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
