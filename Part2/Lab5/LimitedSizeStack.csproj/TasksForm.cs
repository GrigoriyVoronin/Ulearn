using System.Drawing;
using System.Windows.Forms;

namespace TodoApplication
{
    public class TasksForm : Form
    {
        private ListModel<string> model;
        private ListBox TasksList;
        private Button buttonRemove;
        private Button buttonUndo;
        private Button buttonAdd;
        private TextBox textBox;

        public TasksForm()
        {
            model = new ListModel<string>(20);
            model.AddItem("Составить список дел на сегодня");
            model.AddItem("Домашка по C#");
            model.AddItem("Решить задачу 1519");

            this.Text = "Список дел на сегодня";
            this.Font = new Font("Serif", 11);
            this.Size = new Size(400, 300);
            TasksList = new ListBox
            {
                Dock = DockStyle.Fill
            };
            TasksList.Items.AddRange(model.Items.ToArray());
            var label = new Label
            {
                Text = "Введите новое задание",
                Dock = DockStyle.Fill
            };
            textBox = new TextBox
            {
                Dock = DockStyle.Fill,
            };
            buttonAdd = new Button
            {
                Text = "Добавить",
                Dock = DockStyle.Fill
            };
            buttonUndo = new Button
            {
                Text = "Отменить",
                Dock = DockStyle.Fill,
                Enabled = model.CanUndo()
            };
            buttonRemove = new Button
            {
                Text = "Удалить",
                Dock = DockStyle.Fill,
                Enabled = false
            };
            

            var table = new TableLayoutPanel
            {
                Padding = new Padding(5, 10, 5, 10)
            };
            table.RowStyles.Clear();
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 33));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 75));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));

            table.Controls.Add(new Panel(), 3, 3);
            table.Controls.Add(TasksList, 1, 3);
            table.Controls.Add(label, 1, 0);
            table.Controls.Add(textBox, 1, 1);
            table.Controls.Add(buttonAdd, 2, 1);
            var interTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            interTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            interTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            interTable.Controls.Add(buttonUndo, 1, 0);
            interTable.Controls.Add(buttonRemove, 0, 0);
            table.Controls.Add(interTable, 1, 2);
            table.LayoutSettings.SetColumnSpan(interTable, 2);

            table.Dock = DockStyle.Fill;
            Controls.Add(table);


            textBox.KeyDown += (sender, args) =>
            {
                if (args.KeyCode == Keys.Enter)
                {
                    AddTask();
                    args.Handled = true;
                    args.SuppressKeyPress = true;
                }
            };

            buttonAdd.Click += (sender, args) => AddTask();

            buttonUndo.Click += (sender, args) =>
            {
                if (model.CanUndo())
                {
                    model.Undo();
                    TasksList.Items.Clear();
                    TasksList.Items.AddRange(model.Items.ToArray());
                }

                if (!model.CanUndo())
                {
                    buttonUndo.Enabled = false;
                }
            };

            buttonRemove.Click += (sender, args) =>
            {
                int index = TasksList.SelectedIndex;
                if (index != -1)
                {
                    model.RemoveItem(index);
                    TasksList.Items.Clear();
                    TasksList.Items.AddRange(model.Items.ToArray());
                    buttonRemove.Enabled = false;
                    if (model.CanUndo())
                    {
                        buttonUndo.Enabled = true;
                    }
                }
            };

            TasksList.SelectedIndexChanged += (sender, args) =>
            {
                buttonRemove.Enabled = TasksList.SelectedIndex != -1;
            };
        }

        private void AddTask()
        {
            model.AddItem(textBox.Text);
            textBox.Text = "";
            TasksList.Items.Clear();
            TasksList.Items.AddRange(model.Items.ToArray());
            if (model.CanUndo())
            {
                buttonUndo.Enabled = true;
            }
        }
    }
}
