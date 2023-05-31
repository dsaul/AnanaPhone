using System.Reflection;

namespace DanSaul.SharedCode.Asterisk.AsteriskAEL
{
	[AttributeUsage(AttributeTargets.Class)]
	public class MarkContextIncludedAttribute : Attribute
	{
		public static IEnumerable<Type> GetTypesWithAttribute(Assembly assembly)
		{
			foreach (Type type in assembly.GetTypes())
			{
				if (type.GetCustomAttributes(typeof(MarkContextIncludedAttribute), true).Length > 0)
				{
					yield return type;
				}
			}
		}
	}
}
