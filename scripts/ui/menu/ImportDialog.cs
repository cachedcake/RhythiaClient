using Godot;
using System;

public partial class ImportDialog : FileDialog
{
    public override void _Ready()
    {
        FilesSelected += OnFilesSelected;
    }

	public void OnFilesSelected(string[] paths)
	{
        MapParser.BulkImport(paths);
    }
}