using System;
using System.Collections;
using System.Collections.Generic;
using Spewnity;
using UnityEngine;

/**
 * Place this on any sprite. This generates a repeating or random background with parallaxing support.
 * WORK IN PROGRESS, and no I don't remember where I left off :)
 */
namespace Spewnity
{
	[RequireComponent(typeof(Sprite))]
	public class Background : MonoBehaviour
	{
		private SpriteRenderer sr;
		private float camWidth, camHeight;
		private int lastScreenWidth;
		private GameObjectPool pool;
		private int id = 0;
		private Vector2 tileSize = Vector2Int.zero;
		public Vector2 parallax = Vector2.one; // Optional parallax behavior
		public Vector2Int repetitions; // Number of tiles repeated horizontally and vertically

		public Camera cam;

		public void Awake()
		{
			Init();

			if (pool == null)
			{
				pool = new GameObjectPool();
				pool.maxSize = -1;
			}
			pool.createGameObject = BuildGameObject;
			pool.parent = transform;

			UpdateCamSize();
			RebuildSprites();
		}

		void Start() { }

		// Init for both game awake and editor validation
		public void Init()
		{
			if (cam == null)
				cam = Camera.main;

			if (sr == null)
				gameObject.Assign<SpriteRenderer>(ref sr);
			sr.enabled = false; // Disable main sprite
			tileSize = sr.bounds.size;
		}

		void OnValidate()
		{
			// Determine how many sprites you need for scrolling
			// If either value is set to 0, it will suggest a size at least twice as big as the current camera frustrum
			if (repetitions.x == 0 || repetitions.y == 0)
			{
				Init();
				UpdateCamSize();
				repetitions = new Vector2Int(Mathf.CeilToInt(camWidth * 2f / tileSize.x), Mathf.CeilToInt(camHeight * 2f / tileSize.y));
			}
		}

		void Update()
		{
			CheckCamChange();
			// Vector3 pos = new Vector3(cam.transform.position.x - cam.transform.position.x * parallax.x,
			// 	cam.transform.position.y - cam.transform.position.y * parallax.y, transform.position.z);
		}

		private void CheckCamChange()
		{
			if (lastScreenWidth == Screen.width)
				return;

			UpdateCamSize();
			RebuildSprites();
		}

		private void UpdateCamSize()
		{
			camHeight = cam.orthographicSize;
			camWidth = 2.0f * ((float) Screen.width / (float) Screen.height) * cam.orthographicSize;
			lastScreenWidth = Screen.width;
		}

		private GameObject BuildGameObject()
		{
			GameObject go = new GameObject(this.name + id++);
			go.AddComponent(sr); // With copy of SpriteRenderer
			go.transform.SetParent(pool.parent, false);
			go.transform.rotation = transform.rotation;
			return go;
		}

		private void RebuildSprites()
		{
			// Release all pool sprites
			pool.ReleaseAll();

			// Determine half size of repeated background
			Vector2 halfBg = new Vector2((repetitions.x - 1) * tileSize.x / 2f, (repetitions.y - 1) * tileSize.y / 2f);
			Debug.Log("TileSize:" + tileSize.x + "x" + tileSize.y + " Repetitions:" + repetitions.x + "x" + repetitions.y + " HalfBG:" + halfBg.x + "," + halfBg.y);

			// Create those sprites in the right locations
			for (int x = 0; x < repetitions.x; x++)
				for (int y = 0; y < repetitions.y; y++)
				{
					if (pool.Size == pool.maxSize)
						Debug.Log("Pool at max size");
					GameObject go = pool.TryGet();
					go.ThrowIfNull();
					go.transform.localPosition = new Vector3(x * tileSize.x - halfBg.x, y * tileSize.y - halfBg.y, 1);
				}
		}
	}

}