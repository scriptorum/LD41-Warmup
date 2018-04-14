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
	

	void Awake()
	{
		gameObject.SelfAssign<Rigidbody2D>(ref rb);
		sideThrustersBack = gameObject.GetChildComponent<ParticleSystem>("SideThrustersBack");
		sideThrustersFront = gameObject.GetChildComponent<ParticleSystem>("SideThrustersFront");
		CameraDirector.instance.FollowTarget(transform, 0.2f);
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			sideways = !sideways;
			// CameraDirector.instance.RotateTo(sideways ? -90f : 0f);
			CameraDirector.instance.SetRotation(sideways ? -90f : 0f);
		}

		if (sideways)
		{
			float x = Input.GetAxis("Horizontal");
			if (x != 0) Slide(Time.deltaTime * x);
		}
		else
		{ 
			float y = Input.GetAxis("Vertical");
			if (y != 0) Slide(Time.deltaTime * -y);
		}
	}

	private void Slide(float amount)
	{
		rb.transform.Translate(amount * SLIDE_SPEED, 0, 0, Space.Self);
		if (amount < 0)
			sideThrustersBack.Emit(Mathf.CeilToInt(-amount * THRUST_BURST) + MIN_THRUST);
		else if (amount > 0)
			sideThrustersFront.Emit(Mathf.CeilToInt(amount * THRUST_BURST) + MIN_THRUST);
	}
}