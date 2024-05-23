namespace MysteryMemeware
{
    public static class AESHelper
    {
        public static void EncryptStream(byte[] key, System.IO.Stream source, System.IO.Stream destination)
        {
            if (source is null)
            {
                throw new System.Exception("Could not encrypt stream beacuse source is null.");
            }
            else if (!source.CanRead)
            {
                throw new System.Exception("Could not encrypt stream beacuse source is unreadable.");
            }
            else if (!source.CanSeek)
            {
                throw new System.Exception("Could not encrypt stream beacuse source does not support seeking.");
            }
            else if (destination is null)
            {
                throw new System.Exception("Could not encrypt stream beacuse destination is null.");
            }
            else if (!destination.CanWrite)
            {
                throw new System.Exception("Could not encrypt stream beacuse destination is unwriteable.");
            }
            else if (!destination.CanSeek)
            {
                throw new System.Exception("Could not encrypt stream beacuse destination does not support seeking.");
            }
            else if (key is null)
            {
                throw new System.Exception("Could not encrypt stream beacuse key is null.");
            }
            byte[] lengthAdjustedKey = SHA256Helper.HashBytes(key);
            source.Position = 0;
            destination.Position = 0;
            System.Security.Cryptography.Aes aes = System.Security.Cryptography.Aes.Create();
            aes.KeySize = 256;
            aes.Key = lengthAdjustedKey;
            System.Security.Cryptography.ICryptoTransform decryptor = aes.CreateEncryptor();
            System.Security.Cryptography.CryptoStream cryptoStream = new System.Security.Cryptography.CryptoStream(destination, decryptor, System.Security.Cryptography.CryptoStreamMode.Write);
            source.CopyTo(cryptoStream);
            cryptoStream.FlushFinalBlock();
            cryptoStream.Flush();
            cryptoStream.Dispose();
            decryptor.Dispose();
            aes.Dispose();
        }
        public static byte[] EncryptBytes(byte[] source, byte[] key)
        {
            if (source is null)
            {
                throw new System.Exception("Could not encrypt bytes beacuse source is null.");
            }
            else if (key is null)
            {
                throw new System.Exception("Could not encrypt bytes beacuse key is null.");
            }
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(source);
            EncryptStream(key, memoryStream, memoryStream);
            byte[] output = memoryStream.ToArray();
            memoryStream.Dispose();
            return output;
        }
        public static void DecryptStream(System.IO.Stream source, System.IO.Stream destination, byte[] key)
        {
            if (source is null)
            {
                throw new System.Exception("Could not decrypt stream beacuse source is null.");
            }
            else if (!source.CanRead)
            {
                throw new System.Exception("Could not decrypt stream beacuse source is unreadable.");
            }
            else if (!source.CanSeek)
            {
                throw new System.Exception("Could not decrypt stream beacuse source does not support seeking.");
            }
            else if (destination is null)
            {
                throw new System.Exception("Could not decrypt stream beacuse destination is null.");
            }
            else if (!destination.CanWrite)
            {
                throw new System.Exception("Could not decrypt stream beacuse destination is unwriteable.");
            }
            else if (!destination.CanSeek)
            {
                throw new System.Exception("Could not decrypt stream beacuse destination does not support seeking.");
            }
            else if (key is null)
            {
                throw new System.Exception("Could not decrypt stream beacuse key is null.");
            }
            byte[] lengthAdjustedKey = SHA256Helper.HashBytes(key);
            source.Position = 0;
            destination.Position = 0;
            System.Security.Cryptography.Aes aes = System.Security.Cryptography.Aes.Create();
            aes.KeySize = 256;
            aes.Key = lengthAdjustedKey;
            System.Security.Cryptography.ICryptoTransform decryptor = aes.CreateDecryptor();
            System.Security.Cryptography.CryptoStream cryptoStream = new System.Security.Cryptography.CryptoStream(destination, decryptor, System.Security.Cryptography.CryptoStreamMode.Write);
            source.CopyTo(cryptoStream);
            cryptoStream.FlushFinalBlock();
            cryptoStream.Flush();
            cryptoStream.Dispose();
            decryptor.Dispose();
            aes.Dispose();
        }
        public static byte[] DecryptBytes(byte[] source, byte[] key)
        {
            if (source is null)
            {
                throw new System.Exception("Could not decrypt bytes beacuse source is null.");
            }
            else if (key is null)
            {
                throw new System.Exception("Could not decrypt bytes beacuse key is null.");
            }
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(source);
            DecryptStream(memoryStream, memoryStream, key);
            byte[] output = memoryStream.ToArray();
            memoryStream.Dispose();
            return output;
        }
    }
}