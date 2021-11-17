using System;
using Godot;
using Soviet.Soviet.Manager;
using Soviet.Soviet.Utils;

namespace Soviet.Soviet.CameraNode
{
	public class Gimbal : Spatial
	{
		[Export] private readonly Vector3 speed = new Vector3(0, 0, 0);
		private ClippedCamera camera;
		private Spatial innerGimbal;


		public override void _Ready()
		{
			innerGimbal = this.GetChildFromType<Spatial>();
			camera = innerGimbal?.GetChildFromType<ClippedCamera>();
		}

		public override void _Process(float delta)
		{
			GetInputKeyboard(delta);
			RotateInnerGimbal(delta);
			Zoom(delta);
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

		private void RotateInnerGimbal(float delta)
		{
			var velocity = new Vector3(0, 0, 0);
			if (Input.IsActionPressed("rotate_left")) velocity += innerGimbal.Transform.basis.y;
			if (Input.IsActionPressed("rotate_right")) velocity -= innerGimbal.Transform.basis.y;
			innerGimbal.Rotation += velocity * delta * speed;
		}

		private void Zoom(float delta)
		{
			if (Translation.y > 25 || Translation.y < 10) return;
			var velocity = new Vector3(0, 0, 0);
			if (Input.IsActionPressed("cam_zoom_in")) velocity -= Transform.basis.y;
			if (Input.IsActionPressed("cam_zoom_out")) velocity += Transform.basis.y;
			if (Input.IsActionJustReleased("cam_zoom_in")) velocity -= Transform.basis.y;
			if (Input.IsActionJustReleased("cam_zoom_out")) velocity += Transform.basis.y;
			if (Input.IsActionJustReleased("cam_zoom_in")) velocity -= Transform.basis.y;
			if (Input.IsActionJustReleased("cam_zoom_out")) velocity += Transform.basis.y;
			velocity = velocity * speed * delta;
			Translation += velocity;
			Translation = new Vector3(Translation.x, Mathf.Clamp(Translation.y, 10, 25), Translation.z);
		}
		
		
		public override void _Input(InputEvent @event)
		{
			if (!(@event is InputEventMouseButton eventMouseButton) || !eventMouseButton.Pressed ||
			    eventMouseButton.ButtonIndex != 1) return;
			var position = TileManager.Instance.GetCoordinatesGrid(camera);
			TileManager.Instance.CreateTileAt((int)TileConstTree.TreeTwo, position, (int)GridMapLayer.Tree);
		}
	}
}