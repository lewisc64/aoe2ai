namespace Language.ScriptItems
{
    public class Defconst : IScriptItem
    {
        public string Name { get; set; }

        public int Value { get; set; }

        public bool MarkedForDeletion { get; } = false;

        public Defconst(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return $"(defconst {Name} = {Value})";
        }

        public void Optimize()
        {
            // nothing to do.
        }
    }
}
