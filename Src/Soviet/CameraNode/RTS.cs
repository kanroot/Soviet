using Godot;
using Soviet.Soviet.Manager;

namespace Soviet.Soviet.CameraNode
{
	public class RTS : Spatial
	{
		[Export] private Vector3 speedMovement;
		[Export] private int minElevation;
		[Export] private int maxElevation;
		[Export] private int rotationSpeed;
		[Export] public bool InvertY;
		private bool canRotate;
		private Vector2 lastPosMouse;
		private ClippedCamera camera;
		private Spatial elevation;
		public override void _Ready()
		{
			elevation = GetChild<Spatial>(0);
			camera = elevation.GetChild<ClippedCamera>(0);
		}

		public override void _Process(float delta)
		{
			MoveWasd(delta);
			Rotate(delta);
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
			GD.Print($"X: {x}");
			var rotationDegrees = RotationDegrees;
			rotationDegrees.y = x * delta * rotationSpeed;
			RotationDegrees += rotationDegrees;
		}

		private void RotateUpDown(float delta, float y)
		{
			var newRotation = elevation.RotationDegrees.x;
			if (InvertY)
			{
				newRotation += (y * delta * rotationSpeed);
			}
			else
			{
				newRotation -= (y * delta * rotationSpeed);
			}
			
			var clamped = Mathf.Clamp(newRotation, minElevation, maxElevation);
			var vector3 = new Vector3();
			vector3.x = clamped;
			elevation.RotationDegrees = vector3;
		}

	

		public override void _Input(InputEvent @event)
		{
			if (@event.IsActionPressed("cam_pan"))
			{
				canRotate = true;
				lastPosMouse = GetViewport().GetMousePosition();
			}
			if (@event.IsActionReleased("cam_pan"))
			{
				canRotate = false;
			}
			if (!(@event is InputEventMouseButton eventMouseButton) || !eventMouseButton.Pressed ||
			    eventMouseButton.ButtonIndex != 1) return;
			var position = TileManager.Instance.GetCoordinatesGrid(camera);
			TileManager.Instance.CreateTileAt((int)TileConstTree.TreeTwo, position, (int)GridMapLayer.Tree);
		}
	}
}