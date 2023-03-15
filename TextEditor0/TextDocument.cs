using System;

namespace TextEditor
{
    public class TextDocument
    {
        private TextBox textbox;

        public string FilePath { get; set; }
        public string FileName { get; set; }
        public bool HasChanges { get; }

        public TextDocument(TextBox textbox)
        {
            this.textbox = textbox;
        }

        public Save()
        {

        }

        public Open()
        {

        }

    }

}

