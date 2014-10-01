// *********************************************** 
// NAME             : SJPMessageType.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 25 Mar 2011
// DESCRIPTION  	: Enumeration of SJPMessageTypes
// ************************************************
// 

using System;

namespace SJP.Common
{
    /// <summary>
    /// Enumeration of SJPMessageTypes
    /// </summary>
    [Serializable()]
    public enum SJPMessageType
    {
        Info,
        Warning,
        Error
    }
}