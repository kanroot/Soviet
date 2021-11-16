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

		public Vector3 GetPositionCell(Vector3 to)
		{
			return floor.MapToWorld((int)to.x, (int)to.y, (int)to.z);
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