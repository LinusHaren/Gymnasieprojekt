using Godot;
using System;
using System.Threading;

public partial class TimerText : RichTextLabel
{
    public int timeLeft = 100;

    public override void  _Ready(){
        Text = "Time left: " + timeLeft;
    }
    public void OnTimerCountDown()
    {
        timeLeft--;
        Text = "Time left: " + timeLeft;

    }
}
