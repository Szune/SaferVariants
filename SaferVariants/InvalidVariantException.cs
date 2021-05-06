using System;
using System.IO;

namespace SaferVariants
{
    public class InvalidOptionVariantException : Exception
    {
        public InvalidOptionVariantException(string method, string filePath, int lineNumber) : base(
            $"{Path.GetFileName(filePath)}:{lineNumber} [{method}] Invalid Option variant.")
        {
        }
    }

    public class InvalidResultVariantException : Exception
    {
        public InvalidResultVariantException(string method, string filePath, int lineNumber) : base(
            $"{Path.GetFileName(filePath)}:{lineNumber} [{method}] Invalid Result variant.")
        {
        }
    }
}