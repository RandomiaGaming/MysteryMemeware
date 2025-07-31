namespace MysteryHelper
{
	public static class ArrayHelper
	{
		public static T[] ConcatArrays<T>(T[] arrayA, T[] arrayB)
		{
			if (arrayA is null)
			{
				throw new System.Exception("Could not merge arrays because arrayA is null.");
			}
			else if (arrayB is null)
			{
				throw new System.Exception("Could not merge arrays because arrayB is null.");
			}
			else if (arrayA.Length == 0)
			{
				return arrayB;
			}
			else if (arrayB.Length == 0)
			{
				return arrayA;
			}
			T[] output = new T[arrayA.Length + arrayB.Length];
			System.Array.Copy(arrayA, 0, output, 0, arrayA.Length);
			System.Array.Copy(arrayB, 0, output, arrayA.Length, arrayB.Length);
			return output;
		}
		public static T[] SubArray<T>(T[] source, int startIndex, int length)
		{
			if (source is null)
			{
				throw new System.Exception("Could not get range from array because source is null.");
			}
			else if (startIndex < 0 || startIndex >= source.Length)
			{
				throw new System.Exception("Could not get range from array because startIndex was outside the bounds of the array.");
			}
			else if (length <= 0)
			{
				throw new System.Exception("Could not get range from array because length was less than or equal to 0.");
			}
			else if (source.Length < startIndex + length)
			{
				throw new System.Exception("Could not get range from array because the specified range was too large to fit within the bounds of the array.");
			}
			T[] output = new T[length];
			System.Array.Copy(source, startIndex, output, 0, length);
			return output;
		}
		public static bool ArraysEqual<T>(T[] arrayA, T[] arrayB)
		{
			if (arrayA is null && arrayB is null)
			{
				return true;
			}
			else if (arrayA is null || arrayB is null)
			{
				return false;
			}
			else if (arrayA.Length != arrayB.Length)
			{
				return false;
			}
			for (int i = 0; i < arrayA.Length; i++)
			{
				object elementA = arrayA[i];
				object elementB = arrayB[i];
				if (elementA != elementB)
				{
					return false;
				}
			}
			return true;
		}
		public static T[] CloneArray<T>(T[] source)
		{
			if (source is null)
			{
				return null;
			}
			T[] output = new T[source.Length];
			System.Array.Copy(source, 0, output, 0, source.Length);
			return output;
		}
		public static T[] AddToArray<T>(T[] source, T element)
		{
			if (source is null)
			{
				throw new System.Exception("Could not add element to array because source is null.");
			}
			T[] output = new T[source.Length + 1];
			System.Array.Copy(source, 0, output, 0, source.Length);
			output[source.Length] = element;
			return output;
		}
		public static T[] RemoveFromArray<T>(T[] source, int elementIndex)
		{
			if (source is null)
			{
				throw new System.Exception("Could not add element to array because source is null.");
			}
			else if (elementIndex < 0 || elementIndex >= source.Length)
			{
				throw new System.Exception("Could not remove element from array because elementIndex was outside the bounds of the array.");
			}
			T[] output = new T[source.Length - 1];
			System.Array.Copy(source, 0, output, 0, elementIndex);
			System.Array.Copy(source, elementIndex + 1, output, elementIndex, source.Length - elementIndex - 1);
			return output;
		}
	}
}