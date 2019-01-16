using System.Collections.Generic;
using System.IO;
using System.Text;
using DiffMatchPatch;
using Newtonsoft.Json.Linq;

namespace Snapper.Json
{
    internal class JsonDiff
    {
        internal static string GetDiffMessage(JToken old, JToken @new)
        {
            var dmp = new diff_match_patch();
            var a = dmp.diff_linesToChars(old.ToString(), @new.ToString());
            var lineText1 = (string) a[0];
            var lineText2 = (string) a[1];
            var lineArray = (List<string>) a[2];
            var diffs = dmp.diff_main(lineText1, lineText2, false);
            dmp.diff_charsToLines(diffs, lineArray);
            dmp.diff_cleanupEfficiency(diffs);

            var builder = new StringBuilder("\n");

            builder.AppendLine("- Snapshot");
            builder.AppendLine("+ Receieved\n\n");

            for (var i = 0; i < diffs.Count; i++)
            {
                var diff = diffs[i];

                var lines = GetLines(diff.text);

                switch (diff.operation)
                {
                    case Operation.DELETE:
                        lines.ForEach(l => builder.AppendLine($"-  {l}"));
                        break;
                    case Operation.INSERT:
                        lines.ForEach(l => builder.AppendLine($"+  {l}"));
                        break;
                    case Operation.EQUAL:
                        GetEqualLines(lines, i == 0, i == diffs.Count - 1).ForEach(l => builder.AppendLine(l));
                        break;
                }
            }

            return builder.ToString();
        }

        private static List<string> GetEqualLines(List<string> lines, bool isFirst, bool isLast)
        {
            if (isFirst)
                return lines.Count >= 2 ? lines.Splice(lines.Count - 2, 2) : lines;

            if (isLast)
                return lines.Count >= 2 ? lines.Splice(0, 2) : lines;

            if (lines.Count <= 4)
                return lines;

            var firstTwo = lines.Splice(0, 2);
            var lastTwo = lines.Splice(lines.Count - 2, 2);

            firstTwo.Add("...");
            firstTwo.AddRange(lastTwo);
            return firstTwo;
        }

        private static List<string> GetLines(string text)
        {
            var lines = new List<string>();
            using (var sr = new StringReader(text))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines;
        }
    }
}
