extends FileDialog

var MapParser = load("res://scripts/map/MapParser.cs")

func _files_selected(paths: PackedStringArray) -> void:
	MapParser.BulkImport(paths)
