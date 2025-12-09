using Godot;
using System;

public partial class MapButton : Panel
{
	/// <summary>
	/// Parsed map ID
	/// </summary>
    public string Map = "";

	/// <summary>
	/// Index within the full map list
	/// </summary>
    public int ListIndex = 0;

	/// <summary>
	/// Minimum Y size (configure in MapList properties)
	/// </summary>
    public float SizeHeight = 90;

	/// <summary>
	/// Additional Y size when hovered (configure in MapList properties)
	/// </summary>
    public float HoverSizeOffset = 10;

	/// <summary>
	/// Additional Y size when selected (configure in MapList properties)
	/// </summary>
    public float SelectedSizeOffset = 20;

	/// <summary>
	/// Total Y size added on top of minimum size, equivalent to HoverSizeOffset + SelectedSizeOffset
	/// </summary>
    public float SizeOffset = 0;

	/// <summary>
	/// Normalized distance from MapList center
	/// </summary>
    public float CenterOffset = 0;

	/// <summary>
	/// Horizontal anchor offset when selected
	/// </summary>
    public float StickoutOffset = 0;

    public float TargetOutlineFill = 0;
    public float OutlineFill = 0;
    public bool Hovered = false;
    public bool Selected = false;

	[Signal]
    public delegate void OnPressedEventHandler();

    public ShaderMaterial OutlineShader;

    private Label title;
    private RichTextLabel extra;
    private TextureRect cover;
    private Button button;

    public override void _Ready()
    {
        button = GetNode<Button>("Button");
        title = GetNode<Label>("Title");
        extra = GetNode<RichTextLabel>("Extra");
        cover = GetNode<TextureRect>("Cover");

        Panel outline = GetNode<Panel>("Outline");

        OutlineShader = (ShaderMaterial)outline.Material.Duplicate();
        outline.Material = OutlineShader;

        button.MouseEntered += () => { Hover(true); };
		button.MouseExited += () => { Hover(false); };
		button.Pressed += () => {
			Select();
            EmitSignal(SignalName.OnPressed);
        };
    }

    public override void _Process(double delta)
    {
        float smoothCenterOffset = (float)Math.Cos(Math.PI * CenterOffset / 2);
		
        StickoutOffset = (float)Mathf.Lerp(StickoutOffset, Selected ? 0.05 : 0, Math.Min(1, 16 * delta));
        AnchorLeft = (float)(0.1 - smoothCenterOffset / 20 - StickoutOffset);
        Size = new(Size.X, (float)Mathf.Lerp(Size.Y, SizeHeight + SizeOffset, Math.Min(1, 16 * delta)));
        OutlineFill = (float)Mathf.Lerp(OutlineFill, TargetOutlineFill, Math.Min(1, 10 * delta));

        OutlineShader.SetShaderParameter("fill", OutlineFill);
    }

	public void Hover(bool hover)
	{
        Hovered = hover;
        SizeOffset = computeSizeOffset();
		
        CreateTween().SetTrans(Tween.TransitionType.Quad).TweenProperty(this, "self_modulate", Hovered ? Color.Color8(26, 6, 13, 224) : Color.Color8(0, 0, 0, 224), 0.15);
    }

	public void Select(bool select = true)
	{
		if (Selected && select)
		{
            GD.Print($"play map {Map}");
        }

        Selected = select;
		SizeOffset = computeSizeOffset();

        CreateTween().SetTrans(Tween.TransitionType.Quad).TweenProperty(cover, "modulate", Color.Color8(255, 255, 255, (byte)(Selected ? 255 : 128)), 0.1);
    }

	public void Deselect()
	{
        Select(false);
    }

	public void UpdateInfo(string map)
	{
        Map = map;
        Name = Map;
    }

	public void UpdateOutline(float targetFill, float fill = -1)
	{
        TargetOutlineFill = targetFill;

		if (fill != -1)
		{
            OutlineFill = fill;
        }
    }

	private float computeSizeOffset()
	{
		return (Hovered ? HoverSizeOffset : 0) + (Selected ? SelectedSizeOffset : 0);
	}
}