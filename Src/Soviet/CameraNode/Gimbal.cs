using Godot;
using Godot.Collections;
using Soviet.Soviet.Manager;

namespace Soviet.Soviet.CameraNode
{
	public class Gimbal : Spatial
	{
		[Export] private readonly Vector3 speed = new Vector3(0, 0, 0);
		private ClippedCamera camera;
		private Spatial elevation;
		private int rayLenght = 1000;
	

	public override void _Ready()
		{
			elevation = FindNode("Elevation") as Spatial;
			camera = elevation?.FindNode("ClippedCamera") as ClippedCamera;
			
		}

		public override void _Process(float delta)
		{
			GetInputKeyboard(delta);
			RotateCameraGimbal(delta);
			ZoomKeyboard(delta);
			ZoomMouse(delta);
			
		}

		private void GetInputKeyboard(float delta)
		{
			var velocity = new Vector3(0, 0, 0);
			if (Input.IsActionPressed("ui_left")) velocity -= Transform.basis.x;
			if (Input.IsActionPressed("ui_right")) velocity += Transform.basis.x;
			if (Input.IsActionPressed("ui_up")) velocity -= Transform.basis.z;
			if (Input.IsActionPressed("ui_down")) velocity += Transform.basis.z;
			velocity = velocity.Normalized();
			Translation += velocity * delta * speed;
		}

		private void RotateCameraGimbal(float delta)
		{
			var velocity = new Vector3(0, 0, 0);
			if (Input.IsActionPressed("rotate_left")) velocity += Transform.basis.y;
			if (Input.IsActionPressed("rotate_right")) velocity -= Transform.basis.y;
			Rotation += velocity * delta * speed;
		}

		private void ZoomKeyboard(float delta)
		{
			var velocity = new Vector3(0, 0, 0);
			if (Input.IsActionPressed("cam_zoom_in")) velocity -= elevation.Transform.basis.y;
			if (Input.IsActionPressed("cam_zoom_out")) velocity += elevation.Transform.basis.y;
			if (Input.IsActionJustReleased("cam_zoom_in")) velocity -= elevation.Transform.basis.y;
			if (Input.IsActionJustReleased("cam_zoom_out")) velocity += elevation.Transform.basis.y;
			elevation.Translation += velocity * delta * speed;
		}

		private void ZoomMouse(float delta)
		{
			var velocity = new Vector3(0, 0, 0);
			if (Input.IsActionJustReleased("cam_zoom_in")) velocity -= elevation.Transform.basis.y;
			if (Input.IsActionJustReleased("cam_zoom_out")) velocity += elevation.Transform.basis.y;
			var localSpeedMouse = speed;
			localSpeedMouse.y += 150;
			elevation.Translation += velocity * delta * localSpeedMouse;
		}

		public override void _Input(InputEvent @event)
		{
			if (!(@event is InputEventMouseButton eventMouseButton) || !eventMouseButton.Pressed ||
			    eventMouseButton.ButtonIndex != 1) return;
			var mousePos = eventMouseButton.Position;
			var projectPos = camera.ProjectRayOrigin(mousePos);
			var to = projectPos + camera.ProjectRayNormal(mousePos) * rayLenght;
			var space = GetWorld().DirectSpaceState;
			var realPos = space.IntersectRay(projectPos, to, new Array(), 1);
			Vector3 position = (Vector3) realPos["position"];
			position.y = 0;
			position /= 2;
			TileManager.Instance.CreateTileAt((int) TileConstFloor.Grass, position , (int) GridMapLayer.Floor);
		}
	}
}