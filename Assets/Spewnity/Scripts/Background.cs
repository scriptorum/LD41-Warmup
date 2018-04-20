using System;
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
		private float camWidth, camHeight;
		private int lastScreenWidth;

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
			UpdateDimensions();

			if (sr == null)
				gameObject.Assign<SpriteRenderer>(ref sr);
			UpdateSprite();
		}

		private void UpdateDimensions()
		{
			if (lastScreenWidth == Screen.width)
				return;

			camHeight = cam.orthographicSize;
			camWidth = 2.0f * ((float) Screen.width / (float) Screen.height) * cam.orthographicSize;
			lastScreenWidth = Screen.width;
		}

		public void Update()
		{
			UpdateDimensions();
			Vector3 pos = new Vector3(cam.transform.position.x - cam.transform.position.x * parallax.x,
				cam.transform.position.y - cam.transform.position.y * parallax.y, transform.position.z);
		}

		private void UpdateSprite()
		{
			// sr.drawMode = SpriteDrawMode.Tiled;
			// sr.size = new Vector2(Mathf.CeilToInt(camWidth), Mathf.CeilToInt(camHeight));

		}
	}
}