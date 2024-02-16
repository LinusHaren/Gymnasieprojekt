using Godot;
using System;

public partial class Coin : Area2D
{
    public void PlayerEntered(GymProject CharacterBody2D)
    {
        QueueFree();
    }
}
