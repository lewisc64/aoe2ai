using Language.Extensions;
using Language.ScriptItems;
using Language.ScriptItems.Formats;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Language
{
    public class Script : IEnumerable<IScriptItem>, ICopyable<Script>
    {
        public List<IScriptItem> Items { get; } = new List<IScriptItem>();

        public bool InsertLineBreaks { get; set; } = true;

        public int MaxLineLength { get; set; } = 255;

        public Script()
        {
        }

        public Script(IEnumerable<IScriptItem> items)
        {
            Items = items.ToList();
        }

        public string Render()
        {
            return Render(new IScriptItemFormat[0]);
        }

        public string Render(IEnumerable<IScriptItemFormat> scriptItemFormats)
        {
            IEnumerable<IScriptItemFormat> formats = scriptItemFormats
                .Concat(new IScriptItemFormat[]
                {
                    new IndentedCondition(),
                    new IndentedDefrule(),
                    new DefconstFormat(),
                });

            var output = new StringBuilder();

            foreach (var item in Items)
            {
                string formatted = null;

                var chosenFormat = formats.First(x => x.CanBeAppliedTo(item));
                if (chosenFormat is IDefruleFormat defruleFormat)
                {
                    formatted = defruleFormat.Format((Defrule)item, (IConditionFormat)formats.First(x => x is IConditionFormat));
                }
                else if (chosenFormat is IConditionFormat conditionFormat)
                {
                    formatted = conditionFormat.Format((Condition)item);
                }
                else if (chosenFormat is DefconstFormat defconstFormat)
                {
                    formatted = defconstFormat.Format((dynamic)item);
                }

                output.Append(formatted);
                if (InsertLineBreaks)
                {
                    output.Append(Environment.NewLine);
                }
            }

            return output.ToString().Wrap(MaxLineLength);
        }

        public IEnumerator<IScriptItem> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Script Copy()
        {
            return new Script(Items)
            {
                InsertLineBreaks = InsertLineBreaks,
            };
        }
    }
}
