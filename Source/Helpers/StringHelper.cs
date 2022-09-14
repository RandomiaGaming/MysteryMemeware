using System;
namespace MysteryMemeware.Helpers
{
    public static class StringHelper
    {
        public static bool StringContainsChar(string target, char character)
        {
            if (target is null || target is "")
            {
                return false;
            }
            for (int i = 0; i < target.Length; i++)
            {
                if (target[i] == character)
                {
                    return true;
                }
            }
            return false;
        }
        public static string ReplaceCaseless(string a, string b, string c)
        {
            if (a is null)
            {
                return "";
            }
            if (b is null || b is "")
            {
                return a;
            }
            if (c is null)
            {
                c = "";
            }
            if (ContainsCaseless(c, b))
            {
                throw new Exception("c cannot contain b.");
            }
            for (int i = 0; i < a.Length - b.Length + 1; i++)
            {
                var t = SafeSubstring(a, i, i + b.Length - 1);
                if (MatchesCaseless(t, b))
                {
                    var d = SafeSubstring(a, 0, i - 1);
                    var e = SafeSubstring(a, i + b.Length - 1, a.Length - 1);
                    a = d + c + e;
                }
            }
            return a;
        }
        public static bool ContainsCaseless(string a, string b)
        {
            if (a is null && b is null)
            {
                return true;
            }
            else if (a is null || b is null)
            {
                return false;
            }
            if (b.Length > a.Length)
            {
                return false;
            }
            for (int i = 0; i < a.Length - b.Length; i++)
            {
                if (MatchesCaseless(SafeSubstring(a, i, i + b.Length - 1), b))
                {
                    return true;
                }
            }
            return false;
        }
        public static string SafeSubstring(string a, int startIndex, int endIndex)
        {
            if(startIndex == endIndex)
            {
                return "";
            }
            if (startIndex > endIndex)
            {
                int temp = startIndex;
                startIndex = endIndex;
                endIndex = temp;
            }
            if (startIndex < 0)
            {
                startIndex = 0;
            }
            if (endIndex >= a.Length)
            {
                endIndex = a.Length - 1;
            }
            if (a is null || a is "")
            {
                return "";
            }
            return a.Substring(startIndex, endIndex - startIndex + 1);
        }
        public static string SelectAfterCaseless(string a, string b)
        {
            if (a is null || b is null)
            {
                return null;
            }
            for (int i = a.Length; i >= b.Length; i--)
            {
                if (MatchesCaseless(a.Substring(i - b.Length, b.Length), b))
                {
                    return a.Substring(i, a.Length - i);
                }
            }
            return null;
        }
        public static string SelectAfter(string a, string b)
        {
            if (a is null || b is null)
            {
                return null;
            }
            for (int i = a.Length; i >= b.Length; i--)
            {
                if (a.Substring(i - b.Length, b.Length) == b)
                {
                    return a.Substring(i, a.Length - i);
                }
            }
            return null;
        }
        public static string SelectBeforeCaseless(string a, string b)
        {
            if (a is null || b is null)
            {
                return null;
            }
            for (int i = 0; i < a.Length - b.Length; i++)
            {
                if (MatchesCaseless(a.Substring(i, b.Length), b))
                {
                    return a.Substring(0, i);
                }
            }
            return null;
        }
        public static string SelectBefore(string a, string b)
        {
            if (a is null || b is null)
            {
                return null;
            }
            for (int i = 0; i < a.Length - b.Length; i++)
            {
                if (a.Substring(i, b.Length) == b)
                {
                    return a.Substring(0, i);
                }
            }
            return null;
        }
        public static bool EndsWithArrayCaseless(string a, string[] b)
        {
            foreach (string bElement in b)
            {
                if (EndsWithCaseless(a, bElement))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool EndsWithArray(string a, string[] b)
        {
            foreach (string bElement in b)
            {
                if (EndsWithSafe(a, bElement))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool StartsWithArrayCaseless(string a, string[] b)
        {
            foreach (string bElement in b)
            {
                if (StartsWithCaseless(a, bElement))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool StartsWithArray(string a, string[] b)
        {
            foreach (string bElement in b)
            {
                if (StartsWithSafe(a, bElement))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool EndsWithCaseless(string a, string b)
        {
            if (a is null && b is null)
            {
                return true;
            }
            else if (a is null || b is null)
            {
                return false;
            }
            return a.ToLower().EndsWith(b.ToLower());
        }
        public static bool StartsWithCaseless(string a, string b)
        {
            if (a is null && b is null)
            {
                return true;
            }
            else if (a is null || b is null)
            {
                return false;
            }
            return a.ToLower().StartsWith(b.ToLower());
        }
        public static bool EndsWithSafe(string a, string b)
        {
            if (a is null && b is null)
            {
                return true;
            }
            else if (a is null || b is null)
            {
                return false;
            }
            return a.EndsWith(b);
        }
        public static bool StartsWithSafe(string a, string b)
        {
            if (a is null && b is null)
            {
                return true;
            }
            else if (a is null || b is null)
            {
                return false;
            }
            return a.StartsWith(b);
        }
        public static bool MatchesArrayCaseless(string a, string[] b)
        {
            foreach (string bElement in b)
            {
                if (MatchesCaseless(a, bElement))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool MatchesArray(string a, string[] b)
        {
            foreach (string bElement in b)
            {
                if (a == bElement)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool MatchesCaseless(string a, string b)
        {
            if (a is null && b is null)
            {
                return true;
            }
            else if (a is null || b is null)
            {
                return false;
            }
            else if (a.Length != b.Length)
            {
                return false;
            }
            else
            {
                return a.ToLower() == b.ToLower();
            }
        }
    }
}