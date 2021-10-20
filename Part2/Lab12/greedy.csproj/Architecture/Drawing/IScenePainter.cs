using System.Drawing;

namespace Greedy.Architecture.Drawing
{
	public interface IScenePainter
	{
		SizeF Size { get; }
		int CellSize { get; }
		void Paint(Graphics g, float zoomScale);
	}
}