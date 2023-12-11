using Godot;
using System;

public partial class character_body_2d : CharacterBody2D
{	
	
	public void DmgPlayer(GymProject BetaChar)
	{
		
		GD.Print("Player_Got_Hit(Enemy Script)");

		//var PlayerCube = GetNode<GymProject>("BetaChar");

		Vector2 velocity = BetaChar.Velocity;

		const int PlayerHitPushBack = 1300;

		velocity.X = -PlayerHitPushBack;
		velocity.Y = -PlayerHitPushBack;
		
	}
	
	
	public void Hit(GymProject Character2D)
	{
		GD.Print("Enemy Died");
		QueueFree();
	}
	
	private void EnemyDespawn()
	{
		//De-spawns the enemy when leaving the screen
		//Avoids overloading

		//QueueFree();
		
		//...abusable, player can easily run back and forward again when the enemy spawns to despawn it imidietly...
		//(kommentarer till mig själv) tar bort QueueFree för tillfället
	}
	private RayCast2D WallCheck;
	private RayCast2D WallCheckRight;
	private RayCast2D PlayerCheckAbove;

		
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public float AiMoveSpeed = -100;

	public bool MovingLeft = true;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		

		//Adds gravity
		if (!IsOnFloor())
		{
			velocity.Y += gravity * (float)delta * 2;
		}

		
		
		WallCheck = GetNode<RayCast2D>("WallCheck");
		WallCheckRight = GetNode<RayCast2D>("WallCheckRight");
		PlayerCheckAbove = GetNode<RayCast2D>("PlayerCheck");
		
		if (WallCheck.IsColliding())
		//Touched a left wall
		{
			MovingLeft = false;
		}
		else if (WallCheckRight.IsColliding())
		//Touched a right wall
		{
			MovingLeft = true;
		}
		
		if (PlayerCheckAbove.IsColliding())
		{
			QueueFree();
		}


		if (MovingLeft && IsOnFloor())
		{
			velocity.X = AiMoveSpeed;
			//Moving left
		}
		else if (MovingLeft == false)
		{
			velocity.X = -AiMoveSpeed;
			//Moving Right
		}


		
		
		Velocity = velocity;
		MoveAndSlide();

	}


}