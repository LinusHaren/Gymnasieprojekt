using Godot;
using System;

public partial class Menu : Node
{
	public void OnPlayPressed()
	{
		GetTree().ChangeSceneToFile("res://MainScene.tscn");
	}

	public void OnQuitPressed()
	{
		GetTree().Quit();
	}
}
