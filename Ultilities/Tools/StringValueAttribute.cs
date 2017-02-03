using System;
using System.Reflection;

namespace Utilities.Tools
{
    /// <summary>
    /// Represents a string value for an enum value.
    /// </summary>
    public class StringValueAttribute : Attribute
    {
        #region Properties

        public string StringValue { get; protected set; }

        #endregion

        #region Constructors

        public StringValueAttribute(string value)
        {
            StringValue = value;
        }

        #endregion
    }

    public static class EnumString
    {
        #region Methods

        /// <summary>
        /// Will get the string value for a given enum value attribute.
        /// It will only work if the enum value has a StringValue attribute assigned.
        /// </summary>
        /// <param name="value">The Enum value.</param>
        /// <returns>The StringValue attribute as a string.</returns>
        public static string GetStringValue(this Enum value)
        {
            Type type = value.GetType();
            FieldInfo fieldInfo = type.GetField(value.ToString());
            StringValueAttribute[] attributes =
                fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
            return attributes != null && attributes.Length > 0 ? attributes[0].StringValue : null;
        }

        #endregion
    }
}
