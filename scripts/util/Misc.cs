using System;
using System.IO;
using Godot;
using System.Collections.Generic;

namespace Util;

public class Misc
{
    public static GodotObject OBJParser = (GodotObject)GD.Load<GDScript>("res://scripts/OBJParser.gd").New();

    public static string GetProfile()
    {
        return File.ReadAllText($"{Constants.USER_FOLDER}/current_profile.txt");
    }

    public static ImageTexture GetModIcon(string mod)
    {
        ImageTexture tex = new();

        switch (mod)
        {
            case "NoFail":
                tex = SkinManager.Instance.Skin.ModNofailImage;
                break;
            case "Spin":
                tex = SkinManager.Instance.Skin.ModSpinImage;
                break;
            case "Ghost":
                tex = SkinManager.Instance.Skin.ModGhostImage;
                break;
            case "Chaos":
                tex = SkinManager.Instance.Skin.ModChaosImage;
                break;
            case "Flashlight":
                tex = SkinManager.Instance.Skin.ModFlashlightImage;
                break;
            case "HardRock":
                tex = SkinManager.Instance.Skin.ModHardrockImage;
                break;
        }

        return tex;
    }
}
