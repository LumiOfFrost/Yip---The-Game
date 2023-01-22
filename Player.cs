using Godot;
using System;

public partial class Player : CharacterBody3D
{	

	[Export]
	string nodePaths;

	[Export]
	public NodePath cameraNodePath;

	[Export]
	float Speed = 5.0f;

	[Export]
	float JumpVelocity = 4.5f;

	Node3D camera;

	[Export]
	float movementControl = 0.1f;

	Vector2 movementVector = Vector2.Zero;

	public Vector3 lookDirection = Vector3.Forward;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	[Export]
	float fallingGravityMultiplier = 1.75f;

	public override void _Ready() {

		camera = GetNode<Node3D>(cameraNodePath);

	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			if (Velocity.y > 0) {
				velocity.y -= gravity * (float)delta;
			} else {
				velocity.y -= gravity * (float)delta * fallingGravityMultiplier;
			}

		
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputVector = Input.GetVector("movement_left", "movement_right", "movement_forward", "movement_back");
		movementVector = movementVector.Lerp(inputVector, movementControl);
		Vector3 direction = (camera.GlobalTransform.basis * new Vector3(movementVector.x, 0, movementVector.y));
		if (direction != Vector3.Zero && inputVector != Vector2.Zero && lookDirection != direction) {

			lookDirection = lookDirection.Lerp(direction + Transform.basis.x * 0.05f, 0.2f);

		}
		if (direction != Vector3.Zero)
		{
			velocity.x = direction.x * Speed;
			velocity.z = direction.z * Speed;
		}
		else
		{
			velocity.x = Mathf.MoveToward(Velocity.x, 0, Speed);
			velocity.z = Mathf.MoveToward(Velocity.z, 0, Speed);
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor()) {
			velocity.y = JumpVelocity;
		}


		if (lookDirection != Vector3.Zero && lookDirection != Position.DirectionTo(Position + lookDirection * new Vector3(1,0,1))) LookAt(Position + lookDirection * new Vector3(1,0,1));

		Velocity = velocity;
		MoveAndSlide();
	}
}
