using System;
using System.Drawing;
using System.IO;

namespace Greedy.Architecture.Drawing
{
    public static class Sprites
    {
        private static readonly string spritesFolder =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Architecture", "Drawing", "images");

        private static Size spriteSize => Wall.Size;

        public static readonly Bitmap Wall = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "wall.png")));

        public static readonly Bitmap PlayerStanding =
            new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "standing.png")), spriteSize);

        public static readonly Image PlayerWon =
            new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "has-won.png")), spriteSize);

        public static readonly Image PlayerDead =
            new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "is-dead.png")), spriteSize);

        public static readonly Image PlayerRunningRight1 =
            new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "running-right-1.png")), spriteSize);

        public static readonly Image PlayerRunningRight2 =
            new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "running-right-2.png")), spriteSize);

        public static readonly Image PlayerRunningLeft1 =
            new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "running-left-1.png")), spriteSize);

        public static readonly Image PlayerRunningLeft2 =
            new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "running-left-2.png")), spriteSize);

        public static readonly Image PlayerClimbedDown1 =
            new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "climbing-1.png")), spriteSize);

        public static readonly Image PlayerClimbedDown2 =
            new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "climbing-2.png")), spriteSize);

        public static readonly Image ChestOpened = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "chest-opened.png")), spriteSize);
        public static readonly Image ChestClosed = new Bitmap(Image.FromFile(Path.Combine(spritesFolder, "chest-closed.png")), spriteSize);
    }
}