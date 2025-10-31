using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace PreAdvertScreen
{
	public enum CanvasMode : byte
	{
		OVERLAY,
		CAMERA_SPACE
	}

	public class PreAdvertScreenView : MonoBehaviour
	{
		[SerializeField] private List<CanvasData> _canvases = new();
		[SerializeField] private Camera _uiCamera;

		private ScreenData _screenData;
		private TimeSpan _duration;
		private float _counter;
		internal CanvasData CurrentCanvas;

		public void Setup(Sprite backgroundSprite, bool activeTimer, bool activeCounter, ScreenData screenData, CanvasMode mode = CanvasMode.OVERLAY)
		{
			_screenData = screenData;
			var data = CurrentCanvas = _canvases.First(d => d.CanvasMode == mode);
			_uiCamera.gameObject.SetActive(mode == CanvasMode.CAMERA_SPACE);
			for (int i = 0; i < _canvases.Count; i++)
			{
				_canvases[i].gameObject.SetActive(CurrentCanvas.CanvasMode == _canvases[i].CanvasMode);
			}

			data.Background.sprite = backgroundSprite;
			data.TimerTMP.gameObject.SetActive(activeTimer);
			data.CounterTMP.gameObject.SetActive(activeCounter);
			Hide();
		}
		
		public void Setup()
		{
			_screenData = new ScreenData("Счетчик: {tag}", "Реклама через\n{tag}", "{tag}");
			var data = CurrentCanvas = _canvases.First(d => d.CanvasMode == CanvasMode.CAMERA_SPACE);
			_uiCamera.gameObject.SetActive(true);
			for (int i = 0; i < _canvases.Count; i++)
			{
				_canvases[i].gameObject.SetActive(CurrentCanvas.CanvasMode == _canvases[i].CanvasMode);
			}

			data.Background.sprite = Resources.Load<Sprite>("Sprites/adclicker_background");
			data.TimerTMP.gameObject.SetActive(true);
			data.CounterTMP.gameObject.SetActive(true);
			Hide();
		}

		internal void Show()
		{
			CurrentCanvas.ParentContainer.SetActive(true);
			_counter = -1;
			IncrementCounter();
		}

		internal void Hide()
		{
			CurrentCanvas.ParentContainer.SetActive(false);
		}

		public void IncrementCounter()
		{
			CurrentCanvas.CounterTMP.text = _screenData.CounterText.Replace(_screenData.Tag, (++_counter).ToString(CultureInfo.InvariantCulture));
		}

		public void SetTimeLeft(TimeSpan time)
		{
			CurrentCanvas.TimerTMP.text = _screenData.TimerText.Replace(_screenData.Tag, $"{time.Seconds}..");
		}
	}
}