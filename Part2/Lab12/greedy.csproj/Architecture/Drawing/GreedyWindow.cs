using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Greedy.Architecture.Drawing
{
	public sealed class GreedyWindow : Form
	{
		private ScaledViewPanelPainter scaledViewPanelPainter = new ScaledViewPanelPainter();

		private StatusBar statusBar;
		private Timer timer = new Timer();
		private int _timerInterval = 300;
		private int currentStateInd;
		private IPathFinder currentPathFinder;

		private int timerInterval
		{
			get { return _timerInterval; }
			set { _timerInterval = Math.Max(1, value); }
		}

		private ToolStripItemCollection stateBtns => ((ToolStripDropDownButton) MainMenuStrip.Items["States"]).DropDownItems;
		private ToolStripButton pathFinderChangerBtn;
		private State currentState;

		public GreedyWindow(string stateName = null)
		{
			Controls.Add(scaledViewPanelPainter.ScaledViewPanel);
			MainMenuStrip = new MenuStrip();
			AddBtnsToMainMenu();
			Controls.Add(MainMenuStrip);
			Controls.Add(statusBar = new StatusBar {Name = "Status Bar"});
			MinimumSize = new Size(600, 600);
			DoubleBuffered = true;
			if (stateName != null)
			{
				currentStateInd = stateBtns.IndexOfKey(stateName);
			}
			stateBtns[currentStateInd].PerformClick();
		}

		private void AddBtnsToMainMenu()
		{
			var statesBtn = new ToolStripDropDownButton("States", null, BuildDropDownItemsWithStates()) {Name = "States"};
			var restartBtn =
				new ToolStripButton("Restart", null, (_, __) => RestartCurrentStateWithCurrentPathFinder()) {Name = "Restart"};
			var speedUp = new ToolStripButton("+ Speed +", null, (_, __) => { timerInterval -= 100; }) {Name = "+ Speed +"};
			var speedDown = new ToolStripButton("- Speed -", null, (_, __) => { timerInterval += 100; }) {Name = "- Speed -"};
			pathFinderChangerBtn =
				new ToolStripButton("PathFinder", null, ChangePathFinderTypeAndRestart)
				{
					Name = "PathFinder"
				};
			var prevBtn = new ToolStripButton("Previous state", null, SwitchToPreviousState) {Name = "Previous state"};
			var nextBtn = new ToolStripButton("Next state", null, SwitchToNextState) {Name = "Next state"};
			MainMenuStrip.Items.Add(statesBtn);
			MainMenuStrip.Items.Add(restartBtn);
			MainMenuStrip.Items.Add(speedUp);
			MainMenuStrip.Items.Add(speedDown);
			MainMenuStrip.Items.Add(pathFinderChangerBtn);
			MainMenuStrip.Items.Add(prevBtn);
			MainMenuStrip.Items.Add(nextBtn);
		}

		private ToolStripItem[] BuildDropDownItemsWithStates()
		{
			return StatesLoader
				.LoadAllStateNames(Folders.StatesForStudents)
				.Select(stateName => (ToolStripItem) new ToolStripButton(stateName, null, ChangeStateOnStateNameClick)
				{
					Name = stateName
				})
				.ToArray();
		}

		private void ChangeStateOnStateNameClick(object sender, EventArgs e)
		{
			var stateName = ((ToolStripButton) sender).Name;
			currentStateInd = stateBtns.IndexOfKey(stateName);
			ChangeState(stateName);
		}

		private void ChangeState(string stateName)
		{
			SetCurrentPathFinderFromStateName(stateName);
			RestartCurrentStateWithCurrentPathFinder();
		}

		private void SetCurrentPathFinderFromStateName(string stateName)
		{
			if (stateName.StartsWith("not_greedy"))
				currentPathFinder = new NotGreedyPathFinder();
			else
				currentPathFinder = new GreedyPathFinder();
			WritePathFinderTypeToControlsTexts();
		}

		private void RestartCurrentStateWithCurrentPathFinder()
		{
			CheckButton(stateBtns, stateBtns[currentStateInd].Name);
			statusBar.Text = "";
			scaledViewPanelPainter.ScaledViewPanel.FitToWindow = true;
			timer.Dispose();
			currentState = StatesLoader.LoadStateFromFolder(Folders.StatesForStudents, stateBtns[currentStateInd].Name);
			var mutableStateForStudent = new State(currentState);
			var path = currentPathFinder.FindPathToCompleteGoal(mutableStateForStudent);
			scaledViewPanelPainter.Controller = new StateController(currentState, path);
			InvalidateViews();
			StartNewTimer(scaledViewPanelPainter.Controller);
		}

		private void CheckButton(ToolStripItemCollection collection, string name)
		{
			foreach (var item in collection)
			{
				var btn = (ToolStripButton) item;
				btn.Checked = btn.Name == name;
			}
		}

		private void InvalidateViews()
		{
			scaledViewPanelPainter.ScaledViewPanel.Invalidate();
			var controller = scaledViewPanelPainter.Controller;
			var state = controller.State;
			statusBar.Text = $"Scores: {state.Scores} ";
			if (currentPathFinder is GreedyPathFinder)
				statusBar.Text += $"(Goal: {state.Goal}) ";
			statusBar.Text += $"Energy left: {state.Energy} (Out of: {state.InitialEnergy}) ";
			if (controller.GameIsWon)
			{
				statusBar.Text += " GAME WON !";
			}
			else if (controller.GameIsLost)
			{
				statusBar.Text += controller.GameLostMessage;
			}
		}

		private void StartNewTimer(StateController controller)
		{
			timer = new Timer();
			timer.Tick += (_, __) =>
			{
				timer.Interval = timerInterval;
				if (!controller.TryMoveOneStep())
				{
					timer.Dispose();
				}
				InvalidateViews();
			};
			timer.Start();
		}

		private void ChangePathFinderTypeAndRestart(object sender, EventArgs e)
		{
			currentPathFinder = SwitchPathFinderType();
			WritePathFinderTypeToControlsTexts();
			RestartCurrentStateWithCurrentPathFinder();
		}

		private IPathFinder SwitchPathFinderType()
		{
			if (currentPathFinder == null || currentPathFinder is NotGreedyPathFinder)
			{
				return new GreedyPathFinder();
			}
			if (currentState.Chests.Count > 11)
			{
				MessageBox.Show(this, "Won't run NotGreedy algorithm — too many chests for it", "Too many chests");
				return new GreedyPathFinder();
			}
			return new NotGreedyPathFinder();
		}

		private void WritePathFinderTypeToControlsTexts()
		{
			if (currentPathFinder is GreedyPathFinder)
			{
				Text = "Greedy Path Finder";
				pathFinderChangerBtn.Text = "Change to Not Greedy";
			}
			else
			{
				Text = "Not Greedy Path Finder";
				pathFinderChangerBtn.Text = "Change to Greedy";
			}
			Text += $" | {stateBtns[currentStateInd].Name}";
		}

		private void SwitchToNextState(object sender, EventArgs e)
		{
			currentStateInd = (currentStateInd + 1) % stateBtns.Count;
			ChangeState(stateBtns[currentStateInd].Name);
		}

		private void SwitchToPreviousState(object sender, EventArgs e)
		{
			if (--currentStateInd == -1)
				currentStateInd = stateBtns.Count - 1;
			ChangeState(stateBtns[currentStateInd].Name);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Oemplus || e.KeyCode == Keys.Add)
				timerInterval -= 100;
			if (e.KeyCode == Keys.OemMinus || e.KeyCode == Keys.Subtract)
				timerInterval += 100;
			if (e.KeyCode == Keys.R)
				RestartCurrentStateWithCurrentPathFinder();
			if (e.KeyCode == Keys.N)
				SwitchToNextState(new object(), new EventArgs());
			if (e.KeyCode == Keys.P)
				SwitchToPreviousState(new object(), new EventArgs());
		}
	}
}