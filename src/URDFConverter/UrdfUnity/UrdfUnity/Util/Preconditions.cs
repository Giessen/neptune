﻿using System;

namespace UrdfUnity.Util
{
    /// <summary>
    /// Utility class for parameter validation.
    /// </summary>
    public static class Preconditions
    {
        /// <summary>
        /// Asserts that the object is not null, throwing an <c>ArgumentNullException</c> otherwise.
        /// </summary>
        /// <param name="obj">The object being validated as not null</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the object is null</exception>
        public static void IsNotNull(object obj)
        {
            IsNotNull(obj, null);
        }

        /// <summary>
        /// Asserts that the object is not null, throwing an <c>ArgumentNullException</c> with the provided parameter name otherwise.
        /// </summary>
        /// <param name="obj">The object being validated as not null</param>
        /// <param name="paramName">The name of the parameter being validated</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the object is null</exception>
        public static void IsNotNull(object obj, string paramName)
        {
            if (obj == null)
            {
                throw (paramName != null) ? new ArgumentNullException(paramName) : new ArgumentNullException();
            }
        }

        /// <summary>
        /// Asserts that the string is not empty, throwing an <c>ArgumentException</c> otherwise.
        /// </summary>
        /// <param name="str">The string being validated as not empty</param>
        /// <exception cref="System.ArgumentException">Thrown when the string is empty</exception>
        public static void IsNotEmpty(string str)
        {
            IsNotEmpty(str, null);
        }

        /// <summary>
        /// Asserts that the string is not empty, throwing an <c>ArgumentException</c> with the provided parameter name otherwise.
        /// </summary>
        /// <param name="str">The string being validated as not empty</param>
        /// <param name="paramName">The name of the parameter being validated</param>
        /// <exception cref="System.ArgumentException">Thrown when the string is empty</exception>
        public static void IsNotEmpty(string str, string paramName)
        {
            if (str == null || str.Length == 0 || str.Equals(String.Empty))
            {
                throw (paramName != null) ? new ArgumentException(paramName) : new ArgumentException();
            }
        }

        /// <summary>
        /// Asserts that the value is within the range of the specified lower and upper boundaries inclusive.
        /// </summary>
        /// <param name="value">The value being validated as in the range [lowerBound, upperBound]</param>
        /// <param name="lowerBound">The lower boundary of the range being checked</param>
        /// <param name="upperBound">The upper boundary of the range being checked</param>
        /// <exception cref="System.Exception">Thrown when the lowerBound parameter is greater than the upperBound parameter</exception>"
        /// <exception cref="System.ArgumentException">Thrown when the value is outside the specified range</exception>
        public static void IsWithinRange(double value, double lowerBound, double upperBound)
        {
            IsWithinRange(value, lowerBound, upperBound, null);
        }

        /// <summary>
        /// Asserts that the value is within the range of the specified lower and upper boundaries inclusive.
        /// </summary>
        /// <param name="value">The value being validated as in the range [lowerBound, upperBound]</param>
        /// <param name="lowerBound">The lower boundary of the range being checked</param>
        /// <param name="upperBound">The upper boundary of the range being checked</param>
        /// <param name="paramName">The name of the parameter being validated</param>
        /// <exception cref="System.Exception">Thrown when the lowerBound parameter is greater than the upperBound parameter</exception>"
        /// <exception cref="System.ArgumentException">Thrown when the value is outside the specified range</exception>
        public static void IsWithinRange(double value, double lowerBound, double upperBound, string paramName)
        {
            if (lowerBound > upperBound)
            {
                throw new Exception("Range validation cannot have lower bound that is greater than upper bound");
            }
            if (value < lowerBound || value > upperBound)
            {
                throw (paramName != null) ? new ArgumentException(paramName) : new ArgumentException();
            }
        }
    }
}