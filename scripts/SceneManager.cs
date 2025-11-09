using Godot;
using Godot.Collections;
using Menu;

public enum SceneType
{
    Loading,
    Menu,
    Game,
    Results
}

public partial class SceneManager : Node
{
    public static Node Node { get; private set; }

    private static bool skipNextTransition = false;

    public static Node ActiveScene;
    private static string activeScenePath;

    private static SubViewportContainer backgroundContainer;
    private static SubViewport backgroundViewport;

    private static Dictionary<string, Node> scenes;

    public static Node Scene;

    public override void _Ready()
    {
        if (Name != "Main")
        {
            return;
        }

        Node = this;
        backgroundContainer = Node.GetNode<SubViewportContainer>("Background");

        backgroundViewport = backgroundContainer.GetNode<SubViewport>("SubViewport");

        Load("res://scenes/loading.tscn", true);

        //Node.GetTree().Connect("node_added", Callable.From((Node child) =>
        //{
        //    if (child.Name != "SceneMenu" && child.Name != "SceneGame" && child.Name != "SceneResults")
        //    {
        //        return;
        //    }

        //    if (skipNextTransition)
        //    {
        //        skipNextTransition = false;
        //        return;
        //    }

        //    ColorRect inTransition = ActiveScene.GetNode<ColorRect>("Transition");
        //    inTransition.SelfModulate = Color.FromHtml("ffffffff");
        //    Tween inTween = inTransition.CreateTween();
        //    inTween.TweenProperty(inTransition, "self_modulate", Color.FromHtml("ffffff00"), 0.25).SetTrans(Tween.TransitionType.Quad);
        //    inTween.Play();
        //}));
    }

    public static void ReloadCurrentScene()
    {
        Load(activeScenePath);
    }

    private static void registerScene(Node node)
    {
        if (!scenes.ContainsKey(node.Name))
            scenes[node.Name] = node;
    }

    public static void Setup()
    {

    }

    public static void Load(string path, bool skipTransition = false)
    {

        if (skipTransition)
        {
            skipNextTransition = true;
            swapScene(path);
        }
        else
        {
            ColorRect outTransition = ActiveScene.GetNode<ColorRect>("Transition");
            Tween outTween = outTransition.CreateTween();
            outTween.TweenProperty(outTransition, "self_modulate", Color.FromHtml("ffffffff"), 0.25).SetTrans(Tween.TransitionType.Quad);
            outTween.TweenCallback(Callable.From(() =>
            {
                swapScene(path);
            }));
            outTween.Play();
        }
    }

    private static void swapScene(string path)
    {
        if (ActiveScene != null && ActiveScene.GetParent() != null)
        {
            Node.RemoveChild(ActiveScene);
        }

        var node = ResourceLoader.Load<PackedScene>(path).Instantiate();

        activeScenePath = path;
        ActiveScene = node;
        Node.AddChild(node);
    }

    public static void ALoad(string path, bool skipTransition = false)
    {

    }
}
