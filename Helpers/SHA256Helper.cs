namespace MysteryMemeware
{
    public static class SHA256Helper
    {
        public static byte[] HashStream(System.IO.Stream source)
        {
            if (source is null)
            {
                throw new System.Exception("Could not hash stream because source is null.");
            }
            else if (!source.CanRead)
            {
                throw new System.Exception("Could not hash stream because source is not readable.");
            }
            else if (!source.CanSeek)
            {
                throw new System.Exception("Could not hash stream because source does not support seeking.");
            }
            System.Security.Cryptography.SHA256 hash = System.Security.Cryptography.SHA256.Create();
            source.Position = 0;
            byte[] output = hash.ComputeHash(source);
            hash.Dispose();
            return output;
        }
        public static byte[] HashBytes(byte[] source)
        {
            if (source is null)
            {
                throw new System.Exception("Could not hash bytes because source is null.");
            }
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(source);
            byte[] output = HashStream(memoryStream);
            memoryStream.Dispose();
            return output;
        }
    }
}