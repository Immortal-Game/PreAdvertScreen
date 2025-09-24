using UnityEngine;

namespace PreAdvertScreen
{
	public class ParticleClickItem : ClickItem
	{
		[SerializeField] private GameObject _particlePrefab;
		
		public override void Click()
		{
			var instance = Instantiate(_particlePrefab, transform.position, Quaternion.identity, transform.parent);
			Destroy(instance, 5f);
			base.Click();
		}
	}
}