using Godot;
using System;

public partial class Player : KinematicBody
{
	private Player GetPlayerById(int id)
	{
		Node node = GetParent().GetNode(id.ToString());

		if (node.IsInGroup("Actor"))
		{
			return (Player) node;
		}

		return null;
	}

    [Remote]
	private void OnMove(Vector3 position, int playerId, bool isMoving, Vector3 angle)
	{
		var player = GetPlayerById(playerId);

		if (player != null)
		{
			if (isMoving)
			{
				Spatial mesh = (Spatial) player.GetNode("Mesh");
				mesh.SetRotation(angle);
			}
	
			var transform = player.GetGlobalTransform();
			transform.origin = position;
			player.SetGlobalTransform(transform);
		}
	}

	[Remote]
	private void OnChangeWeapon(int playerId, string name, Equipment equip)
	{
		var player = GetPlayerById(playerId);

		if (player != null)
		{
			player.SetEquipment(name, equip);
		}
	}

	[Sync]
	private void OnSkillAttack(int playerId, int skillId)
	{
		GD.Print("on request attack");
		var player = GetPlayerById(playerId);

		if (player != null)
		{
			player.ExecuteSkill(skillId);
		}
	}

	[Sync]
	private void OnRequestAttack(int playerId, int id)
	{
		if (GetTree().IsNetworkServer())
		{
			Player player = GetPlayerById(playerId);

			if (player != null)
			{
				if (player.IsSkillDisponible(id))
				{
					Rpc("OnSkillAttack", playerId, id);
				}
			}
		}
	}

	[Sync]
	private void OnReceiveAttack(int playerFromId, int playerTargetId, float damage)
	{
		// Player playerFrom = GetPlayerById(playerFromId);
		Player playerTarget = GetPlayerById(playerTargetId);

		if (playerTarget != null)
		{
			playerTarget.Damage(damage);
		}
	}
}
