using Godot;
using System;

public partial class HP : RichTextLabel
{
	public void PlayerHit(int currentHP)
	{
		Text = "HP: " + currentHP;

		if (currentHP == 0)
		{	
			/*
			currentHP = 3;
			Text = "HP: " + currentHP;
			*/
		}

		if (Text == "HP: 3" && currentHP != 3)
		{
			currentHP = 3;
		}

	}
}
