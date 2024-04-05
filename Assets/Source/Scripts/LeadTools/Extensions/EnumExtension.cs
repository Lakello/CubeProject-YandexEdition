using System;
using LeadTools.Other;

namespace LeadTools.Extensions
{
	public static class EnumExtension
	{
		public static string GetCurrentName(this ShaderProperty origin) =>
			Enum.GetName(typeof(ShaderProperty), origin);
	}
}