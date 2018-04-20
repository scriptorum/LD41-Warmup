using System.Collections;
using Spewnity;
using UnityEngine;

/**
 * Place this on any sprite. This generates a repeating or random background with parallaxing support.
 */
namespace Spewnity
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Sprite))]
	public class Background : MonoBehaviour
	{
		private SpriteRenderer sr;

		public Vector2 parallax = Vector2.one; // Optional parallax behavior

		public Camera cam;

		public void Awake()
		{
			Init();
		}

		public void OnValidate()
		{
			Init();
		}

		public void Init()
		{
			if (cam == null)
				cam = Camera.main;

			if (sr == null)
				gameObject.Assign<SpriteRenderer>(ref sr);

			if (sr.drawMode == SpriteDrawMode.Simple)
			{
				sr.drawMode = SpriteDrawMode.Tiled;
				// sr.size = Vector4.zero;
			}
		}

		public void Update()
		{
			if (parallax != Vector2.one)
			{
				Vector3 pos = new Vector3(cam.transform.position.x - cam.transform.position.x * parallax.x,
					cam.transform.position.y - cam.transform.position.y * parallax.y, transform.position.z);

				transform.position = pos;
			}
		}
	}
}