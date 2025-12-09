using Godot;
using System;

public partial class SettingsButton : Button
{
    public override void _Pressed()
    {
        SettingsManager.ShowMenu(true);
    }
}