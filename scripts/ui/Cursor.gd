extends TextureRect

#var SkinProfile = load("res://scripts/skinning/SkinProfile.cs")
#var SettingsManager = load("res://scripts/SettingsManager.cs")

var mousePosition := Vector2.ZERO

func _ready() -> void:
	#SkinProfile.OnLoaded.connect(UpdateTexture)
	
	UpdateTexture()
	UpdateSize()

func _process(_delta: float) -> void:
	position = mousePosition - size / 2

func _input(event: InputEvent) -> void:
	if event is InputEventMouseMotion:
		mousePosition = event.position

func UpdateTexture() -> void:
	#texture = SkinProfile.CursorImage
	pass

func UpdateSize() -> void:
	pass
