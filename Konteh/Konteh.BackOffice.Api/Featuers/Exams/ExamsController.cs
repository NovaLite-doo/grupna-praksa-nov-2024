using Konteh.BackOffice.Api.Featuers.Questions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Konteh.BackOffice.Api.Featuers.Exams
{
    [ApiController]
    [Route("questions")]
    //[Authorize]
    public class ExamsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExamsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SearchExams.ExamResponse>>> Search([FromQuery] string? search, [FromQuery] bool? isCompleted)
        {
            var response = await _mediator.Send(new SearchExams.Query
            {
                Search = search,
                IsCompleted = isCompleted
            });

            return Ok(response);
        }
    }
}
