﻿namespace MysteryHelper
{
    public static class StringEncodingHelper
    {
        public static byte[] StringToBytes(string source)
        {
            return StringToBytesUnicode(source);
        }
        public static string BytesToString(byte[] source)
        {
            return BytesToStringUnicode(source);
        }
        public static byte[] StringToBytesUnicode(string source)
        {
            return System.Text.Encoding.Unicode.GetBytes(source);
        }
        public static string BytesToStringUnicode(byte[] source)
        {
            return System.Text.Encoding.Unicode.GetString(source);
        }
        public static byte[] StringToBytesASCII(string source)
        {
            return System.Text.Encoding.ASCII.GetBytes(source);
        }
        public static string BytesToStringASCII(byte[] source)
        {
            return System.Text.Encoding.ASCII.GetString(source);
        }
        public static byte[] StringToBytesUTF8(string source)
        {
            return System.Text.Encoding.UTF8.GetBytes(source);
        }
        public static string BytesToStringUTF8(byte[] source)
        {
            return System.Text.Encoding.UTF8.GetString(source);
        }
        public static byte[] StringToBytesUTF32(string source)
        {
            return System.Text.Encoding.UTF32.GetBytes(source);
        }
        public static string BytesToStringUTF32(byte[] source)
        {
            return System.Text.Encoding.UTF32.GetString(source);
        }
    }
}