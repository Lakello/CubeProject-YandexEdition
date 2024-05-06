using UnityEditor;
using UnityEngine;

namespace MyNamespace
{
	public class RectTransformSnapAnchors
	{
		[MenuItem("Tools/UI/Anchor Around Object")]
		[MenuItem("CONTEXT/RectTransform/Anchor Current Position")]
		private static void FindAnchorAroundObject()
		{
			foreach (GameObject selectionObject in Selection.gameObjects)
			{
				if (selectionObject == null)
					return;

				if (!selectionObject.TryGetComponent(out RectTransform rectTransform)
					|| rectTransform.parent == null)
					continue;

				Undo.RecordObject(selectionObject, "SnapAnchors");
				ChangeAnchorRect(rectTransform);
			}
		}

		private static void ChangeAnchorRect(RectTransform originRect)
		{
			var parentRect = originRect.parent.GetComponent<RectTransform>().rect;

			var offsetMin = originRect.offsetMin;
			var offsetMax = originRect.offsetMax;
			var originAnchorMin = originRect.anchorMin;
			var originAnchorMax = originRect.anchorMax;

			var parentWidth = parentRect.width;
			var parentHeight = parentRect.height;

			var anchorMin = new Vector2(
				originAnchorMin.x + offsetMin.x / parentWidth,
				originAnchorMin.y + offsetMin.y / parentHeight);

			var anchorMax = new Vector2(
				originAnchorMax.x + offsetMax.x / parentWidth,
				originAnchorMax.y + offsetMax.y / parentHeight);

			originRect.anchorMin = anchorMin;
			originRect.anchorMax = anchorMax;

			originRect.offsetMin = Vector2.zero;
			originRect.offsetMax = Vector2.zero;
			originRect.pivot = new Vector2(0.5f, 0.5f);
		}

		[MenuItem("CONTEXT/RectTransform/Fill Parent")]
		private static void FillParent()
		{
			if (Selection.activeTransform.TryGetComponent(out RectTransform rectTransform) == false)
				return;

			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.sizeDelta = Vector2.zero;
		}
	}
}