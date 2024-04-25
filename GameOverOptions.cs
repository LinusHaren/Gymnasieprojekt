using Godot;
using System;

public partial class GameOverOptions : Node
{

	[Signal]
	public delegate void CurrentLevelEventHandler();


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ProcessMode = Node.ProcessModeEnum.Always;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var PlayerHealth = GetNode<RichTextLabel>("../HP");

		var GameOverText = GetNode<RichTextLabel>("GameOverText");
		var RetryButton = GetNode<MarginContainer>("MarginContainer"); 
		var QuitButton = GetNode<MarginContainer>("MarginContainer3");

		if (PlayerHealth.Text == "HP: 0" || PlayerHealth.Text == "HP: -1")
		{
			GameOverText.Show();
			RetryButton.Show();
			QuitButton.Show();


			GetTree().Paused = true;

			//EmitSignal(SignalName.CurrentLevel);
		}
	}

	public void PlayerHit(int currentHP)
	{	
		if (currentHP == 0)
		{
			GD.Print("0 HP remaining");
		}
	}

	public void OnQuitPressed()
	{
		GetTree().Quit();
	}

	public void OnRetryPressed()
	{
		EmitSignal(SignalName.CurrentLevel);
		GetTree().Paused = false;
	}
}
