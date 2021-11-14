using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Soviet.Soviet.Scenes
{
	public class Floor : GridMap
	{
		private readonly MeshInstance grass = new MeshInstance();
		private readonly MeshInstance ground = new MeshInstance();
		private readonly MeshInstance river = new MeshInstance();
		[Export] private int height;
		[Export] private int lenght;

		public override void _Ready()
		{
			CreateGrid();
		}
		
		private void CreateGrid()
		{
			var startPosition = new Vector3(0, 0, 0);
			for (var i = 0; i < lenght; i++)
			{
				SetCellItem((int)startPosition.x + i, (int)startPosition.y, (int)startPosition.z, 0);
				for (var j = 1; j < height; j++) SetCellItem(i, (int)startPosition.y, (int)startPosition.z + j, 1);
				
			}
		}
	}
}