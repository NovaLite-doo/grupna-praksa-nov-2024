using Konteh.BackOffice.Api.Domain.Enumeration;

namespace Konteh.BackOffice.Api.Domain
{
    public class Question
    {
        private String _id;
        private String _text;
        private List<Answer> _answers;
        private QuestionCategory _category;
        private QuestionType _type;

    }
}
