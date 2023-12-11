using Godot;
using Godot.NativeInterop;
using System;
using System.Runtime.CompilerServices;
using System.Threading;


public partial class Main : Node
{
    public override void _Ready()
    {
        PlayerRespawn();
        EnemyRespawn();
    }



    public void PlayerRespawn()
    {
        var player = GetNode<GymProject>("BetaChar");

        var respawn = GetNode<Marker2D>("PlayerSpawn");
        
        player.Position = respawn.Position;

        
    }


    public void PlayerHit(int currentHP)
	{
		if (currentHP == 0)
		{
            /*
			currentHP = 3;
			var tree = GetTree();
			tree.ReloadCurrentScene();
            */
		}
	}

    

    public void EnemyRespawn()
    {
        var enemy = GetNode<character_body_2d>("BetaEnemy");
        var EnemySpawnPosition = GetNode<Marker2D>("EnemySpawn");

        enemy.Position = EnemySpawnPosition.Position;
    }

    public void OnBetaCharPlayerOutOfBounds()
    {
        PlayerRespawn();
    }
    /*
     public void PlayerHit(int currentHP)
	{
        if (currentHP <= 0)
        {
            GD.Print("Respawn");
			//In i deathscreen, längst ner till process, byt inherit till when paused
			var tree = GetTree();
			tree.ReloadCurrentScene();
        }
    }
    */
}
