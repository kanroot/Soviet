using Godot;
using Soviet.Soviet.Debug;

namespace Soviet.Soviet.Manager
{
	public class TileManager : Node
	{
		private Control debug;
		private GridMap floor;
		private GridMap tree;
		public static TileManager Instance { get; private set; }

		public Vector3 GetCoordinatesGrid(ClippedCamera camera, int rayLenght = 10000)
		{
			var mousePos = GetViewport().GetMousePosition();
			var from = camera.ProjectRayOrigin(mousePos);
			var to = @from + camera.ProjectRayNormal(mousePos) * rayLenght;
			var plane = new Plane(Vector3.Up, 0);
			var position = plane.IntersectRay(@from, to);
			if (position is null)
			{
				return new Vector3();
			}
			var gridPos=  tree.WorldToMap( ((Vector3) position).Floor());
			gridPos.y = 0;
			return (Vector3) gridPos;
		}

		public override void _Ready()
		{
			Instance = this;
			floor = GetTree().CurrentScene.FindNode("Floor") as GridMap;
			tree = GetTree().CurrentScene.FindNode("Objects") as GridMap;
			debug = GetTree().CurrentScene.FindNode("DebugNode") as Control;
		}

		public void CreateTileAt(int tile, Vector3 pos, int layer)
		{
			if ((GridMapLayer)layer == GridMapLayer.Floor)
				floor.SetCellItem((int)pos.x, (int)pos.y, (int)pos.z, tile);
			else
				tree.SetCellItem((int)pos.x, (int)pos.y, (int)pos.z, tile);

			var a = debug as DebugNode;
			a?.GetNodes();
		}
		
	}


	public enum TileConstFloor
	{
		Grass,
		River,
		Ground
	}

	public enum TileConstTree
	{
		TreeOne,
		TreeTwo,
		TreeTallOne,
		TreeTallTwo,
		TreeThinOne,
		TreeThinTwo
	}

	public enum GridMapLayer
	{
		Floor,
		Tree
	}
}