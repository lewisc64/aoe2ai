namespace Language.ScriptItems
{
    public interface IScriptItem
    {
        bool MarkedForDeletion { get; }

        void Optimize();
    }
}
