namespace CodeWriter.StyleComponents
{
    using System.Text;

    internal static class StringUtils
    {
        private static readonly StringBuilder Builder = new StringBuilder();

        public static string Replace(string line, params string[] args)
        {
            if (string.IsNullOrEmpty(line))
            {
                return string.Empty;
            }

            if (args.Length == 0)
            {
                return line;
            }

            Builder.Length = 0;

            string key;
            int prev = 0, len = line.Length, start, end;
            while (prev < len && (start = line.IndexOf('<', prev)) != -1)
            {
                Builder.Append(line, prev, start - prev);

                for (int i = 0; i < args.Length; i += 2)
                {
                    if ((key = args[i]) != null &&
                        (end = start + key.Length + 1) < len &&
                        (line[end] == '>') &&
                        (string.Compare(line, start + 1, key, 0, key.Length) == 0))
                    {
                        Builder.Append(args[i + 1]);
                        prev = end + 1;
                        goto replaced;
                    }
                }

                Builder.Append('<');
                prev = start + 1;

                replaced: ;
            }

            if (prev < line.Length)
            {
                Builder.Append(line, prev, line.Length - prev);
            }

            return Builder.ToString();
        }
    }
}