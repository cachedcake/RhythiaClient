using Godot;

public partial class SceneManager : Node
{
    private static Node Node;
    private static bool SkipNextTransition = false;

    // TODO: Pre-loading scenes and swapping them in runtime

    //public static LegacyRunner GameScene = (LegacyRunner)ResourceLoader.Load<PackedScene>("res://scenes/game.tscn").Instantiate();
    //public static Node LoadingScene = ResourceLoader.Load<PackedScene>("res://scenes/loading.tscn").Instantiate();
    //public static Node Menu = ResourceLoader.Load<PackedScene>("res://scenes/main_menu.tscn").Instantiate();

    public static Node Scene;

    public override void _Ready()
    {
        Node = this;

        Node.GetTree().Connect("node_added", Callable.From((Node child) =>
        {
            if (child.Name != "SceneMenu" && child.Name != "SceneGame" && child.Name != "SceneResults")
            {
                return;
            }

            Scene = child;

            if (SkipNextTransition)
            {
                SkipNextTransition = false;
                return;
            }

            ColorRect inTransition = Scene.GetNode<ColorRect>("Transition");
            inTransition.SelfModulate = Color.FromHtml("ffffffff");
            Tween inTween = inTransition.CreateTween();
            inTween.TweenProperty(inTransition, "self_modulate", Color.FromHtml("ffffff00"), 0.25).SetTrans(Tween.TransitionType.Quad);
            inTween.Play();
        }));
    }

    public static void Load(string path, bool skipTransition = false)
    {
        if (skipTransition)
        {
            SkipNextTransition = true;
            Node.GetTree().ChangeSceneToFile(path);
        }
        else
        {
            ColorRect outTransition = Scene.GetNode<ColorRect>("Transition");
            Tween outTween = outTransition.CreateTween();
            outTween.TweenProperty(outTransition, "self_modulate", Color.FromHtml("ffffffff"), 0.25).SetTrans(Tween.TransitionType.Quad);
            outTween.TweenCallback(Callable.From(() =>
            {
                Node.GetTree().ChangeSceneToFile(path);
            }));
            outTween.Play();
        }
    }
}
