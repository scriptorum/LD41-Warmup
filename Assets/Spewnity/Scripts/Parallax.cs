using System.Collections;
using UnityEngine;

/**
 * Parallax behavior.
 * Attach this to the object that you wish to move in a parallax manner. 
 * This object should probably be a child of the camera.
 * Measure the extents of the camera movement and the extents this object local to the camera.
 * If only wish to parallax on one axis, leave the extents for the other axis at 0.
 **/
namespace Spewnity
{
	[ExecuteInEditMode]
	public class Parallax : MonoBehaviour
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

		public void Update()
		{
			Vector3 pos = new Vector3(cam.transform.position.x - cam.transform.position.x * parallax.x,
				cam.transform.position.y - cam.transform.position.y * parallax.y, transform.position.z);

			transform.position = pos;
		}
	}
}