using Godot;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Timers;

public partial class GymProject : CharacterBody2D
{
	AnimatedSprite2D animationHandler;
	AnimatedSprite2D spriteFlip;

	AnimationPlayer AnimationSwitch;

	AudioStreamPlayer2D coinSE;
	AudioStreamPlayer2D EnemyTakingDmgSE;
	AudioStreamPlayer2D JumpSE;
	AudioStreamPlayer2D PlayerTakingDmgSE;
	AudioStreamPlayer2D SwordSwingSE;
	//SE = Sound Effect

	RichTextLabel FinalScore;

	Timer IFramesTimer;
	Timer attackCD;
	

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

	[Signal]
	public delegate void PointsEventHandler(int pointsAmmount);

	[Signal]
	public delegate void DeathEventHandler();
	[Signal]
	public delegate void PlayerOutOfBoundsEventHandler();


	public int currentHP = 3;

	public int pointsAmmount = 0;

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

	public bool timeOut = true;
	public bool timeOut2 = true;
	public bool timeOut3 = true;

	//To create invincibility frames, stops the player from being hit repeatedly
	public bool StartIFramesTimer = false;

	const int wallPushBack = 1300;
	//Walljump 

	public bool isWallJumpAbable = true;

	
	public bool dmgTaken = true;



	public bool attackCooldownState = false;
	//Main function is to stop the player from holding down
	//or just straight up mashing (repeatedly press) the attack button through levels.

	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public void CoinPickup(Area2D area)
	{
		GD.Print("Coin collected");
		pointsAmmount = pointsAmmount + 100;
		EmitSignal(SignalName.Points, pointsAmmount);

		coinSE.Play();
	}

	public void DmgPlayer(character_body_2d BetaEnemy)
	//					 (Node2D body)
	{
		if (StartIFramesTimer == false)
		{
			
		
			//dmgTaken = false;

			GD.Print("Player_got_hit(Player Script)");

			Vector2 playerPosition = Position;


			float Yknockback = 140f;
			
			float Xknockback = 0f;

			//Updates the player's position (moves it a bit above the current position)
			Position = new Vector2(playerPosition.X - Xknockback, playerPosition.Y - Yknockback);
			
			//Decreases the players HP by 1 
			currentHP--;
			EmitSignal(SignalName.LoseHP, currentHP);
			PlayerTakingDmgSE.Play();

			
			StartIFramesTimer = true;

			IFramesTimer.Start(3);

			if(IFramesTimer == null)
			{
				GD.Print("hello");
			}
		}
	}


	public void OnIFramesTimerTimeout()
	{
		GD.Print("I-Frames Timer timeout");
		if (StartIFramesTimer == true)
		{
			//Problem...uneven ammount of time for Iframes
			GD.Print("Player was invincible");
			StartIFramesTimer = false;
			
			Vector2 playerPosition = Position;

			float Yknockback = 5f;

			//Moves the player a bit above its current position in order for the 'DmgPlayer()' void to activate again.
			//Not enough to be noticible while playing normally though.
			Position = new Vector2(playerPosition.X, playerPosition.Y - Yknockback);
		}
	}

	public void OnInvisbleWallTouched(GymProject Area2D)
	{
		GD.Print("Level Boundry Reached");
		Vector2 playerPosition = Position;
		
		var spawnPosition = GetNode<Marker2D>("../Level2SpawnPosition");
		Position = spawnPosition.Position;
	}

	public void OnCancelWallJumpEntered(GymProject Area2D)
	{
		isWallJumpAbable = false;
		GD.Print("Player cant walljump");
	}
	public void OnCancelWallJumpExcited(GymProject Area2D)
	{
		isWallJumpAbable = true;
		GD.Print("Player can walljump");
	}
	
	public override void _Ready()
	{
		animationHandler = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		//Tidigare var det: 	  <AnimatedSprite2D>("Sprite2D");
		spriteFlip = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		//Ta bort AnimationPlayer, helt onödig
		AnimationSwitch = GetNode<AnimationPlayer>("AnimationPlayer");

		coinSE = GetNode<AudioStreamPlayer2D>("CoinSE");
		EnemyTakingDmgSE = GetNode<AudioStreamPlayer2D>("EnemyTakingDmgSE");
		JumpSE = GetNode<AudioStreamPlayer2D>("JumpSE");
		PlayerTakingDmgSE = GetNode<AudioStreamPlayer2D>("PlayerTakingDmgSE");
		SwordSwingSE = GetNode<AudioStreamPlayer2D>("SwordSwingSE");

		FinalScore = GetNode<RichTextLabel>("UI/FinalScoreScreen");

		IFramesTimer = GetNode<Timer>("IFramesTimer");
		attackCD = GetNode<Timer>("AttackCDTimer");
	}


		
	public void OnPlayerOnSpike(GymProject CharacterBody2D)
	//(CharacterBody2D BetaChar)
	//(CharacterBody2D Area2D)
	{
		//if (StartIFramesTimer == false)
		//{	
			GD.Print("SPIKES MADE CONTACT");		
			//Decreases the players HP by 1 
			currentHP--;
			EmitSignal(SignalName.LoseHP, currentHP);
			PlayerTakingDmgSE.Play();

			StartIFramesTimer = true;
			IFramesTimer.Start(3);

			Vector2 playerPosition = Position;
			float Yknockback = 5f;
			Position = new Vector2(playerPosition.X, playerPosition.Y - Yknockback);
			
		//}
	}

	public void OnPlayerOnMagma(CharacterBody2D Area2D) 
	// Does not have Iframes but instead a higher "knockback"
	// (May or may not be used in the final product)
	{
			GD.Print("Player_got_hit(Player Script)");

			Vector2 playerPosition = Position;
			float Yknockback = 300f;
			Position = new Vector2(playerPosition.X, playerPosition.Y - Yknockback);
			
			//Decreases the players HP by 1 
			currentHP--;
			EmitSignal(SignalName.LoseHP, currentHP);
			PlayerTakingDmgSE.Play();
	}

	




//Start on all things involving timers and scores


// My core idea for these are to add additional points to the player depending on how
// fast they cleared the level, because of timers being a bit buggy for me though I decided on
// essentially a gold, silver and bronze point system where one dissapears after a certain ammount of time.
// If the player then is fast enough for the "gold" time they will also get the bronze and silver time bonuses.
// So in other words, "gold" or the EndOfLevel is worth more than itself alone.
	public void EndOfLevel(CharacterBody2D Area2D)
	{
		if (timeOut)
		{
			GD.Print("1000 points earned");
			pointsAmmount = pointsAmmount + 1000;
			EmitSignal(SignalName.Points, pointsAmmount);
		}
		
		
		GD.Print("Final Score text is showing!");
	}

	public void EndOfLevel800(CharacterBody2D Area2D)
	{
		if (timeOut2)
		{
			GD.Print("800 points earned");
			pointsAmmount = pointsAmmount + 800;
			EmitSignal(SignalName.Points, pointsAmmount);
			
		}
		
		GD.Print("Final Score text is showing!");
	}

	public void EndOfLevel500(CharacterBody2D Area2D)
	{
		if (timeOut3)
		{
			GD.Print("500 points earned");
			pointsAmmount = pointsAmmount + 500;
			EmitSignal(SignalName.Points, pointsAmmount);
			
		}

		GD.Print("Final Score text is showing!");
	}


	//Current time: 50 seconds
	public void OnTimerTimeout1()
	{
		GD.Print("Timer 1 (5s) stopped");

		timeOut = false;
	}

	//Current time: 60 seconds
	public void OnTimerTimeOut2()
	{
		GD.Print("Timer 2 (10s) stopped");

		timeOut2 = false;
	}

	//Current time: 80 seconds
	public void OnTimerTimeOut3()
	{
		GD.Print("Timer 3 (15s) stopped");

		timeOut3 = false;
	}

	public void GameOverTimer()
	{
		var tree = GetTree();
		tree.ReloadCurrentScene();
	}

	//Just here to prevent the player from repeatedly pressing/holding attack through levels
	public void OnAttackCDTimerTimeOut()
	{
		if (attackCooldownState == true)
		{
			GD.Print("Attack Cooldown Reset");
		}
		else if (attackCooldownState == false)
		{
			attackCooldownState = true;
		}
	}


//End on all things involving timers and scores

	public override void _Process(double delta)
	{
	   
	   //If the player falls of a ledge or somehow excits the map area
	   if(Position.Y > 2000)
	   {
			EmitSignal(SignalName.PlayerOutOfBounds);
			
			currentHP--;
			EmitSignal(SignalName.LoseHP, currentHP);
			PlayerTakingDmgSE.Play();
	   }

	   //Check MainScene.cs, how I reseted the score with Player.pointsAmmount = -1;
	   if (pointsAmmount == -1)
	   {
			GD.Print("Points reseted after level change");
			pointsAmmount = 0;
			EmitSignal(SignalName.Points, pointsAmmount);
	   }
	}
	
	//Attack hits an enemy
	public void OnCollisionWEnemy(character_body_2d Area2D)
	{
		EnemyTakingDmgSE.Play();
		
		GD.Print("Enemy died");
		Area2D.QueueFree();
		//De spawns the enemy when in range
		
		pointsAmmount = pointsAmmount + 200;
		EmitSignal(SignalName.Points, pointsAmmount);

	}


	
	public void OnAnimatedSprite2DAnimationFinished()
	{
		//GD.Print("Animation finished");
		currentState = state.Idle;
		animationHandler.Play("idle");
	}


	public override void _PhysicsProcess(double delta)
	{

		


		if (currentState != state.Attack)
		{
			currentState = state.Idle;
		}
		var attackReveal = GetNode<CollisionShape2D>("AnimatedSprite2D/Area2D/atkHitBoxCheck"); 



		
		
			
		

		Vector2 velocity = Velocity;

		// Adds the gravity.
		if (!IsOnFloor())
		{
			velocity.Y += gravity * (float)delta * 2;
	
		} 
		else
		{
			djump = false;
		}




		if (dmgTaken == true)
		{


		

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
			spriteFlip.Scale = new Vector2(0.805f,0.755f);
			//							  (0.105f,0.173f);

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

			else if (Input.IsActionPressed("Right") & Input.IsActionPressed("attack"))
			{


				if (attackCooldownState == true)
				{
					currentState = state.Attack;
					attackReveal.Disabled = false;
					attackCooldownState = false;
					animationHandler.Play("WalkAndAttack");
					SwordSwingSE.Play();

					GD.Print("Walk and Attack");
				}
				else if (attackCooldownState == false)
				{
					//animationHandler.Play("walk");
				}

				moveRight = 80;
				velocity.X += moveRight;
				//Limits the acceleration ammount
				if(velocity.X > 500)
				{
					velocity.X = 500;
				}
				
			}

			else 
			{
				animationHandler.Play("walk");
				animationHandler.FlipH = false;
				spriteFlip.FlipH = false;

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
			spriteFlip.Scale = new Vector2(-0.805f,0.755f);
			//							  (-0.155f,0.173f);

			if (Input.IsActionPressed("run"))//holding down shift
			{
				animationHandler.Play("run");
				//animationHandler.FlipH = true;
				//spriteFlip.FlipH = true;
				

				moveLeft = moveLeft * run;
				velocity.X += moveLeft;
				//Limits the acceleration ammount
				if(velocity.X < -1150)
				{
					velocity.X = -1150;

				}
			}


			//Ny animationkod för två samtidigt
			else if (Input.IsActionPressed("Left") & Input.IsActionPressed("attack"))
			{


				if (attackCooldownState == true)
				{
					
					if ( animationHandler.IsPlaying() )
					{
						animationHandler.Stop();
						animationHandler.Play("WalkAndAttack");
					}
					
					currentState = state.Attack;
					attackReveal.Disabled = false;
					attackCooldownState = false;
					animationHandler.Play("WalkAndAttack");
					SwordSwingSE.Play();

					GD.Print("LEFT & ATK REACHED");
					GD.Print("Walk and Attack");
				}
				else if (attackCooldownState == false)
				{
					if ( animationHandler.IsPlaying() )
					{
						GD.Print("animation stopped");
						animationHandler.Stop();
					}
					animationHandler.Play("walk");
				}

				moveLeft = -80;
				velocity.X += moveLeft;
				//Limits the acceleration ammount
				if(velocity.X < -500)
				{
					velocity.X = -500;
				}
				
			}

			else 
			{
				
				animationHandler.Play("walk");
				//animationHandler.FlipH = true;
				//spriteFlip.FlipH = true;
				

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
			if(currentState != state.Attack)
			{
				//animationHandler.Play("idle");

			}
		}


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
				JumpSE.Play();
				animationHandler.Play("jump");
			}
		}	
		if (Input.IsActionJustPressed("attack"))
		//Hela animationen spelas bara om man håller inne och ändrar till IsActionPressed
		{
			if (attackCooldownState == true)
			{
				currentState = state.Attack;
				attackReveal.Disabled = false;
				attackCooldownState = false;
				SwordSwingSE.Play();
				animationHandler.Play("attack");

				GD.Print("Attack_Text");
			}	
		}
		else 
		{
			//attackReveal.Disabled = true;
			currentState = state.Idle;
		}


		}//If dmgTaken == true end

		
			if (Input.IsActionPressed("run") & (Input.IsActionPressed("Left") || 
			Input.IsActionPressed("Right")) & Input.IsActionPressed("attack"))
			{
				if (attackCooldownState == true)
				{
					currentState = state.Attack;
					attackReveal.Disabled = false;
					attackCooldownState = false;
					animationHandler.Play("WalkAndAttack");
					SwordSwingSE.Play();

					GD.Print("RUN AND ATK");
				}
				else if (attackCooldownState == false)
				{
					animationHandler.Play("run");
				}
			}
	
		

		
		
		
		
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
			if (isWallJumpAbable)
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
						animationHandler.Play("walljump");
						animationHandler.FlipH = false;
						spriteFlip.FlipH = false;
						JumpSE.Play();
				}

				else if (IsOnWall() && Input.IsActionJustPressed("Jump") && Input.IsActionPressed("Left"))
				//Can jump off a wall
				{
					
						djump = false;
				
						
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
						animationHandler.Play("walljump");
						JumpSE.Play();
				}

				

				else if ( IsOnWall() && (Input.IsActionPressed("Right") || Input.IsActionPressed("Left")) )
				//Simple wallsliding
				{
					animationHandler.Play("walljump"); //Same animation used for walljump
				
					isWallSliding = false;
					velocity.Y += (wallSlideGravity * (float)delta);

					velocity.Y = Mathf.Clamp(velocity.Y, -wallSlideSpeed, wallSlideSpeed);
				
				}
			}
		}
		
		
		//Death and respawn for the player
		if (currentHP <= 0)
		{
			GD.Print("Player died");
			
			
			//currentHP = 3;
		}


		switch (currentState)
		{

			case state.Idle:
				AnimationSwitch.Play("Idle");
			break;

		}

		



		Velocity = velocity;
		MoveAndSlide();
	

	}
	
}
