namespace MysteryHelper
{
    public enum ShreddingMethod { Random, Zeros };
    public static class ShreddingHelper
    {
        public static void ShredStream(System.IO.Stream source, int passes = 3, ShreddingMethod method = ShreddingMethod.Random, int chunkSize = 1024)
        {
            if (passes <= 0)
            {
                throw new System.Exception("Could not shred stream because passes was less than or equal to 0.");
            }
            else if (source is null)
            {
                throw new System.Exception("Could not shred stream because source is null.");
            }
            else if (!source.CanWrite)
            {
                throw new System.Exception("Could not shred stream because source is not writable.");
            }
            else if (!source.CanSeek)
            {
                throw new System.Exception("Could not shred stream because stream does not support seeking.");
            }
            if (method == ShreddingMethod.Zeros)
            {
                long currentIndex;
                long adjustedLength = source.Length - chunkSize;
                byte[] buffer = new byte[chunkSize];
                for (int pass = 0; pass < passes; pass++)
                {
                    currentIndex = 0;
                    source.Position = 0;
                    while (currentIndex < adjustedLength)
                    {
                        RandomnessHelper.NextBytes(buffer);
                        source.Write(buffer, 0, chunkSize);
                        currentIndex += chunkSize;
                    }
                    buffer = new byte[source.Length - (currentIndex + 1)];
                    RandomnessHelper.NextBytes(buffer);
                    source.Write(buffer, 0, chunkSize);
                    source.Flush();
                }
            }
            else
            {
                long currentIndex;
                long adjustedLength = source.Length - chunkSize;
                byte[] buffer = new byte[chunkSize];
                for (int pass = 0; pass < passes; pass++)
                {
                    currentIndex = 0;
                    source.Position = 0;
                    while (currentIndex < adjustedLength)
                    {
                        RandomnessHelper.NextBytes(buffer);
                        source.Write(buffer, 0, chunkSize);
                        currentIndex += chunkSize;
                    }
                    buffer = new byte[source.Length - (currentIndex + 1)];
                    RandomnessHelper.NextBytes(buffer);
                    source.Write(buffer, 0, chunkSize);
                    source.Flush();
                }
            }
        }
        public static void ShredFile(string filePath, int passes = 3, ShreddingMethod method = ShreddingMethod.Random, int chunkSize = 1024)
        {
            if (passes <= 0)
            {
                throw new System.Exception("Could not shred stream because passes was less than or equal to 0.");
            }
            else if (filePath is null || filePath == "")
            {
                throw new System.Exception("Could not shred file because file path is null or empty.");
            }
            else if (!System.IO.File.Exists(filePath))
            {
                throw new System.Exception("Could not shred file because file path does not exist.");
            }
            System.IO.FileStream fileStream = System.IO.File.Open(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Write);
            ShredStream(fileStream, passes, method, chunkSize);
            fileStream.Dispose();
            System.IO.File.Delete(filePath);
        }
        public static void ShredDirectory(string directoryPath, int passes = 3, ShreddingMethod method = ShreddingMethod.Random, int chunkSize = 1024)
        {
            if (passes <= 0)
            {
                throw new System.Exception("Could not shred directory because passes was less than or equal to 0.");
            }
            else if (directoryPath is null || directoryPath == "")
            {
                throw new System.Exception("Could not shred directory because directory path is null or empty.");
            }
            else if (!System.IO.Directory.Exists(directoryPath))
            {
                throw new System.Exception("Could not shred directory because directory path does not exist.");
            }
            foreach (string subDirectoryPath in System.IO.Directory.GetDirectories(directoryPath))
            {
                ShredDirectory(subDirectoryPath, passes, method, chunkSize);
            }
            foreach (string filePath in System.IO.Directory.GetFiles(directoryPath))
            {
                ShredFile(filePath, passes, method, chunkSize);
            }
            System.IO.Directory.Delete(directoryPath);
        }
    }
}