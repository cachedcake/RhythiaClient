using Godot;
using System;

namespace Space;

public partial class Waves : Node3D
{
    private Godot.Environment environment;
    private ShaderMaterial skyMaterial;
    private ShaderMaterial waterMaterial;
    private Camera3D camera;

    public override void _Ready()
    {
        environment = GetNode<WorldEnvironment>("WorldEnvironment").Environment;
        skyMaterial = environment.Sky.SkyMaterial as ShaderMaterial;
        waterMaterial = (GetNode<MeshInstance3D>("Water").Mesh as PlaneMesh).Material as ShaderMaterial;
        camera = GetNode<Camera3D>("Camera3D");

        skyMaterial.SetShaderParameter("coverage", 0);
        camera.Rotation = Vector3.Zero;
        camera.Fov = 90;

        Tween echoTween = CreateTween().SetTrans(Tween.TransitionType.Linear);
        echoTween.TweenMethod(Callable.From((float echo) => { waterMaterial.SetShaderParameter("echo", echo); }), 0.0, 0.5, 12);

        Tween introTween = CreateTween().SetTrans(Tween.TransitionType.Quart).SetEase(Tween.EaseType.Out).SetParallel();
        introTween.TweenProperty(camera, "rotation", Vector3.Right * Mathf.DegToRad(15), 5);
        introTween.TweenProperty(camera, "fov", 70, 5);
        introTween.SetTrans(Tween.TransitionType.Linear);
        introTween.TweenMethod(Callable.From((float coverage) => { skyMaterial.SetShaderParameter("coverage", coverage); }), 0.0, 1.0, 8);
    }

    public override void _Process(double delta)
    {
        Viewport viewport = GetViewport();
        Vector2 centerOffset = viewport.GetMousePosition() - viewport.GetVisibleRect().Size / 2;

        environment.SkyRotation += Vector3.Up * (float)delta * centerOffset.X / 50000;
    }
}