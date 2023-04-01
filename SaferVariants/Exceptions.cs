using System;

namespace SaferVariants
{
    /// <summary>
    /// The exception that is thrown when trying to get a value that is not present in an <see cref="Option"/>.
    /// </summary>
    public class ValueNotPresentException : SaferVariantsException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueNotPresentException"/> class.
        /// </summary>
        public ValueNotPresentException()
        {
        }
    }

    /// <summary>
    /// The exception that is thrown when trying to get a success value that is not present in a <see cref="Result"/>.
    /// </summary>
    public class SuccessValueNotPresentException : SaferVariantsException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SuccessValueNotPresentException"/> class.
        /// </summary>
        public SuccessValueNotPresentException()
        {
        }
    }

    /// <summary>
    /// The exception that is thrown when trying to get an error value that is not present in a <see cref="Result"/>.
    /// </summary>
    public class ErrorValueNotPresentException : SaferVariantsException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorValueNotPresentException"/> class.
        /// </summary>
        public ErrorValueNotPresentException()
        {
        }
    }

    /// <summary>
    /// The exception that is thrown when trying to get a value that is null.
    /// </summary>
    public class ValueNullException : SaferVariantsException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueNullException"/> class.
        /// </summary>
        public ValueNullException()
        {
        }
    }

    /// <summary>
    /// The catch-all exception for SaferVariants-specific exceptions.
    /// </summary>
    public class SaferVariantsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SaferVariantsException"/> class.
        /// </summary>
        public SaferVariantsException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SaferVariantsException"/> class.
        /// </summary>
        public SaferVariantsException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SaferVariantsException"/> class.
        /// </summary>
        public SaferVariantsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
