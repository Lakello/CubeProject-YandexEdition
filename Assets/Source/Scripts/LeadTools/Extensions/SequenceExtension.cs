using DG.Tweening;

namespace LeadTools.Extensions
{
	public static class SequenceExtension
	{
		public static void Stop(this Sequence origin)
		{
			if (origin != null)
			{
				origin.onComplete = null;
				origin.Kill();
			}
		}
	}
}