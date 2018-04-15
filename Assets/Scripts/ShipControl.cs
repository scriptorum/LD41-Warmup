using System.Collections;
using System.Collections.Generic;
using Spewnity;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
	private bool sideways = false;

	private Rigidbody2D rb;
	private ParticleSystem sideThrustersFront, sideThrustersBack;

	private readonly float SLIDE_SPEED = 20.0f;
	private readonly float THRUST_BURST = 25f;
	private readonly int MIN_THRUST = 5;
	private readonly float CAM_ZOOM = 25f;

	void Awake()
	{
		gameObject.SelfAssign<Rigidbody2D>(ref rb);
		sideThrustersBack = gameObject.GetChildComponent<ParticleSystem>("SideThrustersBack");
		sideThrustersFront = gameObject.GetChildComponent<ParticleSystem>("SideThrustersFront");
		SetFollow();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			sideways = !sideways;
			CameraDirector.instance.RotateTo(sideways ? -90f : 0f, 0.1f);
			if (sideways) StopFollow();
			else SetFollow();
		}

		if (sideways)
		{
			float x = Input.GetAxis("Horizontal");
			Slide(Time.deltaTime * x, true);
		}
		else
		{
			float y = Input.GetAxis("Vertical");
			Slide(Time.deltaTime * -y);
		}
	}

	private void SetFollow()
	{
		CameraDirector.instance.FollowTarget(transform, 0f);
	}

	private void StopFollow()
	{
		CameraDirector.instance.StopMoving();
		CameraDirector.instance.CutTo(transform.position);
	}

	private void Slide(float amount, bool defaultZoom = false)
	{
		rb.transform.Translate(amount * SLIDE_SPEED, 0, 0, Space.Self);
		if (amount < 0)
			sideThrustersBack.Emit(Mathf.CeilToInt(-amount * THRUST_BURST) + MIN_THRUST);
		else if (amount > 0)
			sideThrustersFront.Emit(Mathf.CeilToInt(amount * THRUST_BURST) + MIN_THRUST);
		else return;

		CameraDirector.instance.SetZoom(defaultZoom ? 1f : 1f + Mathf.Abs(amount) * CAM_ZOOM);
	}
}