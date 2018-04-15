using System.Collections;
using System.Collections.Generic;
using Spewnity;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
	private bool sideways = false;

	private Rigidbody2D rb;
	private ParticleSystem foreThruster, aftThruster, portThruster, starboardThruster;
	private readonly float THRUST_SPEED = 25f;
	private readonly float THRUST_BURST = 30f;
	private readonly int MIN_THRUST_BURST = 5;
	private readonly float CAM_ZOOM = 1.3f;
	private readonly float SIDE_THRUST_SPEED = 8f;
	private readonly float SIDE_THRUST_BURST = 3f;
	private readonly int MIN_SIDE_THRUST_BURST = 1;
    private readonly float ROTATE_SPEED = 180f;

    void Awake()
	{
		gameObject.SelfAssign<Rigidbody2D>(ref rb);
		aftThruster = gameObject.GetChildComponent<ParticleSystem>("AftThruster");
		foreThruster = gameObject.GetChildComponent<ParticleSystem>("ForeThruster");
		portThruster = gameObject.GetChildComponent<ParticleSystem>("PortThruster");
		starboardThruster = gameObject.GetChildComponent<ParticleSystem>("StarboardThruster");
		SetFollow();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			sideways = !sideways;
			// CameraDirector.instance.RotateTo(sideways ? transform.localRotation.eulerAngles.z + 90f : 0f, 0.2f);
			CameraDirector.instance.RotateTo(transform.localRotation.eulerAngles.z + (sideways ? 0f : 90f), 0.2f);
			if (sideways) StopFollow();
			else SetFollow();
		}

		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");

		if (sideways)
		{
			CameraDirector.instance.SetZoom(1f);
			Thrust(x);
		}
		else
		{
			Thrust(-y);
			SideThrust( y < 0 ? x : -x);
			CameraDirector.instance.SetZoom(1f + Mathf.Abs(-y) * CAM_ZOOM);
			CameraDirector.instance.SetRotation(transform.localRotation.eulerAngles.z + 90f);
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

	private void Thrust(float amount)
	{
		rb.transform.Translate(amount * Time.deltaTime * THRUST_SPEED, 0, 0, Space.Self);
		int particles = Mathf.CeilToInt(Mathf.Abs(amount * Time.deltaTime * THRUST_BURST) + MIN_THRUST_BURST);
		if (amount < 0) aftThruster.Emit(particles);
		else if (amount > 0) foreThruster.Emit(particles);
	}

	private void SideThrust(float amount)
	{
		transform.Rotate(0f, 0f, amount * Time.deltaTime * ROTATE_SPEED);
		int particles = Mathf.CeilToInt(Mathf.Abs(amount * Time.deltaTime * SIDE_THRUST_BURST) + MIN_SIDE_THRUST_BURST);
		if (amount < 0) starboardThruster.Emit(particles);
		else if (amount > 0) portThruster.Emit(particles);		
	}
}