using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Globalization;

namespace SJP.Common.Extenders
{
    /// <summary>
    /// Contains extension methods for the <see cref="System.String"/> class
    /// </summary>
    public static class StringExtenders
    {
        /// <summary>
        /// Extension method to validate email address
        /// </summary>
        /// <param name="address">email address string to validate</param>
        /// <returns>true if the email address is valid otherwise false</returns>
        public static bool IsValidEmailAddress(this string address)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(address);
        }

        /// <summary>
        /// Extension method to validate United Kingdom postcodes
        /// </summary>
        /// <param name="postcode">United Kingdom postcode</param>
        /// <returns>true if the postcode is valid otherwise false</returns>
        public static bool IsValidPostcode(this string postcode)
        {
            Regex regex = new Regex(@"((\b[A-Z]{1,2}[0-9][A-Z0-9]?\s?[0-9][ABD-HJLNP-UW-Z]{2}\b)|(^([A-Z]{1}[0-9]{1})$|^([A-Z]{1}[0-9]{2})$|^([A-Z]{2}[0-9]{1})$|^([A-Z]{2}[0-9]{2})$))", RegexOptions.IgnoreCase);
            return regex.IsMatch(postcode);
        }

        /// <summary>
        /// Coverts string to other types i.e. int, double
        /// </summary>
        /// <typeparam name="T">Type the string converting to</typeparam>
        /// <param name="value">string value</param>
        /// <returns></returns>
        public static T Parse<T>(this string value)
        {
            // Get default value for type so if string
            // is empty then we can return default value.
            T result = default(T);

            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
                    result = (T)converter.ConvertFromString(value);

                }
                catch
                {
                    result = default(T);

                }
            }

            return result;
        }

        /// <summary>
        /// Coverts string to other types i.e. int, double
        /// </summary>
        /// <typeparam name="T">Type the string converting to</typeparam>
        /// <param name="value">String value</param>
        /// <param name="defaultValue">Default value to return when conversion fails</param>
        /// <returns></returns>
        public static T Parse<T>(this string value, T defaultValue)
        {
            // Get default value for type so if string
            // is empty then we can return default value.
            T result = default(T);

            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
                    result = (T)converter.ConvertFromString(value);

                }
                catch
                {
                    if (defaultValue != null)
                    {
                        result = defaultValue;
                    }
                    else
                    {
                        result = default(T);
                    }

                }
            }
            else
            {
                if (defaultValue != null)
                {
                    result = defaultValue;
                }
                else
                {
                    result = default(T);
                }
            }

            return result;
        }

        /// <summary>
        /// Coverts string to other types i.e. int, double
        /// </summary>
        /// <typeparam name="T">Type the string converting to</typeparam>
        /// <param name="value">String value</param>
        /// <param name="defaultValue">Default value to return when conversion fails</param>
        /// <returns></returns>
        public static T Parse<T>(this string value, T defaultValue, CultureInfo cultureInfo)
        {
            // Get default value for type so if string
            // is empty then we can return default value.
            T result = default(T);

            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
                    result = (T)converter.ConvertFromString(null, cultureInfo, value);

                }
                catch
                {
                    if (defaultValue != null)
                    {
                        result = defaultValue;
                    }
                    else
                    {
                        result = default(T);
                    }

                }
            }

            return result;
        }

        /// <summary>
        /// Makes the first character in the string Lower case
        /// </summary>
        /// <param name="text">String to change</param>
        /// <returns>String with first character as lower case</returns>
        public static string LowercaseFirst(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            char[] a = text.ToCharArray();
            a[0] = char.ToLower(a[0]);
            return new string(a);
        }

        /// <summary>
        /// Retrieves a substring from this instance. The substring starts at character position 0 and retrieves
        /// upto the specified length
        /// </summary>
        /// <param name="length">Length</param>
        /// <returns>String upto the specified length</returns>
        public static string SubstringFirst(this string text, int length)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (text.Length <= length)
            {
                return text;
            }
            else
            {
                return text.Substring(0, length);
            }
        }

        /// <summary>
        /// Retrieves a substring from this instance. The substring starts at character position 0 and retrieves
        /// upto the specified character. If not found, the string is returned
        /// </summary>
        /// <param name="character">character</param>
        /// <returns>String upto the specified character</returns>
        public static string SubstringFirst(this string text, char character)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (!text.Contains(character))
            {
                return text;
            }
            else
            {
                return text.Substring(0, text.IndexOf(character));
            }
        }

        /// <summary>
        /// Checks if the string matches the regular expression
        /// </summary>
        /// <param name="text">Text to match</param>
        /// <param name="regex">Regular expression</param>
        /// <returns>True if the text matches the regular expression</returns>
        public static bool MatchesRegex(this string text, string regex)
        {
            if (text == null || regex == null)
                return false;
            else
            {
                Match match = Regex.Match(text, regex);
                return match.Success;
            }
        }
    }
}
