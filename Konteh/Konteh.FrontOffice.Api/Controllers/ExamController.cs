using Konteh.FrontOffice.Api.Features.Exams;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Konteh.FrontOffice.Api.Controllers
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

        [HttpPost("generate-candidate-exam")]
        public async Task<ActionResult<GetExam.Response>> GenerateCandidateExam()
        {
            var response = await _mediator.Send(new GetExam.Query());

            return Ok(response);
        }
    }
}
