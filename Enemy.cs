using Godot;
using System;

public partial class Enemy : CharacterBody2D
//							 ska vara rigidbody2D,  har för tillfället tagit bort scriptet för godot krashar och 
//							 tillåter ej Velocity för rigidbody, kommer ändra en del i Enemy.cs scriptet
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	float direction = 150;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity.Y += gravity * (float)delta * 2;
		}

		
		
		

		Velocity = velocity;
		MoveAndSlide();
		
		while (true)
		{
			velocity.X = direction;
		}
	}

	public void EnemyOutOfScreen()
	//De-spawns the enemy when its out of the screen 
	{
		QueueFree();
	}

}
