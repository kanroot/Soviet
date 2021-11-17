using Godot;

namespace Soviet.Soviet.Utils
{
	public static class NodeExtensions
	{
		public static T GetSibling<T> (this Node node) where T : Node
		{
			var parent = node.GetParent();
			return parent.GetChildFromType<T>();
		}

		public static T GetChildFromType<T>(this Node node) where T : Node
		{
			var children = node.GetChildren();
			foreach (var child in children)
			{
				if (child is T tChild)
				{
					return tChild;
				}
			}

			return null;
		}
	}
}