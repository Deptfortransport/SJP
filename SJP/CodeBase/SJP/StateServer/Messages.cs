// *********************************************** 
// NAME             : Messages.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Messages class containing state server messages
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.UserPortal.StateServer
{
    /// <summary>
    /// Messages class containing state server messages
    /// </summary>
    public class Messages
    {
        public const string RetryAttemptObjectLocked = "StateServer object with key[{0}] locked, retrying attempt {1}.";
        public const string FailedLocking = "Failed locking object with key[{0}]";
        public const string ErrorLocking = "Error locking object with key[{0}] in StateServer: {1}.";
        public const string ErrorSaving = "Error saving object with key[{0}] to StateServer: {1}.";
        public const string ErrorReading = "Error reading object with key[{0}] from StateServer: {1}.";
        public const string ErrorDeleting = "Error deleting object with key[{0}] from StateServer: {1}.";
        public const string DeleteWhenExpires = "Object will be deleted when it expires";

        static Messages()
        {
        }
    }
}
