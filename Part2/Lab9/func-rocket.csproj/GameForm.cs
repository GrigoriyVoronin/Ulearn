using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace func_rocket
{
	public class GameForm : Form
	{
		private readonly Image rocket;
		private readonly Image target;
		private readonly Timer timer;
		private Level currentLevel;
		private bool right;
		private bool left;
		private readonly Size spaceSize = new Size(800, 600);
		private readonly Image image;
		private int iterationIndex;
		private string helpText;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			DoubleBuffered = true;
			helpText = "Use A and D to control rocket";
			Text = helpText;
			WindowState = FormWindowState.Maximized;
		}

		public GameForm(IEnumerable<Level> levels)
		{
			rocket = Image.FromFile("images/rocket.png");
			target = Image.FromFile("images/target.png");
			image = new Bitmap(spaceSize.Width, spaceSize.Height, PixelFormat.Format32bppArgb);
			timer = new Timer { Interval = 10 };
			timer.Tick += TimerTick;
			timer.Start();
			var top = 10;
			foreach (var level in levels)
			{
				if (currentLevel == null) currentLevel = level;
				var link = new LinkLabel
				{
					Text = level.Name,
					Left = 10,
					Top = top,
					BackColor = Color.Transparent
				};
				link.LinkClicked += (sender, args) => ChangeLevel(level);
				link.Parent = this;
				Controls.Add(link);
				top += link.PreferredHeight + 10;
			}
			KeyPreview = true;
		}

		private void ChangeLevel(Level newSpace)
		{
			currentLevel = newSpace;
			currentLevel.Reset();
			timer.Start();
			iterationIndex = 0;
		}

		private void TimerTick(object sender, EventArgs e)
		{
			if (currentLevel == null) return;
			MoveRocket();
			if (currentLevel.IsCompleted)
				timer.Stop();
			else
				Text = helpText + ". Iteration # " + iterationIndex++;
			Invalidate();
			Update();
		}

		private void MoveRocket()
		{
			var control = left ? Turn.Left : (right ? Turn.Right : Turn.None);
			if (control == Turn.None)
				control = ControlTask.ControlRocket(currentLevel.Rocket, currentLevel.Target);
			currentLevel.Move(spaceSize, control);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			HandleKey(e.KeyCode, true);
		}

		private void HandleKey(Keys e, bool down)
		{
			if (e == Keys.A) left = down;
			if (e == Keys.D) right = down;
		}
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			HandleKey(e.KeyCode, false);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.FillRectangle(Brushes.Bisque, ClientRectangle);
			var g = Graphics.FromImage(image);
			DrawTo(g);
			e.Graphics.DrawImage(image, (ClientRectangle.Width - image.Width)/2, (ClientRectangle.Height - image.Height) / 2);
		}

		private void DrawTo(Graphics g)
		{
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.FillRectangle(Brushes.Beige, ClientRectangle);

			if (currentLevel == null) return;

			DrawGravity(g);
			var matrix = g.Transform;

			g.TranslateTransform((float) currentLevel.Target.X, (float) currentLevel.Target.Y);
			g.DrawImage(target, new Point(-target.Width/2, -target.Height/2));

			if (timer.Enabled)
			{
				g.Transform = matrix;
				g.TranslateTransform((float) currentLevel.Rocket.Location.X, (float) currentLevel.Rocket.Location.Y);
				g.RotateTransform(90 + (float) (currentLevel.Rocket.Direction*180/Math.PI));
				g.DrawImage(rocket, new Point(-rocket.Width/2, -rocket.Height/2));
			}
		}

		private void DrawGravity(Graphics g)
		{
			Action<Vector, Vector> draw = (a,b) => g.DrawLine(Pens.DeepSkyBlue, (int)a.X, (int)a.Y, (int)b.X, (int)b.Y);
			for (int x = 0; x < spaceSize.Width; x += 50)
				for (int y = 0; y < spaceSize.Height; y += 50)
				{
					var p1 = new Vector(x, y);
					var v = currentLevel.Gravity(spaceSize, p1);
					if (double.IsInfinity(v.X) || double.IsInfinity(v.Y))
						continue;
					var p2 = p1 + 20 * v;
					var end1 = p2 - 8*v.Rotate(0.5);
					var end2 = p2 - 8*v.Rotate(-0.5);
					draw(p1, p2);
					draw(p2, end1);
					draw(p2, end2);
				}
		}
	}
}