using Godot;
using System;

public partial class MainScene : Node
{
    [Export]
    public PackedScene levelScene {get; set;}

    TileMap level;
    GymProject player;

    TextureRect Background;

    public override void _Ready()
    {
        var PlayerSpawnLvl2 = GetNode<Marker2D>("Level2SpawnPosition");

        PlayerRespawnLvl1();

    }






    public override void _Process(double delta)
    {
        Background = GetNode<TextureRect>("Background");
        var player = GetNode<CharacterBody2D>("BetaChar");
        var camera = GetNode<Camera2D>("BetaChar/Camera2D");

        Background.Position = player.Position - Background.Size/2;
        //The backgrounds center point. Dividing both the .X and .Y sizes.
    }





    public void PlayerRespawnLvl1()
    {
        var PlayerSpawnLvl1 = GetNode<Marker2D>("Level1SpawnPosition");
        var PlayerCharacter = GetNode<CharacterBody2D>("BetaChar");

        PlayerCharacter.Position = PlayerSpawnLvl1.Position;
    }

    public void OnLevel1DeSpawnerBodyEntered(CharacterBody2D Area2D)
    {
        //DeSpawns Level 1
        var LevelOne = GetNode<TileMap>("TileMap"); //Level 1
        LevelOne.QueueFree();

        var PlayerSpawnLvl2 = GetNode<Marker2D>("Level2SpawnPosition");
        var PlayerCharacter = GetNode<CharacterBody2D>("BetaChar");

        //Changes the players position from level 1 to 2
        PlayerCharacter.Position = PlayerSpawnLvl2.Position;

        var LevelTwo = GetNode<TileMap>("Level2"); //Level 2
        LevelTwo.Show();
    }

    public void OnPlayerOutOfBounds()
    {
        var LevelOne = GetNode<TileMap>("TileMap"); //Level 1
        var LevelTwo = GetNode<TileMap>("Level2"); //Level 2

        var PlayerSpawnLvl1 = GetNode<Marker2D>("Level1SpawnPosition");
        var PlayerSpawnLvl2 = GetNode<Marker2D>("Level2SpawnPosition");
        var PlayerCharacter = GetNode<CharacterBody2D>("BetaChar");
        
        //Skapa en separat spawn för level 2, möjligen i en ny void
     
        PlayerRespawnLvl1();
    }
    
    
    
}
