using Godot;
using System;

public partial class Player : KinematicBody
{
	private const string AREA_NAME = "Area";

	public enum Equipment
	{
		WEAPON,
		SHIELD
	}

	private string GetLocationEquip(Equipment equip)
	{
		switch(equip)
		{
			case Equipment.WEAPON:
				return "Mesh/Skeleton/arm_left/bone_arm_ant/bone_hand";
			case Equipment.SHIELD:
				return "Mesh/Skeleton/arm_right/bone_arm_ant/bone_hand";
			default:
				return null;
		}
	}
	
	public void SetEquipment(string name, Equipment equip, bool notify=false)
	{
		var packed = ResourceLoader.Load("res://" + name + ".tscn") as PackedScene;
		
		if (packed != null)
		{
			var model = (Spatial) packed.Instance();
			var area = (Area) model.GetNode(AREA_NAME);
			var node = GetNode(GetLocationEquip(equip));
			
			if (node != null)
			{
				node.AddChild(model);

				if (notify)
				{
					Rpc("OnChangeWeapon", this.Id, name, equip);
				}
			}
		}
	}
}
