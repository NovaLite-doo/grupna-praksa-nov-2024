using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Konteh.BackOffice.Api.Featuers.Exams.GetExamStatistics;

namespace Konteh.BackOffice.Api.Featuers.Exams
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

        [HttpGet("statistics")]
        [ProducesResponseType(typeof(ExamStatisticsResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<ExamStatisticsResponse>> GetStatistics()
        {
            var statistics = await _mediator.Send(new GetExamStatistics.Query());
            return Ok(statistics);
        }
    }
}
