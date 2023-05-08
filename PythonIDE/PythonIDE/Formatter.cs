using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using static System.Net.Mime.MediaTypeNames;

namespace PythonIDE
{
    internal static class Formatter
    {

        private static readonly KeywordSet[] KeywordSets =
        {
            //  Functions
            new KeywordSet(Colors.Red, "if", "else", "elif", "while", "for", "except", "finally", "raise"),
            //  Evaluators
            new KeywordSet(Colors.Blue, "and", "or", "not", "is", "as", "in"),
            //  End Lines
            new KeywordSet(Colors.Yellow, "break", "continue", "return", "pass", "yield"),
            //  Values
            new KeywordSet(Colors.Green, "False", "None", "True")
        };

        private static readonly Color comments = Colors.RosyBrown;

        public static void ColorText(this RichEditBox box)
        {
            ITextDocument textDocument = box.Document;
            ITextSelection textSelection = textDocument.Selection;
            int cursor = textDocument.Selection.EndPosition;

            textDocument.GetText(TextGetOptions.None, out string text);

            foreach (var set in KeywordSets)
            {
                set.CheckKeywords(text, textSelection);
            }
            CommentColor(text, textSelection);
            
            textSelection.SetRange(cursor, cursor);
            textSelection.CharacterFormat.ForegroundColor = new SolidColorBrush(Colors.Black).Color;
        }


        private static void CheckKeywords(this KeywordSet set, string text, ITextSelection textSelection)
        {
            //// Loop through the keywords and highlight each occurrence in the text
            foreach (string keyword in set.Keywords)
            {
                Regex keywordRegex = new Regex($@"\b{keyword}\b");

                MatchCollection matches = keywordRegex.Matches(text);

                for (int i = matches.Count - 1; i >= 0; i--)
                {
                    Match match = matches[i];

                    //// Highlight the match by changing its text color
                    textSelection.SetRange(match.Index, match.Index + match.Length);
                    textSelection.CharacterFormat.ForegroundColor = set.Color;
                }
            }
        }

        public static void CommentOut(this RichEditBox box)
        {
            ITextDocument textDocument = box.Document;
            ITextSelection textSelection = textDocument.Selection;

            if (box.Tag != null)
            {
                Debug.WriteLine("!!!!!!!!!!!!!!!!! tag:" + box.Tag.ToString());
                Tuple<int, int> selection = (Tuple<int, int>)box.Tag;
                textSelection.SetRange(selection.Item1, selection.Item2);
            }

            if (textSelection.EndPosition > textSelection.StartPosition)
            {
                textSelection.SetRange(textSelection.EndPosition, textSelection.StartPosition);
            }

            int cursor = textDocument.Selection.EndPosition;

            textDocument.GetText(TextGetOptions.None, out string text);

            MatchCollection matches = Regex.Matches(text, @"[\r\n\v]");
            //int start = text.LastIndexOf("\r", textSelection.StartPosition);
            ////int start = matches.Count > 0? matches.Cast<Match>(
            Match startMatch = matches.Where(match => match.Index <= textSelection.StartPosition).OrderByDescending(match => match.Index).FirstOrDefault();
            int start = (startMatch == null || startMatch.Index < 0) ? 0 : startMatch.Index;
            //int end = text.LastIndexOf("\r", textSelection.EndPosition);
            Match endMatch = matches.Where(match => match.Index <= textSelection.EndPosition).OrderByDescending(match => match.Index).FirstOrDefault();
            int end = (endMatch == null || endMatch.Index < start) ? start : endMatch.Index;


            string preSel = (start > 0) ? text.Substring(0, start) : "";
            string postSel = text.Substring(end);

            //string replacement = text.Substring(start, end - start).Replace("\r", "\r#").Replace("\n", "\n#").Replace("\v", "\v#");




            //string result = text.Substring(0, start) + text.Substring(start, end - start).Replace("\r", "\r#").Replace("\n", "\n#").Replace("\v", "\v#") + text.Substring(end);

            string[] lines = text.Substring(start, end - start).Split('\r', '\n', '\v');

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line.StartsWith('#')) line = line.Substring(1);
                else lines[i] = "#" + line;

                if (i > 0) line = "\r" + line;
            }

            string replacement = string.Join("", lines);

            string result = preSel + replacement + postSel;

            //string result = text.Substring(start) + string.Join("", lines) + text.Substring(end);


            Debug.WriteLine(result);

            textDocument.SetText(TextSetOptions.None, result);
        }

        private static void CommentColor(string text, ITextSelection textSelection)
        {
            Regex commentRegex = new Regex($@"#.*$");
            MatchCollection matches = commentRegex.Matches(text);

            for (int i = matches.Count - 1; i >= 0; i--)
            {
                Match match = matches[i];
                textSelection.SetRange(match.Index, match.Index + match.Length);
                textSelection.CharacterFormat.ForegroundColor = comments;
            }
        }

        private class KeywordSet
        {
            public string[] Keywords { get => keywords; }
            private string[] keywords;

            public Color Color { get => color; }
            private Color color;

            public KeywordSet(Color color, params string[] keywords)
            {
                this.keywords = keywords;
                this.color = color;
            }
        }

    }
}
