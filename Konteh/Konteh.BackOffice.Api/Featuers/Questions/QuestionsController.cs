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
        public async Task<ActionResult<GetAllQuestions.PagedResponse>> GetAll([FromQuery] GetAllQuestions.Query request)
        {

            var response = await _mediator.Send(request);


            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteQuestion.Command { Id = id };
            try
            {

                await _mediator.Send(command);


                return NoContent();
            }
            catch (Exception ex)
            {

                return NotFound(new { message = ex.Message });
            }
        }


        [HttpGet("search")]
        public async Task<ActionResult<SearchQuestions.PagedResponse>> Search([FromQuery] SearchQuestions.Query request, CancellationToken cancellationToken)
        {

            var response = await _mediator.Send(request, cancellationToken);


            return Ok(response);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<GetCategories.Response>> GetCategories()
        {
            var response = await _mediator.Send(new GetCategories.Query());
            return Ok(response);
        }
    }
}

