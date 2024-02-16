using Godot;
using System;

public partial class Points : RichTextLabel
{
    public void pointsAmmount(int pointsAmmount)
    {
        Text = "Points: " + pointsAmmount;
    }
}
