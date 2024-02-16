using Godot;
using Godot.NativeInterop;
using System;
using System.Runtime.CompilerServices;
using System.Threading;


public partial class Main : Node
{
    [Signal]
	public delegate void LoseHPEventHandler(int currentHP);
    public int currentHP = 3;

    public override void _Ready() //Old code that only works in playing the "Level1" scene
    {                             //Remade i MainScene.cs
        PlayerRespawn();
        EnemyRespawn();
    }


    public void PlayerRespawn()
    {
        /*
        var player = GetNode<GymProject>("BetaChar");
        
        var respawn = GetNode<Marker2D>("PlayerSpawn");
        
        player.Position = respawn.Position;
        
        */
        //Old code thats only used in the original main scene in level 1
    }


    public void PlayerHit(int currentHP)
	{
        
		if (currentHP <= 0)
		{
            
			currentHP = 3;
		}
	}

    

    public void EnemyRespawn()
    {
        var enemy = GetNode<character_body_2d>("BetaEnemy");
        var EnemySpawnPosition = GetNode<Marker2D>("EnemySpawn");

        enemy.Position = EnemySpawnPosition.Position;
    }
/*
    public void OnBetaCharPlayerOutOfBounds()
    {
        PlayerRespawn();
    }
*/
}
