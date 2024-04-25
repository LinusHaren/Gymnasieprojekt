using Godot;
using System;

public partial class FinalScoreScreen : RichTextLabel
{
	public void OnFinalScoreCleared(int pointsAmmount)
	{
		BbcodeEnabled = true;
		Text = "[center]Final score: [/center]" + pointsAmmount;
	}
}
