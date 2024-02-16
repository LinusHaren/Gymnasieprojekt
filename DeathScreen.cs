using Godot;
using System;
//using System.Security.Cryptography.X509Certificates;

public partial class DeathScreen : RichTextLabel
{
    public override void _Process(double delta)
	{
        if(Input.IsActionJustPressed("Respawn"))
		{
			GD.Print("Respawn");
			var tree = GetTree();
			tree.ReloadCurrentScene();
			tree.Paused = false;
		}
    }
    
    public void OnPlayerDeath()
    {
        GD.Print("Player Died (DeathScreen)");
        Show();
    }   

    public void TimeOver()
    {
        GD.Print("Level timer (100s) stopped");
        Show();
    }
}
