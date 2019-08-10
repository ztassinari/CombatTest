using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
	[System.Serializable]
	public class MovementInfo
	{
		[Header("Ground Movement")]
		[SerializeField]
		private float _WalkingSpeed = 2;
		public float WalkingSpeed => _WalkingSpeed;
		[SerializeField]
		private float _RunningSpeed = 4;
		public float RunningSpeed => _RunningSpeed;
		[SerializeField]
		private float _SneakWalkingSpeed = 1;
		public float SneakWalkingSpeed => _SneakWalkingSpeed;
		[SerializeField]
		private float _SneakRunningSpeed = 3;
		public float SneakRunningSpeed => _SneakRunningSpeed;
		public Vector3 TargetGroundVel { get; internal set; } = Vector3.zero;

		[Header("Components")]
		[SerializeField]
		private PlayerMovementComponent MovementImplementation = null;
		public IPlayerMovement Actions => MovementImplementation.Result;
	}

	[SerializeField]
	private MovementInfo _Movement = new MovementInfo();
	public MovementInfo Movement => _Movement;
	[Space(15)]
	[SerializeField]
	private Transform Head = null;
	[SerializeField]
	private Transform Feet = null;
	[Space(15)]
	[SerializeField]
	private float MinPitch = -80;
	[SerializeField]
	private float MaxPitch = 80;
	[SerializeField]
	[Space(15)]
	private float MaxGroundedDistance = 0.1f;

	public Rigidbody RigidBody { get; private set; } = null;

	private Vector2 LookAxis => new Vector2(Input.GetAxis("Look Horizontal"), Input.GetAxis("Look Vertical"));
	private Vector2 YawAndPitch = Vector2.zero;
	private Quaternion OriginalHeadRotation = Quaternion.identity;
	private Quaternion OriginalBodyRotation = Quaternion.identity;

	public bool Grounded => Physics.Raycast(Feet.position, -transform.up, MaxGroundedDistance);
	public bool Sneaking { get; private set; } = false;
	public bool Running { get; private set; } = false;

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		OriginalHeadRotation = Head.localRotation;
		OriginalBodyRotation = transform.localRotation;

		RigidBody = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		HandleInput();
		RigidBody.useGravity = !Grounded;
	}

	private void HandleInput()
	{
		if (Input.GetKeyDown(KeyCode.LeftBracket))
		{
			GameState.Settings.ToggleRun = !GameState.Settings.ToggleRun;
			GameState.SaveSettings();
		}
		if (Input.GetKeyDown(KeyCode.RightBracket))
		{
			GameState.Settings.ToggleSneak = !GameState.Settings.ToggleSneak;
			GameState.SaveSettings();
		}

		if (!GameState.Settings.ToggleRun)
		{
			Running = Input.GetButton("Run");
		}
		else if (Input.GetButtonDown("Run"))
		{
			Running = !Running;
		}

		if (!GameState.Settings.ToggleSneak)
		{
			Sneaking = Input.GetButton("Sneak");
		}
		else if (Input.GetButtonDown("Sneak"))
		{
			Sneaking = !Sneaking;
		}

		float moveSpeed = 0;
		if (Sneaking)
		{
			if (Running) moveSpeed = Movement.SneakRunningSpeed;
			else moveSpeed = Movement.SneakWalkingSpeed;
		}
		else
		{
			if (Running) moveSpeed = Movement.RunningSpeed;
			else moveSpeed = Movement.WalkingSpeed;
		}

		Vector3 movementDir = Vector3.zero;
		if (Input.GetButton("Move Forward")) movementDir += transform.forward;
		if (Input.GetButton("Move Backward")) movementDir -= transform.forward;
		if (Input.GetButton("Move Left")) movementDir -= transform.right;
		if (Input.GetButton("Move Right")) movementDir += transform.right;
		movementDir.Normalize();
		Movement.TargetGroundVel = movementDir * moveSpeed;

		Movement.Actions.Move(this);

		if(Input.GetButtonDown("Jump"))
		{
			Movement.Actions.Jump(this);
		}

		HandleLooking();
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f) angle += 360f;
		if (angle > 360f) angle -= 360f;
		return Mathf.Clamp(angle, min, max);
	}

	private void HandleLooking()
	{
		YawAndPitch += LookAxis;
		YawAndPitch.x = ClampAngle(YawAndPitch.x, -360f, 360f);
		YawAndPitch.y = ClampAngle(YawAndPitch.y, MinPitch, MaxPitch);

		Quaternion Yaw = Quaternion.AngleAxis(YawAndPitch.x, Vector3.up);
		Quaternion Pitch = Quaternion.AngleAxis(YawAndPitch.y, -Vector3.right);

		Head.localRotation = OriginalHeadRotation * Pitch;
		transform.localRotation = OriginalBodyRotation * Yaw;
	}

	private void Move(Vector3 dir)
	{
		transform.position += dir;
	}
}