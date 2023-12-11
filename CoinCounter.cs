using Godot;
using System;

public partial class Coin : RichTextLabel
{
	public void CoinPickup(int coinAmmount)
	{
		if (true)
		{
			Text = "Coins: " + coinAmmount;
		}

	}
}
