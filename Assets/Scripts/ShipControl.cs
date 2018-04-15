using System.Collections;
using System.Collections.Generic;
using Spewnity;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
	private bool sideways = false;

	private Rigidbody2D rb;
	private ParticleSystem foreThruster, aftThruster, aftPortThruster, aftStarboardThruster, forePortThruster, foreStarboardThruster;
	private readonly float THRUST_SPEED = 25f;
	private readonly float THRUST_BURST = 30f;
	private readonly int MIN_THRUST_BURST = 5;
	private readonly float START_CAM_ZOOM = 1f;
	private readonly float FAST_CAM_ZOOM = 1.2f;
	private readonly float SIDE_THRUST_BURST = 3f;
	private readonly int MIN_SIDE_THRUST_BURST = 1;
    private readonly float ROTATE_SPEED = 180f;

    void Awake()
	{
		gameObject.SelfAssign<Rigidbody2D>(ref rb);
		aftThruster = gameObject.GetChildComponent<ParticleSystem>("AftThruster");
		foreThruster = gameObject.GetChildComponent<ParticleSystem>("ForeThruster");
		aftPortThruster = gameObject.GetChildComponent<ParticleSystem>("AftPortThruster");
		aftStarboardThruster = gameObject.GetChildComponent<ParticleSystem>("AftStarboardThruster");
		forePortThruster = gameObject.GetChildComponent<ParticleSystem>("ForePortThruster");
		foreStarboardThruster = gameObject.GetChildComponent<ParticleSystem>("ForeStarboardThruster");
	}

	void Start()
	{
		SetFollow();
		CameraDirector.instance.SetZoom(START_CAM_ZOOM);
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			sideways = !sideways;
			CameraDirector.instance.SetRotation(transform.localRotation.eulerAngles.z + (sideways ? 90f : 0f));
			if (sideways) CameraDirector.instance.CutTo(new Vector3(transform.position.x - Camera.main.orthographicSize * 3/4, transform.position.y, transform.position.z));
			else SetFollow();
		}

		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");

		if (sideways)
		{
			Thrust(x);
			CameraDirector.instance.SetZoom(START_CAM_ZOOM);
		}
		else
		{
			Thrust(y);
			SideThrust( y < 0 ? x : -x);
			CameraDirector.instance.SetZoom(1f + Mathf.Abs(-y) * FAST_CAM_ZOOM);
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
		rb.transform.Translate(0f, amount * Time.deltaTime * THRUST_SPEED, 0f, Space.Self);
		int particles = Mathf.CeilToInt(Mathf.Abs(amount * Time.deltaTime * THRUST_BURST) + MIN_THRUST_BURST);
		if (amount > 0f) aftThruster.Emit(particles);
		else if (amount < 0f)  foreThruster.Emit(particles);
	}

	private void SideThrust(float amount)
	{
		transform.Rotate(0f, 0f, amount * Time.deltaTime * ROTATE_SPEED);
		int particles = Mathf.CeilToInt(Mathf.Abs(amount * Time.deltaTime * SIDE_THRUST_BURST) + MIN_SIDE_THRUST_BURST);
		if (amount < 0f)
		{
			 aftStarboardThruster.Emit(particles);
			 forePortThruster.Emit(particles);
		}
		else if (amount > 0f)
		{
			aftPortThruster.Emit(particles);		
			foreStarboardThruster.Emit(particles);		
		}
	}
}