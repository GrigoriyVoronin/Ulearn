using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Rivals
{
	public class RivalsForm : Form
	{
		private readonly ScenePainter scenePainter;
		private readonly ScaledViewPanel scaledViewPanel;
		private readonly Timer timer;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			DoubleBuffered = true;
			WindowState = FormWindowState.Maximized;
		}

		public RivalsForm()
		{
			var levels = LoadLevels().ToArray();
			scenePainter = new ScenePainter(levels);
			scaledViewPanel = new ScaledViewPanel(scenePainter) { Dock = DockStyle.Fill };
			CreateLayout(levels);

			timer = new Timer { Interval = 20 };
			timer.Tick += TimerTick;
			timer.Start();
		}

		private static IEnumerable<Map> LoadLevels()
		{
			yield return Map.FromText(Properties.Resources.Dungeon1);
			yield return Map.FromText(Properties.Resources.Dungeon2);
			yield return Map.FromText(Properties.Resources.Dungeon3);
			yield return Map.FromText(Properties.Resources.Dungeon4);
			yield return Map.FromText(Properties.Resources.Dungeon5);
		}

		private void CreateLayout(Map[] levels)
		{
			var menuPanel = new FlowLayoutPanel
			{
				FlowDirection = FlowDirection.LeftToRight,
				Dock = DockStyle.Left,
				Width = 200,
				BackColor = Color.Black,
				Padding = new Padding(20),
				Font = new Font(SystemFonts.DefaultFont.FontFamily, 16)
			};
			DrawLevelSwitch(levels, menuPanel);
			Controls.Add(scaledViewPanel);
			Controls.Add(menuPanel);
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

		private void ChangeLevel(Map newMap)
		{
			scenePainter.ChangeLevel(newMap);
			timer.Start();
			scaledViewPanel.Invalidate();
		}

		private void UpdateLinksColors(Map level, List<LinkLabel> linkLabels)
		{
			foreach (var linkLabel in linkLabels)
			{
				linkLabel.LinkColor = linkLabel.Tag == level ? Color.LimeGreen: Color.White;
			}
			
		}

		private void TimerTick(object sender, EventArgs e)
		{
			scaledViewPanel.Invalidate();
			scenePainter.Update();
		}
	}
}