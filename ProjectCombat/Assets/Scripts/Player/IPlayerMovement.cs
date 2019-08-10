public interface IPlayerMovement
{
	void Jump(PlayerController player);
	void Move(PlayerController player);
}

[System.Serializable]
public class PlayerMovementComponent : IUnifiedContainer<IPlayerMovement> { }