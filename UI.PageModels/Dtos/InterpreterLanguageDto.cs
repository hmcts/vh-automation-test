namespace UI.PageModels.Dtos
{
    public class InterpreterLanguageDto(string description, InterpreterType type)
    {
        public string Description { get; private set; } = description;
        public InterpreterType Type { get; private set; } = type;

    }
    
    public enum InterpreterType
    {
        Sign = 1,
        Verbal = 2
    }
}
