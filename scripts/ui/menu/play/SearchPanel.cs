using Godot;
using System;

public partial class SearchPanel : Panel
{
    [Export]
    public bool SearchAuthor = false;

    private LineEdit lineEdit;

    public override void _Ready()
    {
        lineEdit = GetNode<LineEdit>("LineEdit");

        lineEdit.TextChanged += (text) => {
            MapList.Instance.Search(SearchAuthor ? null : text, SearchAuthor ? text : null);
        };
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey eventKey && eventKey.Pressed && !eventKey.CtrlPressed && !eventKey.AltPressed)
        {
            if (GetViewport().GuiGetFocusOwner() == null)
            {
                lineEdit.GrabFocus();
            }
        }
        else if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed)
        {
            lineEdit.ReleaseFocus();
        }
    }
}
