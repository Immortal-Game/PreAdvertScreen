using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PreAdvertScreen
{
	internal class CanvasData : MonoBehaviour
	{
		[field: SerializeField] public Canvas Canvas { get; private set; }
		[field: SerializeField] public CanvasMode CanvasMode { get; private set; }
		[field: SerializeField] public GameObject ParentContainer { get; private set; }
		[field: SerializeField] public Transform Container { get; private set; }
		[field: SerializeField] public Image Background { get; private set; }
		[field: SerializeField] public TMP_Text TimerTMP { get; private set; }
		[field: SerializeField] public TMP_Text CounterTMP { get; private set; }
		public RectTransform CanvasRect { get; private set; }

		private void Awake()
		{
			CanvasRect = Canvas.GetComponent<RectTransform>();
		}
	}
}