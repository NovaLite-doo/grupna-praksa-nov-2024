namespace Konteh.Domain
{
    public class RadioButtonQuestion : Question
    {
        public override bool IsCorrect(IEnumerable<int> submittedAnswerIds)
        {
            if (submittedAnswerIds.Count() != 1)
            {
                return false;
            }

            return Answers.Single(x => x.IsCorrect).Id == submittedAnswerIds.Single();
        }
    }
}
