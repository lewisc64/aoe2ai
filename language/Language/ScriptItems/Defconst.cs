namespace Language.ScriptItems
{
    public class Defconst<T> : IScriptItem
    {
        public string Name { get; set; }

        public T Value { get; set; }

        public bool MarkedForDeletion { get; } = false;

        public Defconst(string name, T value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            if (Value is int)
            {
                return $"(defconst {Name} {Value})";
            }
            else
            {
                return $"(defconst {Name} \"{Value}\")";
            }
        }

        public void Optimize()
        {
            // nothing to do.
        }
    }
}
