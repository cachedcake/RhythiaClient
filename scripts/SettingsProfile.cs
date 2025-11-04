using System;
using System.IO;
using Godot;
using Godot.Collections;
using System.Reflection;

public class SettingsProfile
{
    public static bool Fullscreen { get; set; } = false;
    public static double VolumeMaster { get; set; } = 50;
    public static double VolumeMusic { get; set; } = 50;
    public static double VolumeSFX { get; set; } = 50;
    public static bool AutoplayJukebox { get; set; } = true;
    public static bool AlwaysPlayHitSound { get; set; } = false;
    public static string Skin { get; set; } = "default";
    public static double FoV { get; set; } = 70;
    public static double Sensitivity { get; set; } = 0.5;
    public static double Parallax { get; set; } = 0.1;
    public static double ApproachRate { get; set; } = 32;
    public static double ApproachDistance { get; set; } = 20;
    public static double ApproachTime { get; set; } = ApproachDistance / ApproachRate;
    public static double FadeIn { get; set; } = 15;
    public static bool FadeOut { get; set; } = true;
    public static bool Pushback { get; set; } = true;
    public static double NoteSize { get; set; } = 0.875;
    public static double CursorScale { get; set; } = 1;
    public static bool CursorTrail { get; set; } = false;
    public static double TrailTime { get; set; } = 0.05;
    public static double TrailDetail { get; set; } = 1;
    public static bool CursorDrift { get; set; } = true;
    public static double VideoDim { get; set; } = 80;
    public static double VideoRenderScale { get; set; } = 100;
    public static bool SimpleHUD { get; set; } = false;
    public static string Space { get; set; } = "skin";
    public static bool AbsoluteInput { get; set; } = false;
    public static bool RecordReplays { get; set; } = true;
    public static bool HitPopups { get; set; } = true;
    public static bool MissPopups { get; set; } = true;
    public static double FPS { get; set; } = 240;
    public static bool UnlockFPS { get; set; } = true;

    public static void Save(string profile = null)
    {
        profile ??= Phoenyx.Util.GetProfile();

        Dictionary data = new()
        {
            ["_Version"] = 1
        };

        foreach (PropertyInfo property in typeof(SettingsProfile).GetProperties())
        {
            data[property.Name] = (Variant)typeof(Variant).GetMethod("From").MakeGenericMethod(property.GetValue(null).GetType()).Invoke(null, [property.GetValue(null)]);
        }

        File.WriteAllText($"{Constants.USER_FOLDER}/profiles/{profile}.json", Json.Stringify(data, "\t"));
        Phoenyx.Skin.Save();
        Logger.Log($"Saved settings {profile}");
    }

    public static void Load(string profile = null)
    {
        profile ??= Phoenyx.Util.GetProfile();

        Exception err = null;

        try
        {
            Godot.FileAccess file = Godot.FileAccess.Open($"{Constants.USER_FOLDER}/profiles/{profile}.json", Godot.FileAccess.ModeFlags.Read);
            Dictionary data = (Dictionary)Json.ParseString(file.GetAsText());

            file.Close();

            foreach (PropertyInfo property in typeof(SettingsProfile).GetProperties())
            {
                if (data.ContainsKey(property.Name))
                {
                    property.SetValue(null, data[property.Name].GetType().GetMethod("As", BindingFlags.Public | BindingFlags.Instance).MakeGenericMethod(property.GetValue(null).GetType()).Invoke(data[property.Name], null));
                }
            }

            if (Fullscreen)
            {
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
            }

            ToastNotification.Notify($"Loaded profile [{profile}]");
        }
        catch (Exception exception)
        {
            err = exception;
        }

        if (!Directory.Exists($"{Constants.USER_FOLDER}/skins/{Skin}"))
        {
            Skin = "default";
            ToastNotification.Notify($"Could not find skin {Skin}", 1);
        }

        Phoenyx.Skin.Load();

        if (err != null)
        {
            ToastNotification.Notify("Settings file corrupted", 2);
            throw Logger.Error($"Settings file corrupted; {err}");
        }

        Logger.Log($"Loaded settings {profile}");
    }
}
