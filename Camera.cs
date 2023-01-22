using Godot;
using System;

public partial class Camera : Node3D
{

	[Export]
	float sensitivity = 5;
	
	[Export]
	NodePath playerNodepath;
	[Export]
	NodePath rayNodePath;
	[Export]
	NodePath cameraNodePath;

	CharacterBody3D player;
	RayCast3D cameraPositionCast;
	Node3D camera;

	float horizontalRotationValue = 0;
	float verticalRotationValue = 0;

	public override void _Ready()
	{

		player = GetNode<CharacterBody3D>(playerNodepath);
		Position = new Vector3(player.Position.x + 0.01f, player.Position.y + 0.01f, player.Position.z + 0.01f);
		cameraPositionCast = GetNode<RayCast3D>(rayNodePath);
		camera = GetNode<Node3D>(cameraNodePath);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if (Input.IsActionPressed("camera_right")) {

			horizontalRotationValue = Mathf.Lerp(horizontalRotationValue, -0.01f, 0.08f);

		} else
		if (Input.IsActionPressed("camera_left")) {

			horizontalRotationValue = Mathf.Lerp(horizontalRotationValue, 0.01f, 0.08f);

		} else {

			horizontalRotationValue = Mathf.Lerp(horizontalRotationValue, 0f, 0.08f);

		}

		Rotate(Vector3.Up, horizontalRotationValue * sensitivity);

		if (Input.IsActionPressed("camera_forward")) {

			verticalRotationValue = Mathf.Lerp(verticalRotationValue, -0.0075f, 0.08f);

		} else
		if (Input.IsActionPressed("camera_back")) {

			verticalRotationValue = Mathf.Lerp(verticalRotationValue, 0.0075f, 0.08f);

		} else {

			verticalRotationValue = Mathf.Lerp(verticalRotationValue, 0f, 0.08f);

		}

		Rotate(Transform.basis.x, verticalRotationValue * sensitivity);
		RotationDegrees = (new Vector3(Mathf.Clamp(RotationDegrees.x, -30, 30), RotationDegrees.y, RotationDegrees.z));

	}

    public override void _PhysicsProcess(double delta)
    {
        
		Position = Position.Lerp(player.Position + Vector3.One * 0.01f, 0.08f);

		
		if (cameraPositionCast.IsColliding()) {

			if (camera.Position != cameraPositionCast.TargetPosition.Normalized() * (Position.DistanceTo(cameraPositionCast.GetCollisionPoint()) - 0.5f)) camera.Position = camera.Position.Lerp(cameraPositionCast.TargetPosition.Normalized() * (Position.DistanceTo(cameraPositionCast.GetCollisionPoint()) - 0.25f), 0.2f);

		} else {

			if (camera.Position != cameraPositionCast.TargetPosition) camera.Position = camera.Position.Lerp(cameraPositionCast.TargetPosition, 0.2f);

		}

    }

}
