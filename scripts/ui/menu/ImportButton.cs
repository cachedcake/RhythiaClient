using Godot;
using System;

public partial class ImportButton : Button
{
    private FileDialog importDialog;

    public override void _Ready()
    {
        importDialog = GetParent().GetParent().GetParent().GetParent().GetParent().GetNode<FileDialog>("ImportDialog");
    }

    public override void _Pressed()
    {
        importDialog.Show();
    }
}