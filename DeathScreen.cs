using Godot;
using System;
//using System.Security.Cryptography.X509Certificates;

public partial class DeathScreen : RichTextLabel
{
    public override void _Ready()
	{
		ProcessMode = Node.ProcessModeEnum.Always;
	}

    public override void _Process(double delta)
	{

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
