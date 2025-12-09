using Godot;
using System;

public partial class Cursor : TextureRect
{
    public override void _Ready()
    {
        SkinManager.Instance.OnLoaded += UpdateTexture;
        // SettingsManager.Instance.Settings.FieldUpdated += (field, value) => { if (field == "CursorScale") { UpdateSize(); } };

        UpdateTexture();
        UpdateSize();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
		{
            Position = mouseMotion.Position - Size / 2;
        }
    }

	public void UpdateTexture()
	{
        Texture = SkinManager.Instance.Skin.CursorImage;
    }

	public void UpdateSize()
	{
        Size = Vector2.One * 32 * (float)SettingsManager.Instance.Settings.CursorScale;
    }
}