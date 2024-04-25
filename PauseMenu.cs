using Godot;
using System;

public partial class PauseMenu : Node
{
	public bool pauseState = true;
	public override void _Ready()
	{
		ProcessMode = Node.ProcessModeEnum.Always;
	}

	public void OnQuitPressed()
	{
		GetTree().Quit();
	}

	public void OnPlayPressed() //Resume in this case
	{
		var PausedGameMenu = GetNode<MarginContainer>("MarginContainer");
		GD.Print("Game is un-paused");
		PausedGameMenu.Hide();
		pauseState = true;
		GetTree().Paused = false;
	}
	public override void _Process(double delta)
	{
		var PausedGameMenu = GetNode<MarginContainer>("MarginContainer");
		if (Input.IsActionJustPressed("menu") && pauseState == true) //Escape key
		{

			GD.Print("Game is paused");
			PausedGameMenu.Show();
			pauseState = false;
			GetTree().Paused = true;
			
		}
		else if (Input.IsActionJustPressed("menu") && pauseState == false)
		{

			GD.Print("Game is un-paused");
			PausedGameMenu.Hide();
			pauseState = true;
			GetTree().Paused = false;
			
		}
	}
}
