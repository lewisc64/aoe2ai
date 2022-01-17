namespace Language.ScriptItems.Formats
{
    public class DefconstFormat : IScriptItemFormat
    {
        public bool CanBeAppliedTo(IScriptItem scriptItem)
        {
            return scriptItem.GetType().Name == "Defconst`1";
        }

        public string Format<T>(Defconst<T> defconst)
        {
            if (defconst.Value is int)
            {
                return $"(defconst {defconst.Name} {defconst.Value})";
            }
            else
            {
                return $"(defconst {defconst.Name} \"{defconst.Value}\")";
            }
        }
    }
}
