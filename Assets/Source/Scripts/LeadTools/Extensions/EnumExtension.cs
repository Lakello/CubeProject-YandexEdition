using System;
using LeadTools.Common;

namespace LeadTools.Extensions
{
	public static class EnumExtension
	{
		public static string GetCurrentName(this ShaderProperty origin) =>
			Enum.GetName(typeof(ShaderProperty), origin);
	}
}