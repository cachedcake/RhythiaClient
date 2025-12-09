using Godot;
using System;

public partial class DiscordButton : TextureButton
{
    public override void _Pressed()
    {
        OS.ShellOpen("https://discord.gg/rhythia");
    }
}