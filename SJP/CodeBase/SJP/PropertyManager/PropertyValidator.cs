// *********************************************** 
// NAME             : PropertyValidator.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 16 Feb 2011
// DESCRIPTION  	: Abstract property validator class provides a basic helper methods 
//                    to help validating properties 
// ************************************************


using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace SJP.Common.PropertyManager
{
    /// <summary>
    /// Property Validator class
    /// </summary>
    public abstract class PropertyValidator
    {
        #region Enums
        /// <summary>
        /// Enum determining level of optionality for the properties
        /// </summary>
        protected enum Optionality : int
        {
            Optional,
            Mandatory,
            Undefined
        }

        #endregion

        #region Protected Fields
        protected IPropertyProvider properties;
        #endregion

        #region Constructors
        /// <summary>
        /// PropertyValidator Constructor
        /// </summary>
        /// <param name="properties">Property store provider object</param>
        public PropertyValidator(IPropertyProvider properties)
        {
            this.properties = properties;
        }
        #endregion

        #region Abstract Methods
        /// <summary>
        /// Abstract method to implement custom logic for validating properties
        /// </summary>
        /// <param name="key">Key to uniquely identify property</param>
        /// <param name="errors">Error list object to store any errors generated while validating property</param>
        /// <returns>True if property is valid otherwise false</returns>
        abstract public bool ValidateProperty(string key, List<string> errors);

        #endregion


        #region Protected Methods

        /// <summary>
        /// method that checks a class defined in the properties by a key exists in a given assembly
        /// </summary>
        /// <param name="key">Key to uniquely identify property</param>
        /// <param name="myAssembly">Assembly to check for existence of a class defined in properties</param>
        /// <param name="errors">Error list object to store any errors generated while validating property</param>
        /// <returns>returs true if exists false otherwise</returns>
        protected bool ValidateClassExists(string key, Assembly myAssembly, List<string> errors)
        {
            bool exists = false;
            string className = properties[key];
            bool validated = false;

            try
            {
                Type[] assemblyTypes = myAssembly.GetTypes();

                foreach (Type assemblyType in assemblyTypes)
                {
                    if (assemblyType.Name == className)
                    {
                        exists = true;
                    }
                }

                validated = true;
            }
            catch (Exception ex) // no specific exceptions documented so catch all
            {
                if (myAssembly == null)
                {
                    errors.Add(String.Format(SJP.Common.Messages.BadAssembly, key, ex.Message));
                }
                else
                {
                    errors.Add(String.Format(SJP.Common.Messages.AssemblyTypeReadFail, myAssembly.FullName, key));
                }
            }

            if ((!exists) && (validated))
            {
                errors.Add(String.Format(SJP.Common.Messages.ClassNotFoundInAssembly,
                           className,
                           key,
                           myAssembly.FullName));
            }

            return exists;
        }

        /// <summary>
        /// Return the assembly against an assembly name stored in the properties
        /// </summary>
        /// <param name="key">Key to uniquely identify property</param>
        /// <param name="errors">Error list object to store any errors generated while validating property</param>
        /// <returns>Return an assembly if exists</returns>
        protected Assembly ValidateAssembly(string key, List<string> errors)
        {
            string property = properties[key];
            Assembly assembly = null;

            try
            {
                assembly = Assembly.Load(property);
            }
            catch (ArgumentNullException argumentNullException)
            {
                errors.Add(String.Format(Messages.BadAssembly,
                    key, argumentNullException.Message));
            }
            catch (FileNotFoundException fileNotFoundException)
            {
                errors.Add(String.Format(Messages.BadAssembly,
                    key, fileNotFoundException.Message));
            }
            catch (BadImageFormatException badImageFormatException)
            {
                errors.Add(String.Format(Messages.BadAssembly,
                    key, badImageFormatException.Message));
            }
            catch (Exception exception)// catch undocumented exceptions
            {
                errors.Add(String.Format(Messages.BadAssembly,
                    key, exception.Message));
            }

            return assembly;
        }

        /// <summary>
        /// Checks first if the property at the given key is defined when the property is mandatory.
        /// Validates also if an enum property in the properties exists in a given type.
        /// </summary>
        /// <param name="key">Key to uniquely identify property</param>
        /// <param name="t">Type of object</param>
        /// <param name="optionality">Leve of requirement i.e. optional, mendatory, etc.</param>
        /// <param name="errors">Error list object to store any errors generated while validating property</param>
        /// <returns>return true if the property exist in the type</returns>
        protected bool ValidateEnumProperty(string key,
            Type t,
            Optionality optionality,
            List<string> errors)
        {
            string property = properties[key];
            bool Valid = true;

            if (property == null)
            {
                if (optionality.Equals(Optionality.Mandatory))
                {
                    errors.Add(String.Format(Messages.PropertyValueMissing, key));
                    Valid = false;
                }
            }
            else
            {
                bool found = false;
                StringBuilder usage = new StringBuilder();

                foreach (FieldInfo fieldInfo in t.GetFields())
                {
                    if (fieldInfo.Name == property)
                        found = true;

                    usage.Append(fieldInfo.Name);
                    usage.Append("|");
                }

                if (!found)
                {
                    Valid = false;
                    usage.Replace("value__", "");
                    errors.Add(String.Format(Messages.PropertyValueBad, key, property, usage));
                }
            }

            return Valid;
        }

        /// <summary>
        /// checks that a property at a given key is defined when the property is mandatory/ undefined optionality
        /// </summary>
        /// <param name="key">Key to uniquely identify property</param>
        /// <param name="optionality">Leve of requirement i.e. optional, mendatory, etc.</param>
        /// <param name="errors">Error list object to store any errors generated while validating property</param>
        /// <returns></returns>
        protected bool ValidateExistence(string key, Optionality optionality, List<string> errors)
        {
            bool Valid = true;

            if (optionality.Equals(Optionality.Mandatory))
            {
                string property = properties[key];
                if (property == null)
                {
                    Valid = false;
                    errors.Add(String.Format(Messages.PropertyKeyMissing, key));
                }
            }

            if (optionality.Equals(Optionality.Undefined))
            {
                string property = properties[key];
                if (property != null)
                {
                    Valid = false;
                    errors.Add(String.Format(Messages.InvalidPropertyKey, key));
                }
            }

            return Valid;
        }

        /// <summary>
        /// Checks that a property at given key does not exceed the min/max boundaries
        /// </summary>
        /// <param name="key">Key to uniquely identify property</param>
        /// <param name="min">Minimum boundary value for value of property</param>
        /// <param name="max">Maximum boundary value for value of property</param>
        /// <param name="errors">Error list object to store any errors generated while validating property</param>
        /// <returns>returns true if the length is valid, false otherwise</returns>
        protected bool ValidateLength(string key, int min, int max, List<string> errors)
        {
            bool valid = true;
            string property = properties[key];
            int length = property.Length;

            if ((length < min) || (length > max))
            {
                valid = false;
                errors.Add(String.Format(Messages.InvalidPropertyLength,
                    property, key, min.ToString(CultureInfo.CurrentCulture.NumberFormat), max.ToString(CultureInfo.CurrentCulture.NumberFormat)));
            }

            return valid;
        }

        /// <summary>
        /// Checks that a property at a given key does not exceed the min boudary.
        /// </summary>
        /// <param name="key">Key to uniquely identify property</param>
        /// <param name="min">Minimum boundary value for value of property</param>
        /// <param name="errors">Error list object to store any errors generated while validating property</param>
        /// <returns>returns true if the length is valid, false otherwise</returns>
        protected bool ValidateLength(string key, int min, List<string> errors)
        {
            bool valid = true;
            string property = properties[key];
            int length = property.Length;

            if (length < min)
            {
                valid = false;
                errors.Add(String.Format(Messages.InvalidPropertyLengthMin,
                    property, key, min.ToString(CultureInfo.CurrentCulture.NumberFormat)));
            }

            return valid;
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Returns the Enum value from a string in a given type
        /// </summary>
        /// <param name="t">
        /// The enum type.
        /// </param>
        /// <param name="strValue">
        /// The value to convert to the given enum type.
        /// </param>
        /// <returns>
        /// Returns the Enum type if exists, throws an exception otherwise
        /// </returns>
        /// <exception cref="SJPException">
        /// Value to convert does not have a matching enum value.
        /// </exception>
        public static object StringToEnum(Type t, string strValue)
        {
            foreach (FieldInfo fieldInfo in t.GetFields())
                if (fieldInfo.Name == strValue)
                    return fieldInfo.GetValue(null);

            throw new SJPException(String.Format(Messages.EnumConversionError,
                strValue, t.FullName), false, SJPExceptionIdentifier.PSBadEnum);
        }

        /// <summary>
        /// Checks to see if a string passed is a Whole number
        /// </summary>
        /// <param name="strNumber">String to evaluate</param>
        /// <returns>True if string is whole number.</returns>
        public static bool IsWholeNumber(String strNumber)
        {
            Regex objNotWholePattern = new Regex("[^0-9]");
            return !objNotWholePattern.IsMatch(strNumber);
        }

        #endregion

    }
}
