using System;
using System.Drawing;
using System.Windows.Forms;

namespace TableParser
{
    public class ParserMainForm : Form
    {
        private readonly TextBox parsedTextBox;
        private readonly TextBox textBox;
        private readonly string title = "Результат работы парсера:";

        public ParserMainForm()
        {
            var initialString = "\"bcd ef\" a 'x y'";
            Padding = new Padding(20);
            textBox = new TextBox
            {
                Text = initialString,
                Dock = DockStyle.Top,
                Font = new Font(SystemFonts.DefaultFont.FontFamily, 18)
            };
            parsedTextBox = new TextBox
            {
                ReadOnly = true,
                Multiline = true,
                WordWrap = false,
                Dock = DockStyle.Fill,
                ScrollBars = ScrollBars.Both,
                Font = new Font("Courier New", 18)
            };
            UpdateParsedText();
            //WindowState = FormWindowState.Maximized;
            KeyPreview = true;
            Text = "Тестирование парсера";
            InitializeComponent();
            textBox.TextChanged += (sender, e) => UpdateParsedText();
        }

        private void UpdateParsedText()
        {
            parsedTextBox.Text =
                title
                + Environment.NewLine + Environment.NewLine
                + string.Join(Environment.NewLine, FieldsParserTask.ParseLine(textBox.Text));
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // textBox
            // 
            textBox.Location = new Point(130, 111);
            textBox.Name = "textBox";
            textBox.Size = new Size(100, 22);
            textBox.TabIndex = 0;
            // 
            // label
            // 
            parsedTextBox.AutoSize = true;
            parsedTextBox.Location = new Point(158, 181);
            parsedTextBox.Name = "parsedTextBox";
            parsedTextBox.Size = new Size(46, 17);
            parsedTextBox.TabIndex = 1;
            // 
            // GameForm
            // 
            ClientSize = new Size(532, 395);
            Controls.Add(parsedTextBox);
            Controls.Add(textBox);
            Name = "ParserMainForm";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}