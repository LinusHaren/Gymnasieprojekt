using Godot;
using System;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

public partial class MainScene : Node
{
	[Export]
	public PackedScene levelScene {get; set;}

	TileMap level;
	GymProject player;

	TextureRect Background;

	public bool LevelChange = false;

	public bool RetryPresed = false;

	public bool Level2CheckpointReached = false;
	public bool Level1CheckpointReached = false;

	public override void _Ready()
	{
		if (LevelChange == true)
		{
			PlayerRespawnLvl2();
		}
		else if(LevelChange == false)
		{
			PlayerRespawnLvl1();	
		}

	}




	public override void _Process(double delta)
	{
		Background = GetNode<TextureRect>("Background");
		var player = GetNode<CharacterBody2D>("BetaChar");

		Background.Position = player.Position - Background.Size/2;
		//The backgrounds center point. Dividing both the .X and .Y sizes.
		
	}

	public void OnCancelWallJumpEntered(GymProject Area2D)
	{
		GD.Print("Player cant walljump");
		var player = GetNode<GymProject>("BetaChar");
		player.isWallJumpAbable = false;
	}

	public void OnCancelWallJumpExcited(GymProject Area2D)
	{
		GD.Print("Player can walljump");
		var player = GetNode<GymProject>("BetaChar");
		player.isWallJumpAbable = true;
	}

	public void OnHealthReset(int currentHP)
	{
		var Player = GetNode<GymProject>("BetaChar");
		if (LevelChange == true) //Level 2
		{
			if (currentHP <= 0)
			{
				Player.Hide();
				//PlayerRespawnLvl2();
				//currentHP = 3;
				var HP = GetNode<RichTextLabel>("BetaChar/UI/HP");
				GetTree().Paused = true;
				//HP.Text = "HP: 3";

			}
		}
		else if(LevelChange == false) //Level 1
		{
			if (currentHP <= 0)
			{
				Player.Hide();
				GetTree().Paused = true;
				
				//PlayerRespawnLvl1();
			}
		}
	}


	//Level 1 Checkpoint
	public void OnLvl1CheckpointReached(GymProject Area2D)
	{
		GD.Print("Checkpoint reached");

		Level1CheckpointReached = true;

		var GrayLvl1CpFlag = GetNode<Sprite2D>("Level1/CheckpointLvl1/UnMarkedFlag");
		var RedLvl1CpFlag = GetNode<Sprite2D>("Level1/CheckpointLvl1/MarkedFlag");

		GrayLvl1CpFlag.Hide();
		RedLvl1CpFlag.Show();
	}

	//Level 2 Checkpoint
	public void OnCheckpointReached(GymProject Area2D)
	{
		GD.Print("Checkpoint reached");

		Level2CheckpointReached = true;

		var GrayLvl2CpFlag = GetNode<Sprite2D>("Level2/Checkpoint/UnMarkedFlag");
		var RedLvl2CpFlag = GetNode<Sprite2D>("Level2/Checkpoint/MarkedFlag");

		GrayLvl2CpFlag.Hide();
		RedLvl2CpFlag.Show();
	}

	public void PlayerRespawnLvl1()
	{
		var PlayerSpawnLvl1 = GetNode<Marker2D>("Level1SpawnPosition");
		var PlayerCharacter = GetNode<GymProject>("BetaChar");
		GetNode<HP>("BetaChar/UI/HP").PlayerHit(3);

		GetNode<MarginContainer>("BetaChar/UI/GameOverOptions/MarginContainer").Hide();
		GetNode<MarginContainer>("BetaChar/UI/GameOverOptions/MarginContainer3").Hide();
		GetNode<RichTextLabel>("BetaChar/UI/GameOverOptions/GameOverText").Hide();

		var Lvl1CP = GetNode<Marker2D>("Level1Checkpoint");

		if (Level1CheckpointReached == false)
		{
			PlayerCharacter.currentHP = 3;
			PlayerCharacter.Position = PlayerSpawnLvl1.Position;			
		}
		else
		{
			PlayerCharacter.currentHP = 3;
			PlayerCharacter.Position = Lvl1CP.Position;
		}

	}

	public void PlayerRespawnLvl2()
	{
		var PlayerSpawnLvl2 = GetNode<Marker2D>("Level2SpawnPosition");
		var PlayerCharacter = GetNode<GymProject>("BetaChar");
		GetNode<HP>("BetaChar/UI/HP").PlayerHit(3);

		GetNode<MarginContainer>("BetaChar/UI/GameOverOptions/MarginContainer").Hide();
		GetNode<MarginContainer>("BetaChar/UI/GameOverOptions/MarginContainer3").Hide();
		GetNode<RichTextLabel>("BetaChar/UI/GameOverOptions/GameOverText").Hide();
		
		var Lvl2CP = GetNode<Marker2D>("Level2Checkpoint");

		if (Level2CheckpointReached == false)
		{
			PlayerCharacter.currentHP = 3;
			PlayerCharacter.Position = PlayerSpawnLvl2.Position;			
		}
		else
		{
			PlayerCharacter.currentHP = 3;
			PlayerCharacter.Position = Lvl2CP.Position;
		}
	}

//Also for Level 2 because I did not think it through all to well when originally naming things...
	public void OnLevel1FinalScoreRegisterBodyEntered(CharacterBody2D Area2D)
	{		
		//Pauses the game for a period of time,
		//made so the player can look at their score before moving on
		Thread.Sleep(2000);
	}

	public void OnLevel1UIRemovalBodyEntered(CharacterBody2D Area2D)
	{
		//Made to only show off the score for a bit before moving on
		var HP = GetNode<RichTextLabel>("BetaChar/UI/HP");
		HP.Hide();
		var Points = GetNode<RichTextLabel>("BetaChar/UI/Points");
		Points.Hide();
		var TimeLeft = GetNode<RichTextLabel>("BetaChar/UI/TimerText");
		TimeLeft.Hide();
		var Player = GetNode<CharacterBody2D>("BetaChar");
		Player.Hide();

		//Show your score for 3 seconds (might change later)
		var FinalScore = GetNode<RichTextLabel>("BetaChar/UI/FinalScoreScreen");
		FinalScore.Show();

		Level1CheckpointReached = false;
	}

	public void OnLevel1DeSpawnerBodyEntered(CharacterBody2D Area2D)
	{
		
		//Reveal everything again
		var HP = GetNode<RichTextLabel>("BetaChar/UI/HP");
		HP.Show();
		var Points = GetNode<RichTextLabel>("BetaChar/UI/Points");
		Points.Show();
		var TimeLeft = GetNode<TimerText>("BetaChar/UI/TimerText");
		TimeLeft.Show();
		var Player = GetNode<GymProject>("BetaChar");
		Player.Show();

		
		var FinalScore = GetNode<RichTextLabel>("BetaChar/UI/FinalScoreScreen");
		FinalScore.Hide();


		//DeSpawns Level 1
		var LevelOne = GetNode<TileMap>("Level1"); //Level 1
		LevelOne.QueueFree();

		var PlayerSpawnLvl2 = GetNode<Marker2D>("Level2SpawnPosition");
		var PlayerCharacter = GetNode<GymProject>("BetaChar");

		//Changes the players position from level 1 to 2
		PlayerCharacter.Position = PlayerSpawnLvl2.Position;

		var LevelTwo = GetNode<TileMap>("Level2"); //Level 2
		LevelTwo.Show();

		PlayerCharacter.currentHP = 3;
		GetNode<HP>("BetaChar/UI/HP").PlayerHit(3);

		TimeLeft.timeLeft = 100;
		Player.pointsAmmount = -1;

		//Reset the time bonus timers for Level 2	
		var Timer1000P = GetNode<Timer>("BetaChar/EndGoal1sTimer");
		Timer1000P.WaitTime = 50;
		var Timer800P = GetNode<Timer>("BetaChar/EndGoal2sTimer");
		Timer800P.WaitTime = 60;
		var Timer500P = GetNode<Timer>("BetaChar/EndGoal3sTimer");
		Timer500P.WaitTime = 80;

		LevelChange = true;
	}

	public void OnPlayerOutOfBound()
	{
		var LevelOne = GetNode<TileMap>("Level1"); //Level 1
		var LevelTwo = GetNode<TileMap>("Level2"); //Level 2

		var PlayerSpawnLvl1 = GetNode<Marker2D>("Level1SpawnPosition");
		var PlayerSpawnLvl2 = GetNode<Marker2D>("Level2SpawnPosition");
		var PlayerCharacter = GetNode<GymProject>("BetaChar");
		
		//PlayerCharacter.currentHP--;
		//GetNode<HP>("BetaChar/UI/HP").PlayerHit();

		var Lvl2CP = GetNode<Marker2D>("Level2Checkpoint");
		var Lvl1CP = GetNode<Marker2D>("Level1Checkpoint");

	
		if (LevelTwo.Visible == true)
		{
			//PlayerRespawnLvl2();
			if (Level2CheckpointReached == false)
			{
				PlayerCharacter.Position = PlayerSpawnLvl2.Position;
			}
			else
			{
				PlayerCharacter.Position = Lvl2CP.Position;	
			}
			
			GD.Print("Player Respawned in level 2");
		}
		else
		{
			if (Level1CheckpointReached == false)
			{
				PlayerCharacter.Position = PlayerSpawnLvl1.Position;	
			}
			else
			{
				PlayerCharacter.Position = Lvl1CP.Position;
			}
			//PlayerRespawnLvl1();

			GD.Print("Player Respawned in level 1");
		}
	
	}

	public void CurrentLevelRetry()
	{
		RetryPresed = true;

		var Player = GetNode<GymProject>("BetaChar");
		Player.Show();
		
		var TimeLeft = GetNode<TimerText>("BetaChar/UI/TimerText");
		TimeLeft.timeLeft = 100;

		if (LevelChange == true)
		{
			var GameOverRetryButton = GetNode<Button>("BetaChar/UI/GameOverOptions/MarginContainer/VBoxContainer/Retry");
			GD.Print("Retry on level 2");
				
			PlayerRespawnLvl2();
			
		}
		else if (LevelChange == false)
		{
			PlayerRespawnLvl1();

			//Current problem, HP Stays at 0 when pressing Retry in GameOverOptions in both levels
		}
	}

	public void MainMenuReturn(GymProject Area2D)
	{
		GetTree().ChangeSceneToFile("res://Menu.tscn");
	}
	
}








