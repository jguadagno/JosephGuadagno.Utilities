using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace JosephGuadagno.Utilities.Extensions
{
	public static class ObjectExtensions
	{
		public static void TrimAllStrings(this object objectToTrim)
		{
			if (objectToTrim == null)
				return;

			foreach (var info in objectToTrim.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
			{
				if (info.PropertyType == typeof(string))
				{
					if (info.CanWrite && info.PropertyType == typeof(string))
					{
						var value = (string)info.GetValue(objectToTrim, null);
						if (!string.IsNullOrEmpty(value))
							info.SetValue(objectToTrim, value.Trim(), null);
					}
				}
				else if (
					  info.PropertyType == typeof(Boolean) ||
					  info.PropertyType == typeof(Byte) ||
					  info.PropertyType == typeof(SByte) ||
					  info.PropertyType == typeof(Char) ||
					  info.PropertyType == typeof(DateTime) ||
					  info.PropertyType == typeof(Decimal) ||
					  info.PropertyType == typeof(Double) ||
					  info.PropertyType == typeof(Guid) ||
					  info.PropertyType == typeof(Single) ||
					  info.PropertyType == typeof(Int32) ||
					  info.PropertyType == typeof(UInt32) ||
					  info.PropertyType == typeof(Int64) ||
					  info.PropertyType == typeof(UInt64) ||
					  info.PropertyType == typeof(Int16) ||
					  info.PropertyType == typeof(UInt16) ||
					  (info.PropertyType.IsGenericType && info.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
				{
					// Do nothing
				}
				else if (info.PropertyType.IsGenericType && info.PropertyType.GetGenericTypeDefinition() == typeof(IList<>))
				{
					var objectProperty = (System.Collections.IList)info.GetValue(objectToTrim, null);
					if (objectProperty == null) return;
					foreach (var o in objectProperty)
					{
						o.TrimAllStrings();
					}

					info.SetValue(objectToTrim, objectProperty, null);
				}
				else // Must be an object               
				{
					var objectProperty = info.GetValue(objectToTrim, null);
					objectProperty.TrimAllStrings();
					info.SetValue(objectToTrim, objectProperty, null);
				}
			}
		}

		public static IDictionary<string, object> ToAttributesDictionary(this object attributes)
		{
			IDictionary<string, object> dictionary = new Dictionary<string, object>();

			try
			{
				foreach (var property in attributes.GetType().GetProperties())
				{
					dictionary.Add(property.Name.Replace("_", "-"), property.GetValue(attributes, null));
				}

				return dictionary;
			}
			catch (Exception)
			{
				return dictionary;
			}
		}

        private static T To<T>(this Object @object, Boolean returnDefaultOnException)
        {
            Type type = typeof(T);
            Type underlyingTypeOfNullable = Nullable.GetUnderlyingType(type);
            try
            {
                return (T)Convert.ChangeType(@object, underlyingTypeOfNullable ?? type);
            }
            catch (Exception exception)
            {
                if (returnDefaultOnException)
                    return default(T);
                String typeName = type.Name;
                if (underlyingTypeOfNullable != null)
                    typeName += " of " + underlyingTypeOfNullable.Name;
                throw new InvalidCastException("Object can't be cast to " + typeName, exception);

            }
        }
        public static T To<T>(this Object @object) { return @object.To<T>(returnDefaultOnException: false); }
        public static T ToOrDefault<T>(this Object @object) { return @object.To<T>(returnDefaultOnException: true); }

        public static string ToStringOrEmpty<T>(this T? param) where T : struct
		{
			return param.HasValue ? param.Value.ToString() : string.Empty;
		}

		/// <summary>
		/// Returns the value of the attribute property specified on the property's TAttribute.
		/// </summary>
		/// <typeparam name="TAttribute">The type of the attribute.</typeparam>
		/// <typeparam name="TAttributeProperty">The type of the attribute property.</typeparam>
		/// <param name="value">The public property with the TAttribute attribute.</param>
		/// <param name="attributePropertyName">The attribute's public property with the property name.</param>
		/// <returns>The value of the attribute property.</returns>
		public static TAttributeProperty GetAttributePropertyValue<TAttribute, TAttributeProperty>(this object value, string attributePropertyName)
			where TAttribute : class
		{
			var fi = value.GetType().GetField(value.ToString());

			if (fi == null)
			{
				return default(TAttributeProperty);
			}

			var attributes = fi.GetCustomAttributes(typeof(TAttribute), false);

			if (attributes.Length > 0)
			{
				var attribute = attributes[0];
				var attributeProperty = attribute.GetType().GetProperty(attributePropertyName);

				if (attributeProperty == null)
				{
					return default(TAttributeProperty);
				}

				var attributePropertyValue = attributeProperty.GetValue(attribute, null);

				if (attributePropertyValue is TAttributeProperty)
				{
					return (TAttributeProperty)attributePropertyValue;
				}

				return default(TAttributeProperty);
			}

			return default(TAttributeProperty);
		}

		public static TEnum GetEnum<TEnum>(this string value, string property = "Name")
		{
			return String.IsNullOrWhiteSpace(value)
				? default(TEnum)
				: Enum.GetValues(typeof(TEnum))
					  .Cast<TEnum>()
					  .FirstOrDefault(x => x.GetAttributePropertyValue<DisplayAttribute, string>(property).Equals(value));
		}
	}
}