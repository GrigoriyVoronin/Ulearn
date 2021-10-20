using System.Drawing;
using System.Linq;

namespace Greedy.Architecture.Drawing
{
    public static class PlayerImageProvider
    {
        private static Image lastImage;
        private static Point lastPosition;

        public static Image ProvidePlayerImage(StateController controller)
        {
            return lastImage = ProvidePlayerImageInternal(controller);
        }

        private static Image ProvidePlayerImageInternal(StateController controller)
        {
            if (controller.GameIsLost)
                return Sprites.PlayerDead;

            if (!controller.Path.Any() || !controller.MovementsToFrom.Any())
                return controller.GameIsWon ? Sprites.PlayerWon : Sprites.PlayerStanding;

            var movedTo = controller.State.Position;
            var movedFrom = controller.MovementsToFrom[controller.State.Position].Last();

            if (lastPosition == controller.State.Position)
                return lastImage;

            lastPosition = controller.State.Position;

            var movedRight = movedTo.X == movedFrom.X + 1;
            if (movedRight)
            {
                return lastImage == Sprites.PlayerRunningRight1
                    ? Sprites.PlayerRunningRight2
                    : Sprites.PlayerRunningRight1;
            }

            var movedLeft = movedTo.X == movedFrom.X - 1;
            if (movedLeft)
            {
                return lastImage == Sprites.PlayerRunningLeft1
                    ? Sprites.PlayerRunningLeft2
                    : Sprites.PlayerRunningLeft1;
            }

            var movedDown = movedTo.Y == movedFrom.Y + 1;
            var movedUp = movedTo.Y == movedFrom.Y - 1;
            if (movedDown || movedUp)
            {
                return lastImage == Sprites.PlayerClimbedDown1
                    ? Sprites.PlayerClimbedDown2
                    : Sprites.PlayerClimbedDown1;
            }

            return Sprites.PlayerStanding;
        }
    }
}