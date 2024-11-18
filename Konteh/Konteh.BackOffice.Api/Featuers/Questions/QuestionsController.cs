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

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(CreateQuestion.QuestionRequest request)
        {
            try
            {
                await _mediator.Send(request);

                return Created();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<Unit>> Edit(EditQuestion.QuestionRequest request)
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
        }
    }
}
