namespace Language.ScriptItems.Formats
{
    public interface IFormatter
    {
        bool CanBeAppliedTo(IScriptItem scriptItem);
    }
}
