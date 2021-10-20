using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace rocket_bot
{
    public class GameForm : Form
    {
        private const int Interval = 20;
        private readonly Channel<Rocket> channel;
        private readonly Image checkpointImage;
        private readonly Level level;
        private readonly HashSet<Turn> manualControls = new HashSet<Turn>();
        private readonly TrackBar rewindTrackBar;
        private readonly Image rocketImage;
        private string helpText;
        private int lastChannelCount;
        private bool manualRewindInProgress;
        private bool paused;
        private Rocket rocket;
        private int skipTurns = 1;

        public GameForm(Level level, Channel<Rocket> channel)
        {
            rocket = level.InitialRocket;
            WindowState = FormWindowState.Maximized;
            this.channel = channel;
            this.level = level;
            KeyPreview = true;
            rocketImage = Image.FromFile("images/rocket.png");
            checkpointImage = Image.FromFile("images/flag.png");
            rewindTrackBar = new TrackBar
            {
                Dock = DockStyle.Bottom,
                Height = 10,
                TickFrequency = 10,
                LargeChange = 100,
                TabStop = false
            };
            rewindTrackBar.ValueChanged += (sender, e) => RewindTo(rewindTrackBar.Value);
            rewindTrackBar.MouseDown += (sender, e) => manualRewindInProgress = true;
            rewindTrackBar.MouseUp += (sender, e) => manualRewindInProgress = false;
            Controls.Add(rewindTrackBar);
            var playButtonsPanel = CreatePlayButtonsPanel();
            Controls.Add(playButtonsPanel);

            var timer = new Timer {Interval = Interval};
            timer.Tick += TimerTick;
            timer.Start();
        }

        private TableLayoutPanel CreatePlayButtonsPanel()
        {
            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Bottom,
                TabIndex = 1,
                TabStop = false,
                Height = 40
            };
            table.RowStyles.Clear();
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 31));
            table.ColumnStyles.Clear();
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, 6));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, 6));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, 6));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            var buttonSize = new Size(35, 30);
            var fastButton = new Button
            {
                Text = "►►",
                Size = buttonSize,
                TabStop = false
            };
            fastButton.Click += (sender, e) => SetPlaySpeed(5);
            table.Controls.Add(fastButton, 3, 0);
            var slowButton = new Button
            {
                Text = "►",
                Size = buttonSize,
                TabStop = false
            };
            slowButton.Click += (sender, e) => SetPlaySpeed(1);
            table.Controls.Add(slowButton, 2, 0);
            var pauseButton = new Button
            {
                Text = "❚❚",
                Size = buttonSize,
                TabStop = false
            };
            pauseButton.Click += (sender, e) => paused = true;
            table.Controls.Add(pauseButton, 1, 0);
            table.Controls.Add(new Panel(), 4, 0);
            return table;
        }

        private void SetPlaySpeed(int speed)
        {
            skipTurns = speed;
            paused = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DoubleBuffered = true;
            helpText = "Use A, W and D to control rocket";
            Text = helpText;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (level == null) return;
            rewindTrackBar.SetRange(0, level.MaxTicksCount);
            var moved = false;
            if (!manualRewindInProgress && !paused)
            {
                rewindTrackBar.Value = rocket.Time;
                moved = MoveRocket();
            }

            Text =
                $"{helpText}. Iteration # {rocket.Time} Checkpoints taken: {rocket.TakenCheckpointsCount}. Ticks precalculated: {channel.Count}. Precalculation speed: {Math.Max(0, (channel.Count - lastChannelCount) * 1000 / Interval)} ticks per second.";
            lastChannelCount = channel.Count;
            if (moved)
            {
                Invalidate();
                Update();
            }
        }

        private bool MoveRocket()
        {
            if (manualControls.Any())
            {
                if (rocket.IsCompleted(level)) return false;
                var control = manualControls.First();
                for (var i = 0; i < skipTurns; ++i)
                {
                    rocket = rocket.Move(control, level);
                    channel[rocket.Time] = rocket;
                }

                return true;
            }

            return RewindTo(rocket.Time + (manualRewindInProgress ? 0 : skipTurns));
        }

        private bool RewindTo(int time)
        {
            var prevRocket = channel[time] ?? channel.LastItem();
            if (prevRocket != null && rocket != prevRocket)
            {
                rocket = prevRocket;
                return true;
            }

            return false;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            HandleKey(e.KeyCode, true);
        }

        private void HandleKey(Keys e, bool down)
        {
            if (e == Keys.A || e == Keys.Left) SetManualControl(Turn.Left, down);
            if (e == Keys.D || e == Keys.Right) SetManualControl(Turn.Right, down);
            if (e == Keys.W || e == Keys.Up) SetManualControl(Turn.None, down);
            if (e == Keys.R) channel[0] = level.InitialRocket;
        }

        private void SetManualControl(Turn control, bool down)
        {
            if (down) manualControls.Add(control);
            else manualControls.Remove(control);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            HandleKey(e.KeyCode, false);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Bisque, ClientRectangle);
            var image = new Bitmap(ClientRectangle.Width, ClientRectangle.Height, PixelFormat.Format32bppArgb);
            var g = Graphics.FromImage(image);
            DrawTo(g, ClientRectangle);
            e.Graphics.DrawImage(image, (ClientRectangle.Width - image.Width) / 2,
                (ClientRectangle.Height - image.Height) / 2);
        }

        private void DrawTo(Graphics g, Rectangle rect)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.FillRectangle(Brushes.Beige, rect);
            var trackBarHeight = 100;
            var aiCalculationBarRect =
                new Rectangle(rect.Left, rect.Height - trackBarHeight, rect.Width, trackBarHeight + 1);
            g.FillRectangle(Brushes.DimGray, aiCalculationBarRect);
            g.FillRectangle(Brushes.MediumSeaGreen,
                new Rectangle(aiCalculationBarRect.Location,
                    new Size(aiCalculationBarRect.Width * channel.Count / level.MaxTicksCount, trackBarHeight)));

            if (level == null) return;

            g.TranslateTransform((rect.Width - level.SpaceSize.Width) / 2f,
                (rect.Height - level.SpaceSize.Height) / 2f);
            var matrix = g.Transform;

            for (var i = 0; i < level.Checkpoints.Length; ++i)
            {
                g.Transform = matrix;
                g.TranslateTransform((float) level.Checkpoints[i].X, (float) level.Checkpoints[i].Y);
                g.DrawImage(checkpointImage, new Point(-checkpointImage.Width / 2, -checkpointImage.Height / 2));
                if (rocket.TakenCheckpointsCount % level.Checkpoints.Length == i)
                    g.FillEllipse(Brushes.Gold, new Rectangle(-10, -20, 20, 20));
            }

            g.Transform = matrix;
            g.TranslateTransform((float) rocket.Location.X, (float) rocket.Location.Y);
            g.RotateTransform(90 + (float) (rocket.Direction * 180 / Math.PI));
            g.DrawImage(rocketImage, new Point(-rocketImage.Width / 2, -rocketImage.Height / 2));
        }
    }
}