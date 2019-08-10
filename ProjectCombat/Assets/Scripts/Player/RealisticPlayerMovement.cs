using UnityEngine;

public class RealisticPlayerMovement : MonoBehaviour, IPlayerMovement
{
	[System.Serializable]
	public class GroundForce
	{
		public float MovingForce;
		public float StoppingForce;
	}

	[SerializeField]
	private GroundForce Walking = null, Running = null, SneakWalking = null, SneakRunning = null;
	[SerializeField]
	private float JumpImpulseForce = 5;

	private GroundForce GetForces(PlayerController player)
	{
		GroundForce[] forces = { Walking, Running, SneakWalking, SneakRunning };
		return forces[(player.Sneaking ? 1 : 0) * 2 + (player.Running ? 1 : 0)];
	}

	public void Jump(PlayerController player)
	{
		if (player.Grounded)
		{
			player.RigidBody.AddForce(player.transform.up * JumpImpulseForce, ForceMode.Impulse);
		}
	}

	public void Move(PlayerController player)
	{
		// Only allow player to change velocity while grounded
		if (player.Grounded)
		{
		}
	}
}