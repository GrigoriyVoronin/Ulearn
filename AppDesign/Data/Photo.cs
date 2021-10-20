namespace MyPhotoshop
{
    public class Photo
    {
        private readonly Pixel[,] _data;

        public Photo(int width, int height)
        {
            Width = width;
            Height = height;
            _data = new Pixel[width, height];
        }

        public int Width { get; }
        public int Height { get; }

        public Pixel this[int x, int y]
        {
            get => _data[x, y];
            set => _data[x, y] = value;
        }
    }
}