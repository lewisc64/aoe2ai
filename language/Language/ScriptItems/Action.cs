namespace Language.ScriptItems
{
    public class Action : ICopyable<Action>
    {
        public string Text { get; set; }

        public virtual int Length => 1;

        public Action(string text)
        {
            Text = text;
        }

        public override string ToString()
        {
            return $"({Text})";
        }

        public virtual Action Copy()
        {
            return new Action(Text);
        }
    }
}
