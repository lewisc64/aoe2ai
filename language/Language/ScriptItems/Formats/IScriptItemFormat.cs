namespace Language.ScriptItems.Formats
{
    public interface IScriptItemFormat
    {
        bool CanBeAppliedTo(IScriptItem scriptItem);
    }
}
