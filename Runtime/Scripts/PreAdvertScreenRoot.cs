using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace PreAdvertScreen
{
	public static class PreAdvertScreenRoot
	{
		private static PreAdvertScreenView _view;
		private static ClickItem[] _clickItemPrefabs;

		public static float Speed = 300;
		public static TimeSpan Duration = TimeSpan.FromSeconds(10);
		public static TimeSpan SpawnCooldown = TimeSpan.FromSeconds(0.25f);

		private static readonly List<ClickItem> Spawned = new List<ClickItem>();

		public static void Initialize(params ClickItem[] clickItemPrefabs)
		{
			if (clickItemPrefabs == null || clickItemPrefabs.Length == 0)
				throw new ArgumentNullException(nameof(clickItemPrefabs));

			var prefab = Resources.Load<PreAdvertScreenView>("Prefabs/PreAdvertScreen Root");
			_view = Object.Instantiate(prefab);
			_view.name = "PreAdvertScreenView";
			Object.DontDestroyOnLoad(_view);

			_clickItemPrefabs = clickItemPrefabs;
		}

		public static void Setup(Sprite backgroundSprite, bool activeTimer, bool activeCounter, ScreenData screenData, CanvasMode mode = CanvasMode.OVERLAY)
		{
			if (_view == null) throw new Exception("PreAdvertScreenRoot not initialized. Use PreAdvertScreenRoot.Initialize()");
			_view.Setup(backgroundSprite, activeTimer, activeCounter, screenData, mode);
		}

		public static void Setup()
		{
			if (_view == null) throw new Exception("PreAdvertScreenRoot not initialized. Use PreAdvertScreenRoot.Initialize()");
			_view.Setup();
		}

		public static void Run(Action onOpen = null, Action<ClickItem> onSpawnClickItem = null, Action onClose = null)
		{
			if (!Application.isPlaying) return;
			if (_view == null) throw new Exception("PreAdvertScreenRoot not initialized. Use PreAdvertScreenRoot.Initialize()");
			onOpen?.Invoke();
			_view.Show();
			_view.StartCoroutine(UpdateTimerRoutine(Duration, () =>
			{
				_view.Hide();
				onClose?.Invoke();
			}));
			_view.StartCoroutine(SpawnClickItemsRoutine(onSpawnClickItem));
			_view.StartCoroutine(MoveClickObjectRoutine(Duration));
		}

		private static IEnumerator SpawnClickItemsRoutine(Action<ClickItem> onSpawnClickItem = null)
		{
			var time = 0f;
			while (time <= Duration.TotalSeconds)
			{
				int randomIndex = Random.Range(0, _clickItemPrefabs.Length);
				var newItem = Object.Instantiate(_clickItemPrefabs[randomIndex], _view.CurrentCanvas.Container);
				newItem.SetCanvasData(GetCanvasSize(_view.CurrentCanvas.Canvas, _view.CurrentCanvas.CanvasRect));
				onSpawnClickItem?.Invoke(newItem);
				newItem.name = $"ClickItem №{Spawned.Count}";
				newItem.OnClick.AddListener(_view.IncrementCounter);

				var canvasSize = GetCanvasSize(_view.CurrentCanvas.Canvas, _view.CurrentCanvas.CanvasRect);

				var randomX = Random.Range(
					newItem.RectTransform.sizeDelta.x / 2,
					canvasSize.x - newItem.RectTransform.sizeDelta.x / 2
				);

				newItem.RectTransform.localPosition = new Vector3(
					randomX - canvasSize.x / 2,
					-newItem.RectTransform.sizeDelta.y / 2 - canvasSize.y / 2,
					0
				);

				Spawned.Add(newItem);
				yield return new WaitForSecondsRealtime((float)SpawnCooldown.TotalSeconds);
				time += (float)SpawnCooldown.TotalSeconds;
			}

			foreach (var item in Spawned)
			{
				if (item != null) Object.Destroy(item.gameObject);
			}
			Spawned.Clear();
		}

		private static IEnumerator MoveClickObjectRoutine(TimeSpan duration)
		{
			var startTime = Time.realtimeSinceStartup;
			var targetEndTime = startTime + duration.TotalSeconds;
			while (Time.realtimeSinceStartup < targetEndTime)
			{
				for (int i = 0; i < Spawned.Count; i++)
				{
					if (Spawned[i] == null) continue;
					Spawned[i].RectTransform.localPosition += Vector3.up * (Speed * Time.unscaledDeltaTime);
				}
				yield return null;
			}
		}

		private static IEnumerator UpdateTimerRoutine(TimeSpan duration, Action onComplete = null)
		{
			var startTime = Time.realtimeSinceStartup;
			var targetTime = startTime + duration.TotalSeconds;

			while (Time.realtimeSinceStartup < targetTime)
			{
				var elapsed = Time.realtimeSinceStartup - startTime;
				var remaining = duration - TimeSpan.FromSeconds(elapsed);

				if (remaining <= TimeSpan.Zero) break;
				_view.SetTimeLeft(remaining);

				yield return null;
			}

			onComplete?.Invoke();
			_view.SetTimeLeft(TimeSpan.Zero);
		}

		private static Vector2 GetCanvasSize(Canvas canvas, RectTransform canvasRect)
		{
			return canvas.renderMode switch {
				RenderMode.ScreenSpaceOverlay or RenderMode.ScreenSpaceCamera => canvasRect.sizeDelta,
				RenderMode.WorldSpace => new Vector2(canvasRect.rect.width, canvasRect.rect.height),
				_ => canvasRect.sizeDelta
			};
		}
	}
}