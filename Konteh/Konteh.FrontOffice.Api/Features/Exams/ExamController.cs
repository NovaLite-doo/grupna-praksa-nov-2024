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
        public async Task<ActionResult<int>> GenerateExam()
        {
            var response = await _mediator.Send(new CreateExam.Command());

            return Ok(response);
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
    }
}
