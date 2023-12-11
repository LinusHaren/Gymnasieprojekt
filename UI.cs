using Godot;
using System;

public partial class UI : CanvasLayer
{
	private void OnStartPressed()
	{
		GetNode<Button>("StartButton").Hide();
		
	}
}
