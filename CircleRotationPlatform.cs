using Godot;
using System;

public partial class CircleRotationPlatform : AnimatableBody2D
{
    AnimationPlayer P;

    enum state
    {
        circleRotation
    }

    state currentState;

    public override void _Ready()
	{
        
        P = GetNode<AnimationPlayer>("AnimationPlayer");
        currentState = state.circleRotation;
        P.Play("circleRotation");
        
    }
}
