using System;
using UnityEngine;

public class AdvancedPlayerMovement : MonoBehaviour, IPlayerMovement
{
	[SerializeField]
	private float Acceleration, Deceleration;
	[SerializeField]
	private float JumpImpulseForce = 5;

	private float CurrentMovementSpeed;

	public void Jump(PlayerController player)
	{
		if (player.Grounded)
		{
			player.RigidBody.AddForce(player.transform.up * JumpImpulseForce, ForceMode.Impulse);
		}
	}

	public void Move(PlayerController player)
	{
		float targetSpeed = player.Movement.TargetGroundVel.magnitude;
		if (CurrentMovementSpeed < targetSpeed)
		{
			CurrentMovementSpeed = Mathf.Min(CurrentMovementSpeed + Acceleration * Time.deltaTime, targetSpeed);
		}
		else if (CurrentMovementSpeed > targetSpeed)
		{
			CurrentMovementSpeed = Mathf.Max(CurrentMovementSpeed - Deceleration * Time.deltaTime, targetSpeed);
		}

		Vector3 groundVel = player.Movement.TargetGroundVel.normalized;
		if (player.Movement.TargetGroundVel.magnitude == 0)
		{
			groundVel.x = player.RigidBody.velocity.x;
			groundVel.z = player.RigidBody.velocity.z;
			groundVel = groundVel.normalized;
		}
		groundVel *= CurrentMovementSpeed;
		Vector3 velocity = player.RigidBody.velocity;
		velocity.x = groundVel.x;
		velocity.z = groundVel.z;
		player.RigidBody.velocity = velocity;
	}
}