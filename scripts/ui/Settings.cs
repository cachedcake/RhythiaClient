using Godot;
using System;
using System.Collections.Generic;

public partial class Settings : ColorRect
{
    public bool Shown = false;

    private Dictionary<string, Panel> settingPanels = [];
    private Button deselect;
    private Panel holder;
    private VBoxContainer sidebar;
    private Panel categories;
    private ScrollContainer selectedCategory;

    public override void _Ready()
    {
        deselect = GetNode<Button>("Deselect");
        holder = GetNode<Panel>("Holder");
        sidebar = holder.GetNode("Sidebar").GetNode<VBoxContainer>("Container");
        categories = holder.GetNode<Panel>("Categories");
        selectedCategory = categories.GetNode<ScrollContainer>("Gameplay");

        SettingsManager.Instance.OnShown += ShowMenu;
        SettingsManager.Instance.Settings.FieldUpdated += (string field, Variant value) => {
            Panel panel = settingPanels[field];

            HSlider slider = (HSlider)panel?.FindChild("HSlider", false);
            LineEdit lineEdit = (LineEdit)panel?.FindChild("LineEdit", false);
            CheckButton toggle = (CheckButton)panel?.FindChild("CheckButton", false);

            if (slider != null)
			{
				updateSlider(slider, lineEdit, (double)value);
			}
			else if (toggle != null)
			{
				updateToggle(toggle, (bool)value);
			}
        };

        ShowMenu(false);

        deselect.Pressed += () => { SettingsManager.ShowMenu(false); };
		
		foreach (ColorRect buttonHolder in sidebar.GetChildren())
		{
            ScrollContainer category = (ScrollContainer)categories.FindChild(buttonHolder.Name, false);

			if (category != null)
			{
                buttonHolder.GetNode<Button>("Button").Pressed += () => { SelectCategory(category); };
            }
        }

		foreach (ScrollContainer category in categories.GetChildren())
		{
			foreach (Panel panel in category.GetNode("Container").GetChildren())
			{
                string field = panel.Name;

                settingPanels[field] = panel;

                Variant value = SettingsManager.Instance.Settings.Get(field);

                HSlider slider = (HSlider)panel.FindChild("HSlider", false);
				LineEdit lineEdit = (LineEdit)panel.FindChild("LineEdit", false);
				CheckButton toggle = (CheckButton)panel.FindChild("CheckButton", false);

				if (slider != null)
				{
                    setupSlider(field, slider, lineEdit);
                    updateSlider(slider, lineEdit, (double)value);
                }
				else if (toggle != null)
				{
                    setupToggle(field, toggle);
                    updateToggle(toggle, (bool)value);
                }
            }
		}
    }

	public void ShowMenu(bool show)
	{
        Shown = show;
        deselect.MouseFilter = show ? MouseFilterEnum.Stop : MouseFilterEnum.Ignore;

        MoveToFront();

		if (Shown)
		{
            Visible = true;
            holder.OffsetTop = 25;
            holder.OffsetBottom = 25;
        }

        Tween tween = CreateTween().SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out).SetParallel();
        tween.TweenProperty(this, "modulate", Color.Color8(255, 255, 255, (byte)(Shown ? 255 : 0b0)), 0.25);
        tween.TweenProperty(holder, "offset_top", Shown ? 0 : 25, 0.25);
		tween.TweenProperty(holder, "offset_bottom", Shown ? 0 : 25, 0.25);
        tween.Chain().TweenCallback(Callable.From(() => { Visible = Shown; }));
    }

	public void SelectCategory(ScrollContainer category)
	{
        sidebar.GetNode<ColorRect>(new(selectedCategory.Name)).Color = Color.Color8(255, 255, 255, 0);
        selectedCategory.Visible = false;

        selectedCategory = category;

        selectedCategory.Visible = true;
		sidebar.GetNode<ColorRect>(new(selectedCategory.Name)).Color = Color.Color8(255, 255, 255, 8);
    }

	private void setupSlider(string field, HSlider slider, LineEdit lineEdit)
	{
		void applyLineEdit() {
            double value = (lineEdit.Text == "" ? lineEdit.PlaceholderText : lineEdit.Text).ToFloat();
            SettingsManager.ApplySetting(field, value);
        }

        lineEdit.FocusExited += applyLineEdit;
        lineEdit.TextSubmitted += (_) => { applyLineEdit(); };
        slider.ValueChanged += (double value) => { SettingsManager.ApplySetting(field, value); };
    }

	private void updateSlider(HSlider slider, LineEdit lineEdit, double value)
	{
        lineEdit.Text = value.ToString();
        lineEdit.ReleaseFocus();
        slider.SetValueNoSignal(value);
    }

	private void setupToggle(string field, CheckButton button)
	{
        button.Toggled += (bool value) => { SettingsManager.ApplySetting(field, value); };
    }

	private void updateToggle(CheckButton button, bool value)
	{
        button.ButtonPressed = value;
    }
}