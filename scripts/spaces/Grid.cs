using Godot;
using System;

namespace Space;

public partial class Grid : Node3D
{
    public Color Color = Color.Color8(255, 255, 255, 255);

    private StandardMaterial3D tileMaterial;
    private WorldEnvironment environment;

    public override void _Ready()
    {
        tileMaterial = (GetNode<MeshInstance3D>("Top").Mesh as PlaneMesh).Material as StandardMaterial3D;
        environment = GetNode<WorldEnvironment>("WorldEnvironment");
    }

    public override void _Process(double delta)
    {
        Color = Color.Lerp(LegacyRunner.CurrentAttempt.LastHitColour, (float)delta * 8);

        tileMaterial.AlbedoColor = Color;
        tileMaterial.Uv1Offset += Vector3.Up * (float)delta * 3;
    }
}