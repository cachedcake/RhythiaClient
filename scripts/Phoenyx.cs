using Godot;

public partial class Phoenyx : Node
{
    private static bool initialized = false;

    // TODO: Initializing the game should all be done here
    public override void _Ready()
    {
        //SettingsManager.Load();

        //var settings = SettingsManager.Settings;

        //if (initialized)
        //{
        //    return;
        //}

        //initialized = true;
    }

    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
        {
            if (SceneManager.Scene.Name == "SceneGame")
            {
                Stats.RageQuits++;
            }

            Util.Quit();
        }
    }
}
