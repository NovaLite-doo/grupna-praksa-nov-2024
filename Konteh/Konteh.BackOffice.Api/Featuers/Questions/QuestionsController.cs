using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Konteh.BackOffice.Api.Featuers.Questions
{
    [ApiController]
    [Route("questions")]
    public class QuestionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public QuestionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _mediator.Send(new DeleteQuestion.Command { Id = id });

                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<SearchQuestions.PagedResponse>> Search([FromQuery] SearchQuestions.Query request)
        {
            var response = await _mediator.Send(request);

            return Ok(response);
        }
    }
}

