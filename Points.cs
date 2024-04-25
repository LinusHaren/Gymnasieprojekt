using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class Points : RichTextLabel
{
	public void pointsAmmount(int pointsAmmount)
	{
		Text = "Points: " + pointsAmmount;
	}
}
