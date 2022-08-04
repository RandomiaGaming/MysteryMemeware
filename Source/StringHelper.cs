//#Approve File 08/03/2022 11:35am.
namespace MysteryMemeware
{
    public static class StringHelper
    {
        public static string SelectAfterCaseless(string a, string b)
        {
            if (a is null || b is null)
            {
                return null;
            }
            for (int i = a.Length; i >= b.Length; i--)
            {
                if (MatchCaseless(a.Substring(i - b.Length, b.Length), b))
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
                if (MatchCaseless(a.Substring(i, b.Length), b))
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
                if (MatchCaseless(a, bElement))
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
        public static bool MatchCaseless(string a, string b)
        {
            if (a is null && b is null)
            {
                return true;
            }
            else if (a is null || b is null)
            {
                return false;
            }
            return a.ToLower() == b.ToLower();
        }
    }
}