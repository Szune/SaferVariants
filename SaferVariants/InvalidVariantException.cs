using System;
using System.IO;

namespace SaferVariants
{
    /// <summary>
    /// The exception that is thrown when validation of an Option variant fails.
    /// </summary>
    public class InvalidOptionVariantException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidOptionVariantException"/> class.
        /// </summary>
        public InvalidOptionVariantException(string method, string filePath, int lineNumber) : base(
            $"{Path.GetFileName(filePath)}:{lineNumber} [{method}] Invalid Option variant.")
        {
        }
    }

    /// <summary>
    /// The exception that is thrown when validation of a Result variant fails.
    /// </summary>
    public class InvalidResultVariantException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidOptionVariantException"/> class.
        /// </summary>
        public InvalidResultVariantException(string method, string filePath, int lineNumber) : base(
            $"{Path.GetFileName(filePath)}:{lineNumber} [{method}] Invalid Result variant.")
        {
        }
    }
}