namespace UI.PageModels.Dtos
{
    public class PanelMemberInterpreterLanguageDto(string description, InterpreterType type)

    {
        public string Description { get; private set; } = description;
        public InterpreterType Type { get; private set; } = type;

    }
}
