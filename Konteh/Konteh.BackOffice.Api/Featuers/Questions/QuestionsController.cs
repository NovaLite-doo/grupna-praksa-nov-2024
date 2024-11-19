using Azure.Core;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAllQuestions.Response>>> GetAll()
        {
            var response = await _mediator.Send(new GetAllQuestions.Query());
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
