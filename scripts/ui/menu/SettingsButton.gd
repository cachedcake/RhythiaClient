extends Button

var SettingsManager = load("res://scripts/SettingsManager.cs")

func _pressed() -> void:
	SettingsManager.ShowSettings(true)
