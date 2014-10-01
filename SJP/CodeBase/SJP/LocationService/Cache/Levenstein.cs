// *********************************************** 
// NAME             : Levenstein.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 07 Jun 2011
// DESCRIPTION  	: Implements Levenstein distance algorithm to find similarity between the strings
//                    ref : http://anastasiosyal.com/archive/2009/01/11/18.aspx
//                          http://staffwww.dcs.shef.ac.uk/people/sam.chapman@k-now.co.uk/simmetrics.html
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.Common.LocationService
{
    /// <summary>
    /// Implements Levenstein distance algorithm to find similarity between the strings
    /// </summary>
    public sealed class Levenstein
    {
        #region Constants
        const double defaultPerfectMatchScore = 1.0;
        const double defaultMismatchScore = 0.0;
        const int charExactMatchScore = 1;
        const int charMismatchMatchScore = 0;
        #endregion
               
        #region Public Methods
        /// <summary>
        /// gets the similarity of the two strings using levenstein distance.
        /// </summary>
        /// <param name="firstWord">first word</param>
        /// <param name="secondWord">second word</param>
        /// <returns>a value between 0-1 of the similarity</returns>
        public double GetSimilarity(string firstWord, string secondWord)
        {
            if ((firstWord != null) && (secondWord != null))
            {
                double levensteinDistance = GetUnnormalisedSimilarity(firstWord, secondWord);
                double maxLen = firstWord.Length;
                if (maxLen < secondWord.Length)
                {
                    maxLen = secondWord.Length;
                }
                if (maxLen == defaultMismatchScore)
                {
                    return defaultPerfectMatchScore;
                }
                else
                {
                    return defaultPerfectMatchScore - levensteinDistance / maxLen;
                }
            }
            return defaultMismatchScore;
        }

        /// <summary> 
        /// gets the un-normalised similarity measure of the metric for the given strings.</summary>
        /// <param name="firstWord"></param>
        /// <param name="secondWord"></param>
        /// <returns> returns the score of the similarity measure (un-normalised)</returns>
        /// <remarks>
        /// <p/>
        /// Copy character from string1 over to string2 (cost 0)
        /// Delete a character in string1 (cost 1)
        /// Insert a character in string2 (cost 1)
        /// Substitute one character for another (cost 1)
        /// <p/>
        /// D(i-1,j-1) + d(si,tj) //subst/copy
        /// D(i,j) = min D(i-1,j)+1 //insert
        /// D(i,j-1)+1 //delete
        /// <p/>
        /// d(i,j) is a function whereby d(c,d)=0 if c=d, 1 else.
        /// </remarks>
        public double GetUnnormalisedSimilarity(string firstWord, string secondWord)
        {
            if ((firstWord != null) && (secondWord != null))
            {
                // Step 1
                int n = firstWord.Length;
                int m = secondWord.Length;
                if (n == 0)
                {
                    return m;
                }
                if (m == 0)
                {
                    return n;
                }

                double[][] d = new double[n + 1][];
                for (int i = 0; i < n + 1; i++)
                {
                    d[i] = new double[m + 1];
                }

                // Step 2
                for (int i = 0; i <= n; i++)
                {
                    d[i][0] = i;
                }
                for (int j = 0; j <= m; j++)
                {
                    d[0][j] = j;
                }

                // Step 3
                for (int i = 1; i <= n; i++)
                {
                    // Step 4
                    for (int j = 1; j <= m; j++)
                    {
                        // Step 5
                        double cost = GetCost(firstWord, i - 1, secondWord, j - 1);
                        // Step 6
                        d[i][j] = MinOf3(d[i - 1][j] + 1.0, d[i][j - 1] + 1.0, d[i - 1][j - 1] + cost);
                    }
                }

                // Step 7
                return d[n][m];
            }
            return 0.0;
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// get cost between characters where d(i,j) = 1 if i does not equals j, 0 if i equals j.
        /// </summary>
        /// <param name="firstWord">the string1 to evaluate the cost</param>
        /// <param name="firstWordIndex">the index within the string1 to test</param>
        /// <param name="secondWord">the string2 to evaluate the cost</param>
        /// <param name="secondWordIndex">the index within the string2 to test</param>
        /// <returns>the cost of a given subsitution d(i,j) where d(i,j) = 1 if i!=j, 0 if i==j</returns>
        public double GetCost(string firstWord, int firstWordIndex, string secondWord, int secondWordIndex)
        {
            if ((firstWord != null) && (secondWord != null))
            {
                return firstWord[firstWordIndex] != secondWord[secondWordIndex] ? charExactMatchScore : charMismatchMatchScore;
            }
            return 0.0;
        }


        /// <summary>
        /// returns the min of three numbers.
        /// </summary>
        /// <param name="firstNumber">first number to test</param>
        /// <param name="secondNumber">second number to test</param>
        /// <param name="thirdNumber">third number to test</param>
        /// <returns>the min of three numbers.</returns>
        private double MinOf3(double firstNumber, double secondNumber, double thirdNumber)
        {
            return Math.Min(firstNumber, Math.Min(secondNumber, thirdNumber));
        }
        #endregion


    }
}
