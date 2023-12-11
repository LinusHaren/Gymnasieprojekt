using Godot;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Timers;

public partial class GymProject : CharacterBody2D
{
	AnimatedSprite2D animationHandler;
	Sprite2D spriteFlip;
	//För kub sprite

	AnimationPlayer AnimationSwitch;
	enum state
	{
		Attack = 0,
		Idle = 1
	}

	state currentState;

	[Signal]
	public delegate void LoseHPEventHandler(int currentHP);

	[Signal]
	public delegate void CoinEventHandler(int coinAmmount);
	public int currentHP = 3;

	[Export]
	public float jumpHeight = -750; 

	[Export]
	public float moveRight = 80;

	[Export]
	public float moveLeft = -80;

	[Export]
	public bool djump = false;
	//Double Jump

	[Export]
	public float run = 2;
	//Sprint/Running variable

	const int wallPushBack = 1300;
	//Walljump 

	const float dash = 1300;
	//Dash length

	
	public bool dmgTaken = true;
	//For the players knockback when in range of an enemy, disables all movement
	public bool dashAmmount = false;

	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	[Signal]
	public delegate void PlayerOutOfBoundsEventHandler();

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public void DmgPlayer(character_body_2d BetaEnemy)
	{
		dmgTaken = false;

		GD.Print("Player_got_hit(Player Script)");
		
		Vector2 velocity = Velocity;

		int PlayerHitPushBack = 13000000;

		velocity.X = -PlayerHitPushBack;
		velocity.Y = -PlayerHitPushBack;

		GD.Print(PlayerHitPushBack);

	}


	public override void _Ready()
	{
		animationHandler = GetNode<AnimatedSprite2D>("Sprite2D");
		spriteFlip = GetNode<Sprite2D>("CubeSprite");
		AnimationSwitch = GetNode<AnimationPlayer>("AnimationPlayer");

	}

	//If the player exits the map area
    public override void _Process(double delta)
    {
       //GD.Print(Position.Y);
	   if(Position.Y > 2000)
	   {
			EmitSignal(SignalName.PlayerOutOfBounds);
	   }
    }
	

	public void OnCollisionWEnemy(character_body_2d Area2D)
	{
		
		GD.Print("Enemy_Died");
		Area2D.QueueFree();
		//De spawns the enemy when in range
		
	}

    public override void _PhysicsProcess(double delta)
	{

		


		if (currentState != state.Attack)
		{
			currentState = state.Idle;
		}
		var attackReveal = GetNode<CollisionShape2D>("CubeSprite/Area2D/atkHitBoxCheck"); 

		spriteFlip = GetNode<Sprite2D>("CubeSprite");

		if (Input.IsActionJustPressed("attack"))
		{
			if (currentState != state.Idle)
			{
				currentState = state.Attack;
			}
			currentState = state.Attack;
			attackReveal.Disabled = false;
			
			//Tillfällig, bara för att se så att HP kan minska
			currentHP--;

			EmitSignal(SignalName.LoseHP, currentHP);
			GD.Print("Attack_Text");
			
		}
		else 
		{
			attackReveal.Disabled = true;
			currentState = state.Idle;
		}
		
			
		

		Vector2 velocity = Velocity;

		// Adds the gravity.
		if (!IsOnFloor())
		{
			velocity.Y += gravity * (float)delta * 2;
	
		} 
		else 
		{
			djump = false;
			//dashAmmount = true;
		}




		if (dmgTaken == true)
		{


		

		// Handle Jump.
		if (Input.IsActionJustPressed("Jump"))//spacebar
		{
			if (currentState != state.Attack)
			{
				currentState = state.Idle;
			}
			if(IsOnFloor() || !djump)
			{
				
				velocity.Y = jumpHeight;
				//Limits the acceleration ammount 
				if (velocity.Y > 500)
				{
					velocity.Y = 500;
				}
				if(!IsOnFloor())
				{
					djump = true;
				}
			}
			
		}  

		if (Input.IsActionPressed("fastfall") && !IsOnFloor()) //s key
		//Will make the player touch the ground faster, for example if it overjumps a platform
		{
			if (currentState != state.Attack)
			{
				currentState = state.Idle;
			}
			velocity.Y += gravity * (float)delta * 6;
		}

		else if (Input.IsActionJustPressed("fastfall") && IsOnFloor())//Drop through one way platforms
		{
			Vector2 platformDrop = Position + new Vector2(0, 1);
            Position = platformDrop;
			
		}
			

		//Handle left and right movement
		if (Input.IsActionPressed("Right"))//d key 
		{
			if (currentState != state.Attack)
			{
				currentState = state.Idle;
			}
			spriteFlip.Scale = new Vector2(0.155f,0.173f);
			if (Input.IsActionPressed("run"))//holding down shift
			{
				moveRight = moveRight * run;
				velocity.X += moveRight;
				//Limits the acceleration ammount
				
				animationHandler.Play("run");
				animationHandler.FlipH = false;
				spriteFlip.FlipH = false;

				
				if(velocity.X > 1150)
				{
					velocity.X = 1150;
					
				}
				
			}
			else 
			{
				/*
				animationHandler.Play("walk");
				animationHandler.FlipH = false;
				spriteFlip.FlipH = false;
				*/
				//old code for sprite flipping

				moveRight = 80;
				velocity.X += moveRight;
				//Limits the acceleration ammount
				if(velocity.X > 500)
				{
					velocity.X = 500;

				}
			}
		}

		else if (Input.IsActionPressed("Left"))//a key
		{
			if (currentState != state.Attack)
			{
				currentState = state.Idle;
			}
			spriteFlip.Scale = new Vector2(-0.155f,0.173f);
			if (Input.IsActionPressed("run"))//holding down shift
			{
				animationHandler.Play("run");
				animationHandler.FlipH = true;
				spriteFlip.FlipH = true;

				moveLeft = moveLeft * run;
				velocity.X += moveLeft;
				//Limits the acceleration ammount
				if(velocity.X < -1150)
				{
					velocity.X = -1150;

				}
			}
			else 
			{
				/*
				animationHandler.Play("walk");
				animationHandler.FlipH = true;
				spriteFlip.FlipH = true;
				*/
				//Old animationcode

				moveLeft = -80;
				velocity.X += moveLeft;
				//Limits the acceleration ammount
				if(velocity.X < -500)
				{
					velocity.X = -500;
				}
			}
		}
		else 
		{
			velocity.X = 0;
			animationHandler.Play("idle");
		}


		}//If dmgTaken == true end




		/*
		if (velocity.X < 0)// left
		{
			spriteFlip.Scale = new Vector2(-1,1);
			
		}
		else // right
		{
			spriteFlip.Scale = new Vector2(1,1);
		}
		*/
		
		
		//Start of wallsliding + walljump
		var wallSlideSpeed = 100;
		var wallSlideGravity = 100;
		bool isWallSliding = false;

		

		if (IsOnWall() && !IsOnFloor())
		{
			isWallSliding = true;
		}
		else 
		{
			isWallSliding = false;
		}

		
		if (isWallSliding == true && !IsOnFloor()) //Player is on a wall
		{
			if (IsOnWall() && Input.IsActionJustPressed("Jump") && Input.IsActionPressed("Right"))
			//Can jump off a wall
			{
					isWallSliding = false;
					djump = false;
					
					velocity.Y += gravity * (float)delta * 2; //Normal gravity
					velocity.X += -wallPushBack;
					//Walljump
					
					velocity.Y = jumpHeight;
					//Limits the acceleration ammount 
					if (velocity.Y > 500)
					{
						velocity.Y = 500;
					}
			}

			else if (IsOnWall() && Input.IsActionJustPressed("Jump") && Input.IsActionPressed("Left"))
			//Can jump off a wall
			{
					djump = false;
					//Restore double jump after jumping once to for easier platforming
					isWallSliding = false;
					
					
					velocity.Y += gravity * (float)delta * 2; //Normal gravity
					velocity.X += wallPushBack;
					//Walljump
					
					velocity.Y = jumpHeight;
					//Limits the acceleration ammount 
					if (velocity.Y > 500)
					{
						velocity.Y = 500;
					}
			}

			

			else if ( IsOnWall() && (Input.IsActionPressed("Right") || Input.IsActionPressed("Left")) )
			//Simple wallsliding
			{
			
			isWallSliding = false;
			velocity.Y += (wallSlideGravity * (float)delta);

			velocity.Y = Mathf.Clamp(velocity.Y, -wallSlideSpeed, wallSlideSpeed);
			//Returnerar velocity.Y variabeln om den är i mellan -wallSlideSpeed och wallSlideSpeed
			//																(-100 och 100)
			}
		}

		//var player = GetNode<GymProject>("BetaChar");

        //var respawn = GetNode<TileMap>("TileMap/Main/PlayerSpawn");

        if (currentHP == 0)
		{	
			//QueueFree();
			GD.Print("Player died");
			//player.Position = respawn.Position;
			
			currentHP = 3;
			
		}


		switch (currentState)
		{
			case state.Attack:
				AnimationSwitch.Play("Attack");
				//Skulle va något extra mot slutet, notering till mig själv, eller kolla upp
				//Finite state machine
			break;
			
			case state.Idle:
				AnimationSwitch.Play("Idle");
			break;
		}

		



		Velocity = velocity;
		MoveAndSlide();
	

	}
	
}
