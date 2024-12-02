using Konteh.BackOffice.Api.Featuers.Questions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Konteh.BackOffice.Api.Featuers.Exams
{
    [ApiController]
    [Route("exams")]
    [Authorize]
    public class ExamsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExamsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SearchExams.ExamResponse>>> Search([FromQuery] string? search)
        {
            var response = await _mediator.Send(new SearchExams.Query
            {
                Search = search
            });

            return Ok(response);
        }
    }
}
