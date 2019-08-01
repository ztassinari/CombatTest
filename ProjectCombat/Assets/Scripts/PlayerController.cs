using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
	public float WalkingSpeed = 1;
	public float RunningSpeed = 2;
	public Transform Head = null;
	public Transform Feet = null;
	public float MinPitch = -60, MaxPitch = 60;
	public float MaxGroundedDistance = 1;
	public float JumpSpeed = 1;

	private Rigidbody rb = null;

	Vector2 LookAxis => new Vector2(Input.GetAxis("Look Horizontal"), Input.GetAxis("Look Vertical"));
	Vector2 YawAndPitch = Vector2.zero;
	Quaternion OriginalHeadRotation = Quaternion.identity;
	Quaternion OriginalBodyRotation = Quaternion.identity;

	private bool Grounded => Physics.Raycast(Feet.position, -transform.up, MaxGroundedDistance);

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		OriginalHeadRotation = Head.localRotation;
		OriginalBodyRotation = transform.localRotation;

		rb = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		HandleInput();
	}

	private void HandleInput()
	{
		float speed = (Input.GetButton("Run") ? RunningSpeed : WalkingSpeed);

		Vector3 movementDir = Vector3.zero;
		if (Input.GetButton("Move Forward")) movementDir += transform.forward;
		if (Input.GetButton("Move Backward")) movementDir -= transform.forward;
		if (Input.GetButton("Move Left")) movementDir -= transform.right;
		if (Input.GetButton("Move Right")) movementDir += transform.right;
		movementDir.Normalize();

		Move(movementDir * speed * Time.deltaTime);
		if(Input.GetButtonDown("Jump") && Grounded)
		{
			Debug.Log("Jump");
			rb.velocity = transform.up * JumpSpeed;
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