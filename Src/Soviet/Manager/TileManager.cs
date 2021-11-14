using Godot;
using Soviet.Soviet.Debug;

namespace Soviet.Soviet.Manager
{
	public class TileManager : Node
	{
		private static TileManager instance;
		public static TileManager Instance => instance;

		private GridMap floor;
		private GridMap tree;
		private Control debug;
		
		public override void _Ready()
		{
			instance = this;
			floor = GetTree().CurrentScene.FindNode("Floor") as GridMap;
			tree = GetTree().CurrentScene.FindNode("Trees") as GridMap;
			debug = GetTree().CurrentScene.FindNode("DebugNode") as Control;
		}
		
		public void CreateTileAt(int tile, Vector3 pos, int layer)
		{
			if ((GridMapLayer) layer == GridMapLayer.Floor)
			{
				floor.SetCellItem((int)pos.x, (int)pos.y, (int)pos.z, tile);
			}
			else
			{
				tree.SetCellItem((int)pos.x, (int)pos.y, (int)pos.z, tile);
			}

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
		Cabezon,
		Seco,
		Pine
	}

	public enum GridMapLayer
	{
		Floor,
		Tree
	}
}