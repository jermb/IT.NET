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
            new KeywordSet(Color.FromArgb(255, 138, 51, 51), "if", "else", "elif", "while", "for", "except", "finally", "raise"),
            //  Evaluators
            new KeywordSet(Color.FromArgb(255, 51, 52, 138), "and", "or", "not", "is", "as", "in", "=", @"\+", "-", "/", "%", @"\*", @"\|", "&"),
            //  End Lines
            new KeywordSet(Color.FromArgb(255, 138, 83, 51), "break", "continue", "return", "pass", "yield"),
            //  Values
            new KeywordSet(Color.FromArgb(255, 51, 138, 57), "False", "None", "True"),
            //  Strings
            new KeywordSet(Color.FromArgb(255, 110, 51, 138), new Regex("\".*\"|'.*'"), "\"", "'"),
            //  Custom Functions
            new KeywordSet(Colors.DarkCyan, new Regex(@"\b\w+(?=\([\w\s\S]*\))"))
            // Comments
            //new KeywordSet(Colors.RosyBrown, new Regex($@"#.*$"))
        };

        private static readonly Color comments = Colors.RosyBrown;

        private static readonly SolidColorBrush Default = new SolidColorBrush(Colors.Black);
       
        public static void ColorText(this RichEditBox box)
        {
            ITextDocument textDocument = box.Document;
            ITextSelection textSelection = textDocument.Selection;
            int cursor = textDocument.Selection.EndPosition;

            //  Gets Text from RichEditBox
            textDocument.GetText(TextGetOptions.None, out string text);

            foreach (var set in KeywordSets)
            {
                set.CheckKeywords(text, textSelection);
            }

            CommentColor(text, textSelection);

            //  Resets Cursor Position and text color of cursor
            textSelection.SetRange(cursor, cursor);
            textSelection.CharacterFormat.ForegroundColor = Default.Color;
        }

        public static void NoColor(this RichEditBox box)
        {
            //  Resets the color of the whole text box to black
            var selection = box.Document.Selection;

            int start = selection.StartPosition;
            int end = selection.EndPosition;

            selection.SetRange(0, TextConstants.MaxUnitCount);
            selection.CharacterFormat.ForegroundColor = Default.Color;

            selection.SetRange(start, end);
        }

        private static void CheckKeywords(this KeywordSet set, string text, ITextSelection textSelection)
        {
            //  Uses the Regex from the KeywordSet to get word matches
            MatchCollection matches = set.Regex.Matches(text);
            if (matches.Count < 1) return;

            for (int i = matches.Count - 1; i >= 0; i--)
            {
                Match match = matches[i];

                //  Changes the color of the text at each match
                textSelection.SetRange(match.Index, match.Index + match.Length);
                textSelection.CharacterFormat.ForegroundColor = set.Color;
            }
        }

        public static void CommentOut(this RichEditBox box)
        {
            ITextDocument textDocument = box.Document;
            ITextSelection textSelection = textDocument.Selection;

            //  Gets selection if it was stored in the tag (used for button press)
            if (box.Tag != null)
            {
                Tuple<int, int> selection = (Tuple<int, int>)box.Tag;
                textSelection.SetRange(selection.Item1, selection.Item2);
            }

            //  Saves cursor position
            int cursor = textDocument.Selection.EndPosition;

            //  Gets the text from the text box
            textDocument.GetText(TextGetOptions.None, out string text);

            //  Finds all endlines
            MatchCollection matches = Regex.Matches(text, @"[\r\n\v]");

            //  Finds the last endline position before the selection
            Match startMatch = matches.Where(match => match.Index <= textSelection.SelectionMin()).OrderByDescending(match => match.Index).FirstOrDefault();
            int start = (startMatch == null || startMatch.Index < 0) ? 0 : startMatch.Index;

            //  Finds the last endline position before the selection ends
            Match endMatch = matches.Where(match => match.Index <= textSelection.SelectionMax()).OrderByDescending(match => match.Index).FirstOrDefault();
            if (endMatch == null)
            {
                endMatch = matches.Where(match => match.Index >= textSelection.EndPosition).OrderBy(match => match.Index).FirstOrDefault();
            }
            int end = (endMatch == null || endMatch.Index < start) ? start : endMatch.Index;

            //  gets the text before and after the selection
            string preSel = (start > 0) ? text.Substring(0, start) : "#";
            string postSel = text.Substring(end);

            //  Splits selection into lines
            string[] lines = text.Substring(start, end - start).Split(new string[] {"\r\n", "\r", "\n", "\v"}, StringSplitOptions.RemoveEmptyEntries);

            //  Adds or removes # from each line
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line.StartsWith('#')) line = line.Substring(1);
                else lines[i] = "#" + line;

                lines[i] = "\r\n" + lines[i];
            }

            //  Joins all strings into one final string then sets that as the text in teh text box
            string replacement = string.Join("", lines);
            string result = preSel + replacement + postSel;

            textDocument.SetText(TextSetOptions.None, result);
        }

        private static void CommentColor(string text, ITextSelection textSelection)
        {
            //  
            Regex commentRegex = new Regex($@"#.*$", RegexOptions.Multiline);
            MatchCollection matches = commentRegex.Matches(text);

            for (int i = matches.Count - 1; i >= 0; i--)
            {
                Match match = matches[i];
                textSelection.SetRange(match.Index, match.Index + match.Length);
                textSelection.CharacterFormat.ForegroundColor = comments;
            }
        }

        private static int SelectionMin(this ITextSelection selection)
        {
            //  Gets the lowest index of the selection
            if (selection.StartPosition < selection.EndPosition)
            {
                return selection.StartPosition;
            }
            return selection.EndPosition;
        }

        private static int SelectionMax(this ITextSelection selection)
        {
            //  Gets highest index of selection
            if (selection.StartPosition < selection.EndPosition)
            {
                return selection.EndPosition;
            }
            return selection.StartPosition;
        }

        private class KeywordSet
        {

            // This class is just used for easy storage of corresponding Keywords, Regex, and their colors

            public string[] Keywords { get => keywords; }
            private string[] keywords;

            public Regex Regex { get => regex; }
            private Regex regex;

            public Color Color { get => color; }
            private Color color;

            public KeywordSet(Color color, params string[] keywords)
            {
                this.keywords = keywords;
                this.color = color;
                regex = new Regex($@"\b({string.Join("|", keywords)})\b");
            }

            public KeywordSet(Color color, Regex regex, params string[] keywords)
            {
                this.color = color;
                this.keywords = keywords;
                this.regex = regex;
            }
        }

    }
}
