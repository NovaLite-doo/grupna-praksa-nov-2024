namespace Konteh.Domain
{
    public class CheckboxQuestion : Question
    {
        public override bool IsCorrect(IEnumerable<int> submittedAnswerIds)
        {
            var correctAnswerIds = Answers.Where(x => x.IsCorrect).Select(x => x.Id).Order();
            return correctAnswerIds.SequenceEqual(submittedAnswerIds.Order());
        }
    }
}
