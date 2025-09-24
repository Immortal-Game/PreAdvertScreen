using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PreAdvertScreen
{
	public class ClickItem : MonoBehaviour, IPointerDownHandler
	{
		[SerializeField] protected Image Icon;
		[SerializeField] internal RectTransform RectTransform;

		public UnityEvent OnClick = new UnityEvent();

		public virtual void SetSize(float width, float height) => Icon.rectTransform.sizeDelta = new Vector2(width, height);
		public virtual void SetSize(float size) => SetSize(size, size);
		public virtual void SetSize(int percentage) => SetSize(Screen.width / 100f * percentage, Screen.width / 100f * percentage);
		public virtual void SetPicture(Sprite sprite) => Icon.sprite = sprite;

		public virtual void Click()
		{
			OnClick.Invoke();
			Destroy(gameObject);
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			Click();
		}
	}
}