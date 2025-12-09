using Godot;
using System;

public partial class Background : Panel
{
    private TextureRect tile;
    private ShaderMaterial tileMaterial;

    public override void _Ready()
    {
        tile = GetNode<TextureRect>("Tile");
        tileMaterial = tile.Material as ShaderMaterial;

        SkinManager.Instance.OnLoaded += updateSkin;

        updateSkin();
    }

	private void updateSkin()
	{
        tile.Texture = SkinManager.Instance.Skin.BackgroundTileImage;
        tileMaterial.Shader = SkinManager.Instance.Skin.BackgroundTileShader;
    }
}