using System;
using System.Linq;

namespace JosephGuadagno.Utilities.Extensions
{
	public static class AssemblyExtensions
	{
		public static T GetAssemblyAttribute<T>(this System.Reflection.Assembly assembly) where T : Attribute
		{
			object[] attributes = assembly.GetCustomAttributes(typeof(T), false);
			if (attributes == null || attributes.Length == 0)
				return null;
			return attributes.OfType<T>().SingleOrDefault();
		}
	}
}
