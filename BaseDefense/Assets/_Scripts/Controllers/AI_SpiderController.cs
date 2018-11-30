using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_SpiderController : MonoBehaviour
{
	#region ShowInEditor
	[SerializeField] AI_Sight sight;
	[SerializeField] float moveForce;
	[SerializeField] float maxSpeed;
	[SerializeField] string targetName;
	[SerializeField] string turretName;
	[SerializeField] float turnOnRange;
	#endregion
	#region HideInEditor
	Rigidbody2D self;
	float sign = 1;
	Transform target;
	AI_TurretController turretController;
	#endregion

	private void Start()
	{
		self = GetComponent<Rigidbody2D>();
		target = GameObject.Find(targetName).transform;
		turretController = transform.Find(turretName).GetComponent<AI_TurretController>();
		turretController.TurnedOn = false;
		
	}
	private void Update()
	{
		CheckDirection();
		//if (Mathf.Abs(self.velocity.x) < maxSpeed)
		//{
			
		//}
		//else
		//{

		//}

		if (Mathf.Abs(target.position.x - transform.position.x) < turnOnRange)
		{
			turretController.TurnedOn = true;
			self.velocity = Vector2.zero;
		}
		else
		{
			turretController.TurnedOn = false;
			self.velocity = Vector2.left * maxSpeed * sign;
		}
		
	}
	private void CheckDirection()
	{
		sign = Mathf.Sign(transform.position.x - target.position.x);
	}
}