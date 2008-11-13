using System;

namespace com.mosso.cloudfs.exceptions
{
    /// <summary>
    /// This exception is thrown when an account has no containers
    /// </summary>
    public class NoContainersFoundException : Exception
    {

        /// <summary>
        /// The default constructor
        /// </summary>
        public NoContainersFoundException()
        {
        }
        /// <summary>
        /// An alternate constructor indicating more explicitly the reason for failure
        /// </summary>
        /// <param name="msg">The message detailing the failure</param>
        public NoContainersFoundException(string msg) : base(msg)
        {
        }
    }
}