using Godot;
using Godot.Collections;

namespace Soviet.Soviet.Debug
{
	public class DebugNode : Control
	{
		private Spatial cameraElevation;
		private Spatial cameraGimball;
		[Export] private NodePath cameraPath;
		private Label cameraPos;
		private Array countCells;
		private Label elevation;
		private GridMap floor;
		[Export] private NodePath floorPath;
		private int grassCount;
		private int groundCount;
		private int riverCount;
		private Label textCountCells;
		private Label textgrassCount;
		private Label textRiverCount;
		private Label textGroundCount;
		private VBoxContainer vBoxContainer;

		public override void _Ready()
		{
			cameraGimball = GetNode<Spatial>(cameraPath);
			floor = GetNode<GridMap>(floorPath);
			GetNodes();
		}

		public override void _Process(float delta)
		{
			var x = cameraGimball.Translation.x;
			var z = cameraGimball.Translation.z;
			cameraPos.Text = $"Posicion:({x.ToString()} , {z.ToString()})";
			elevation.Text = $"Elevacion: {cameraElevation.Translation.y.ToString()}";
			textCountCells.Text = $"Cuenta de celdas: {countCells.Count.ToString()}";
			textgrassCount.Text = $"Celdas de pasto: {grassCount.ToString()}";
			textRiverCount.Text = $"Celdas de rio: {riverCount.ToString()}";
			textGroundCount.Text = $"Celdas de tierra: {groundCount.ToString()}";
		}

		public void GetNodes()
		{
			vBoxContainer = GetChild<VBoxContainer>(0);
			cameraElevation = cameraGimball.GetChild<Spatial>(0);
			cameraPos = vBoxContainer.GetChild<Label>(0);
			elevation = vBoxContainer.GetChild<Label>(1);
			textCountCells = vBoxContainer.GetChild<Label>(2);
			textgrassCount = vBoxContainer.GetChild<Label>(3);
			textRiverCount = vBoxContainer.GetChild<Label>(4);
			textGroundCount = vBoxContainer.GetChild<Label>(5);
			countCells = floor.GetUsedCells();
			GetTypeCells();
		}

		private void GetTypeCells()
		{
			foreach (var cell in countCells)
			{
				var cellPos = (Vector3)cell;
				var number = floor.GetCellItem((int)cellPos.x, (int)cellPos.y, (int)cellPos.z);
				switch (number)
				{
					case 0:
						grassCount++;
						break;
					case 1:
						riverCount++;
						break;
					case 2:
						groundCount++;
						break;
				}
			}
		}
	}
}