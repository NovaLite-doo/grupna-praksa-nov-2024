using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Konteh.FrontOffice.Api.Features.Exams
{
    [ApiController]
    [Route("exams")]
    public class ExamController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExamController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost()]
        public async Task<IActionResult> GenerateExam(CreateExam.Command command)
        {
            await _mediator.Send(command);

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetExamById.Response>> GetExamById([FromRoute] int id)
        {
            try
            {
                var query = new GetExamById.Query { Id = id };

                var response = await _mediator.Send(query);

                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

    }
}
