using Godot;
using System;

public class UI : Control
{
    public override void _Process(float delta)
	{
		var label = (Label) GetNode("Label");
		int id = GetTree().GetNetworkUniqueId();

		var node = (Node) GetParent().GetNode("Network/Players/"+id);

		if (node != null && node.IsInGroup("Actor"))
		{
			var player = (Player) node;
			label.Text = "HP: " + player.CurrentHP;
		}
	}
}
