using System;

namespace MyPhotoshop
{
    public struct Pixel
    {
        private double _blue;
        private double _green;
        private double _red;

        public Pixel(double red, double green, double blue)
        {
            _red = _green = _blue = 0;
            Red = red;
            Green = green;
            Blue = blue;
        }

        public static Pixel operator *(Pixel p, double c)
        {
            return new Pixel(
                Trim(p.Red * c),
                Trim(p.Green * c),
                Trim(p.Blue * c));
        }

        public static Pixel operator *(double c, Pixel p)
        {
            return p * c;
        }

        public double Red
        {
            get => _red;
            set
            {
                CheckColorValue(value);
                _red = value;
            }
        }


        public double Green
        {
            get => _green;
            set
            {
                CheckColorValue(value);
                _green = value;
            }
        }


        public double Blue
        {
            get => _blue;
            set
            {
                CheckColorValue(value);
                _blue = value;
            }
        }

        public static double Trim(double value)
        {
            if (value < 0) return 0;
            if (value > 1) return 1;
            return value;
        }

        private static void CheckColorValue(double value)
        {
            if (value < 0 || value > 1)
                throw new ArgumentOutOfRangeException(
                    $"Wrong channel value {value} (the value must be between 0 and 1");
        }
    }
}