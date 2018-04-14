using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spewnity;

public class ShipControl : MonoBehaviour
{
	private bool sideways = true;

	private Rigidbody2D rb;
	private ParticleSystem sideThrustersFront, sideThrustersBack;
    private readonly float SLIDE_SPEED = 20.0f;
    private readonly float THRUST_BURST = 10f;
    private readonly int MIN_THRUST = 5;

    void Awake()
	{
		gameObject.SelfAssign<Rigidbody2D>(ref rb);
		sideThrustersBack = gameObject.GetChildComponent<ParticleSystem>("SideThrustersBack");
		sideThrustersFront = gameObject.GetChildComponent<ParticleSystem>("SideThrustersFront");
	}

	// Update is called once per frame
	void Update()
	{
		if(sideways)
		{
			float x = Input.GetAxis("Horizontal");
			if(x != 0) Slide(Time.deltaTime * x);
		}
	}

	private void Slide(float amount)
	{
		rb.transform.Translate(amount * SLIDE_SPEED, 0, 0, Space.Self);
		if(amount < 0) 
			sideThrustersBack.Emit(Mathf.CeilToInt(-amount * THRUST_BURST) + MIN_THRUST);
		else if(amount > 0) 
			sideThrustersFront.Emit(Mathf.CeilToInt(amount * THRUST_BURST) + MIN_THRUST);
	}
}