using System.Collections;
using System.Collections.Generic;
using Spewnity;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
	private bool sideways = false;

	private Rigidbody2D rb;
	private ParticleSystem sideThrustersFront, sideThrustersBack;

	private readonly float THRUST_SPEED = 25f;
	private readonly float THRUST_BURST = 30f;
	private readonly int MIN_BURST = 5;
	private readonly float CAM_ZOOM = 1.5f;

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
			CameraDirector.instance.RotateTo(sideways ? -90f : 0f, 0.2f);
			if (sideways) StopFollow();
			else SetFollow();
		}

		if (sideways)
		{
			float x = Input.GetAxis("Horizontal");
			Thrust(x, true);
		}
		else
		{
			float y = Input.GetAxis("Vertical");
			Thrust(-y);
		}
	}

	private void SetFollow()
	{
		CameraDirector.instance.FollowTarget(transform, 0.2f);
	}

	private void StopFollow()
	{
		CameraDirector.instance.StopMoving();
		CameraDirector.instance.CutTo(transform.position);
	}

	private void Thrust(float amount, bool defaultZoom = false)
	{
		rb.transform.Translate(amount * Time.deltaTime * THRUST_SPEED, 0, 0, Space.Self);
		if (amount < 0)
			sideThrustersBack.Emit(Mathf.CeilToInt(-amount * Time.deltaTime * THRUST_BURST) + MIN_BURST);
		else if (amount > 0)
			sideThrustersFront.Emit(Mathf.CeilToInt(amount * Time.deltaTime * THRUST_BURST) + MIN_BURST);
		else return;

		CameraDirector.instance.SetZoom(defaultZoom ? 1f : 1f + Mathf.Abs(amount) * CAM_ZOOM);
	}
}