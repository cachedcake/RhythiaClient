using Godot;
using System;

public partial class QuitButton : Button
{
    public override void _Pressed()
    {
        GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);
    }
}