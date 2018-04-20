using System.Collections;
using UnityEngine;

/**
 * Place this on any sprite. This generates a repeating or random background with parallaxing support.
 */
namespace Spewnity
{
	[ExecuteInEditMode]
	public class Background : MonoBehaviour
	{
		public Vector2 parallax = Vector3.one;
		public Camera cam;

		public void Awake()
		{
			Init();
		}

		public void Init()
		{
			if (cam == null)
				cam = Camera.main;
		}

		public void LateUpdate()
		{
			Vector3 pos = new Vector3(cam.transform.position.x - cam.transform.position.x * parallax.x,
				cam.transform.position.y - cam.transform.position.y * parallax.y, transform.position.z);

			transform.position = pos;
		}
	}
}