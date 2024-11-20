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
        public async Task<IActionResult> GenerateExam()
        {
            await _mediator.Send(new CreateExam.Command());

            return Ok();
        }
    }
}
