using Godot;
using Soviet.Soviet.Manager;

namespace Soviet.Soviet.CameraNode
{
	public class RTS : Spatial
	{
		//Child
		private ClippedCamera camera;
		private bool canRotate;

		private Spatial elevation;

		//rotate
		[Export] public bool InvertY;
		private Vector2 lastPosMouse;
		[Export] private int maxElevation;
		[Export] private int maxZoom;

		[Export] private int minElevation;

		//zoom
		[Export] private int minZoom;

		[Export] private int rotationSpeed;

		//WASD
		[Export] private Vector3 speedMovement;
		[Export] private int speedZoom;
		private int zoomDirection;

		public override void _Ready()
		{
			elevation = GetChild<Spatial>(0);
			camera = elevation.GetChild<ClippedCamera>(0);
		}

		public override void _Process(float delta)
		{
			MoveWasd(delta);
			Rotate(delta);
			Zoom(delta);
		}

		private void MoveWasd(float delta)
		{
			var velocity = new Vector3();
			if (Input.IsActionPressed("move_up")) velocity -= Transform.basis.z;
			if (Input.IsActionPressed("move_down")) velocity += Transform.basis.z;
			if (Input.IsActionPressed("move_left")) velocity -= Transform.basis.x;
			if (Input.IsActionPressed("move_right")) velocity += Transform.basis.x;
			velocity = velocity.Normalized();
			Translation += velocity * delta * speedMovement;
		}

		private void Rotate(float delta)
		{
			if (!canRotate) return;
			var displacement = GetMousePosition();
			RotateLeftRight(delta, displacement.x);
			RotateUpDown(delta, displacement.y);
			Zoom(delta);
		}

		private Vector2 GetMousePosition()
		{
			var currentPosMouse = GetViewport().GetMousePosition();
			var displacement = currentPosMouse - lastPosMouse;
			lastPosMouse = currentPosMouse;
			return displacement;
		}

		private void RotateLeftRight(float delta, float x)
		{
			var rotationDegrees = RotationDegrees;
			rotationDegrees.y = x * delta * rotationSpeed;
			RotationDegrees += rotationDegrees;
		}

		private void RotateUpDown(float delta, float y)
		{
			var newRotation = elevation.RotationDegrees.x;
			if (InvertY)
				newRotation += y * delta * rotationSpeed;
			else
				newRotation -= y * delta * rotationSpeed;
			var clamped = Mathf.Clamp(newRotation, minElevation, maxElevation);
			var vector3 = new Vector3();
			vector3.x = clamped;
			elevation.RotationDegrees = vector3;
		}

		private void Zoom(float delta)
		{
			var newZoom = Mathf.Clamp(camera.Translation.z + speedZoom * delta * zoomDirection, minZoom, maxZoom);
			var zoom = camera.Translation;
			zoom.z = newZoom;
			camera.Translation = zoom;
			zoomDirection = 0;
		}


		public override void _Input(InputEvent @event)
		{
			if (@event.IsActionPressed("cam_pan"))
			{
				canRotate = true;
				lastPosMouse = GetViewport().GetMousePosition();
			}

			if (@event.IsActionReleased("cam_pan")) canRotate = false;
			if (@event.IsActionPressed("cam_zoom_out")) zoomDirection = 1;
			if (@event.IsActionPressed("cam_zoom_in")) zoomDirection = -1;
			if (!(@event is InputEventMouseButton eventMouseButton) || !eventMouseButton.Pressed ||
			    eventMouseButton.ButtonIndex != 1) return;
			var position = TileManager.Instance.GetCoordinatesGrid(camera);
			TileManager.Instance.CreateTileAt((int)TileConstTree.TreeTwo, position, (int)GridMapLayer.Tree);
		}
	}
}