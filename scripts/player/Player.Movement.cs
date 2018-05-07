using Godot;
using System;

public partial class Player : KinematicBody
{
	private int speed = 10;
	private int acceleration = 3;
	private int deAcceleration = 5;

	private float gravity = -20f;
	private Vector3 velocity;

    private void _MovementControl(float delta)
	{
		Transform camera = ((Camera) GetNode("Camera")).GetGlobalTransform();

		Vector3 charRot = new Vector3();
		Vector3 direction = new Vector3();
		Vector2 joystick = new Vector2(Input.GetJoyAxis(0, 0), Input.GetJoyAxis(0, 1));
		bool isMoving = false;

		if (joystick.Length() > 0.25)
		{
			direction.x = joystick.x;
			direction.z = joystick.y;
			isMoving = true;
		}
		else
		{
			if (Input.IsActionPressed("ui_up"))
			{
				direction += -camera.basis[0];
				isMoving = true;
			}
		
			if (Input.IsActionPressed("ui_left"))
			{
				direction += camera.basis[2];
				isMoving = true;
			}
			
			if (Input.IsActionPressed("ui_right"))
			{
				direction += -camera.basis[2];
				isMoving = true;
			}
			
			if (Input.IsActionPressed("ui_down"))
			{
				direction += camera.basis[0];
				isMoving = true;
			}
		}

		direction.y = 0;
		direction = direction.Normalized();

		velocity.y += gravity * delta;
		
		Vector3 hv = new Vector3(velocity.x, 0, velocity.z);
		
		var newPos = direction * speed;
		var accel = deAcceleration;
		
		if (direction.Dot(hv) > 0)
		{
			accel = acceleration;
		}

		hv = hv.LinearInterpolate(newPos, accel * delta);

		velocity.x = hv.x;
		velocity.z = hv.z;

		if (isMoving)
		{
			Spatial node = (Spatial) GetNode("Mesh");
			float angle = Mathf.Atan2(hv.z, hv.x);
			charRot = node.GetRotation();
			charRot.z = angle;
			node.SetRotation(charRot);
		}
 
		velocity = MoveAndSlide(velocity, new Vector3(0, 1, 0));

		if (IsOnFloor() && Input.IsActionPressed("ui_accept"))
		{
			velocity.y = 10;
		}

		RpcUnreliable("OnMove", GetGlobalTransform().origin, Id, isMoving, charRot);
	}
}
