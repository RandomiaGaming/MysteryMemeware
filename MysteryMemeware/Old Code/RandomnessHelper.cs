namespace MysteryMemeware
{
    public static class RandomnessHelper
    {
        private static System.Random RNG = new System.Random();
        public static int Next(int max)
        {
            if (max is 0)
            {
                return 0;
            }
            if (max < 0)
            {
                throw new System.Exception("max must be greater than or equal to 0.");
            }
            if (max is int.MaxValue)
            {
                throw new System.Exception("max must be less than int.MaxValue.");
            }
            return RNG.Next(max + 1);
        }
        public static int Next(int min, int max)
        {
            if (min == max)
            {
                return min;
            }
            if (min > max)
            {
                throw new System.Exception("min must be less than or equal to max.");
            }
            if (max is int.MaxValue)
            {
                throw new System.Exception("max must be less than int.MaxValue.");
            }
            return RNG.Next(min, max + 1);
        }
        public static byte NextByte()
        {
            byte[] buffer = new byte[1];
            RNG.NextBytes(buffer);
            return buffer[0];
        }
        public static byte[] NextBytes(int bufferSize)
        {
            if (bufferSize < 0)
            {
                throw new System.Exception("bufferSize must be greater than or equal to zero.");
            }
            else if (bufferSize == 0)
            {
                return new byte[0];
            }
            byte[] buffer = new byte[bufferSize];
            RNG.NextBytes(buffer);
            return buffer;
        }
        public static void NextBytes(byte[] buffer)
        {
            if (buffer is null)
            {
                throw new System.Exception("buffer cannot be null.");
            }
            if (buffer.Length is 0)
            {
                return;
            }
            RNG.NextBytes(buffer);
        }
        public static double NextDouble()
        {
            return RNG.NextDouble();
        }
        public static double NextDouble(double min, double max)
        {
            if (min is double.NaN || min is double.PositiveInfinity || min is double.NegativeInfinity)
            {
                throw new System.Exception("min must be a real number.");
            }
            if (max is double.NaN || max is double.PositiveInfinity || max is double.NegativeInfinity)
            {
                throw new System.Exception("max must be a real number.");
            }
            double ratio = RNG.NextDouble();
            double range = max - min;
            return min + (range * ratio);
        }
        public static string NextString(int length, string charset)
        {
            if (length < 0)
            {
                throw new System.Exception("length must be greater than 0.");
            }
            else if (charset is null || charset.Length is 0)
            {
                throw new System.Exception("charset cannot be null or empty.");
            }
            char[] outputArray = new char[length];
            for (int i = 0; i < length; i++)
            {
                outputArray[i] = charset[RNG.Next(charset.Length)];
            }
            return new string(outputArray);
        }
    }
}