using UnityEngine;

public class BasicPlayerMovement : MonoBehaviour, IPlayerMovement
{
	[SerializeField]
	private float JumpSpeed = 5;

	public void Jump(PlayerController player)
	{
		if (player.Grounded)
		{
			Vector3 velocity = player.RigidBody.velocity;
			velocity.y = JumpSpeed;
			player.RigidBody.velocity = velocity;
		}
	}

	public void Move(PlayerController player)
	{
		Vector3 velocity = player.RigidBody.velocity;
		velocity.x = player.Movement.TargetGroundVel.x;
		velocity.z = player.Movement.TargetGroundVel.z;
		player.RigidBody.velocity = velocity;
	}
}
