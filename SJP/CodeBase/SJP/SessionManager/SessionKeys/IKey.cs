// *********************************************** 
// NAME             : IKey.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Mar 2011
// DESCRIPTION  	: IKey interface that defines the ID property
// which must be included in every TypeKey class defined in
// the Key.cs file.
// ************************************************
// 

namespace SJP.UserPortal.SessionManager
{
    /// <summary>
    /// Interface for type specific Key classes
    /// </summary>
    public interface IKey
    {
        // When implemented in Key classes this ID attribute is unique for each data type. 
        // This prevents keys of different types but the same name overwriting each other.
        string ID { get; }
    }
}
