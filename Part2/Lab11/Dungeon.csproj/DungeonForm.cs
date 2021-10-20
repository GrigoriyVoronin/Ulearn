using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Dungeon.Properties;

namespace Dungeon
{
    public class DungeonForm : Form
    {
        private readonly ScaledViewPanel scaledViewPanel;
        private readonly ScenePainter scenePainter;
        private readonly Timer timer;

        public DungeonForm()
        {
            var levels = LoadLevels().ToArray();
            scenePainter = new ScenePainter(levels);
            var menuPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Dock = DockStyle.Left,
                Width = 200,
                BackColor = Color.Black,
                Padding = new Padding(20),
                Font = new Font(SystemFonts.DefaultFont.FontFamily, 16)
            };
            scaledViewPanel = new ScaledViewPanel(scenePainter) {Dock = DockStyle.Fill};
            DrawLevelSwitch(levels, menuPanel);
            Controls.Add(scaledViewPanel);
            Controls.Add(menuPanel);

            timer = new Timer {Interval = 50};
            timer.Tick += TimerTick;
            timer.Start();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DoubleBuffered = true;
            WindowState = FormWindowState.Maximized;
            Text = "Click on any empty cell to run BFS";
        }

        private static IEnumerable<Map> LoadLevels()
        {
            yield return Map.FromText(Resources.Dungeon1);
            yield return Map.FromText(Resources.Dungeon2);
            yield return Map.FromText(Resources.Dungeon3);
            yield return Map.FromText(Resources.Dungeon4);
            yield return Map.FromText(Resources.BigTestDungeon);
        }


        private void DrawLevelSwitch(Map[] levels, FlowLayoutPanel menuPanel)
        {
            menuPanel.Controls.Add(new Label
            {
                Text = "Choose level:",
                ForeColor = Color.White,
                AutoSize = true,
                Margin = new Padding(8)
            });
            var linkLabels = new List<LinkLabel>();
            for (var i = 0; i < levels.Length; i++)
            {
                var level = levels[i];
                var link = new LinkLabel
                {
                    Text = $"Level {i + 1}",
                    ActiveLinkColor = Color.LimeGreen,
                    TextAlign = ContentAlignment.MiddleCenter,
                    AutoSize = true,
                    Margin = new Padding(32, 8, 8, 8),
                    Tag = level
                };
                link.LinkClicked += (sender, args) =>
                {
                    ChangeLevel(level);
                    UpdateLinksColors(level, linkLabels);
                };
                menuPanel.Controls.Add(link);
                linkLabels.Add(link);
            }

            UpdateLinksColors(levels[0], linkLabels);
        }

        private void UpdateLinksColors(Map level, List<LinkLabel> linkLabels)
        {
            foreach (var linkLabel in linkLabels)
                linkLabel.LinkColor = linkLabel.Tag == level ? Color.LimeGreen : Color.White;
        }

        private void ChangeLevel(Map newMap)
        {
            scenePainter.ChangeLevel(newMap);
            timer.Start();
            scaledViewPanel.Invalidate();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            scaledViewPanel.Invalidate();
            scenePainter.Update();
        }
    }
}