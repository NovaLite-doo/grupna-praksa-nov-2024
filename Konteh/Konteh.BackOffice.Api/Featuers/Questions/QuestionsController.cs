using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Konteh.BackOffice.Api.Featuers.Questions
{
    [ApiController]
    [Authorize]
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

        [HttpGet("{id}")]
        public async Task<ActionResult<GetQuestionById.Response>> GetQuestionById(int id)
        {
            try
            {
                var response = await _mediator.Send(new GetQuestionById.Query { Id = id });
                return Ok(response);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> CreateOrUpdate(CreateOrUpdateQuestion.QuestionRequest request)
        {
            try
            {
                await _mediator.Send(request);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}

