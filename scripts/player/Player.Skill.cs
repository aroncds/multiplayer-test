using Godot;
using System;
using System.Collections.Generic;

public partial class Player : KinematicBody
{
	public class Skill
	{
		private int _id;
		private string _name;
		private string _anim;
		private float _countdown;
		public float currentCountdown;

		public int Id
		{
			get {return _id;}
		}

		public float Countdown
		{
			get {return _countdown;}
		}
		
		public string Anim
		{
			get {return _anim;}
		}
		
		public Skill(int id, string name, string anim, float countdown)
		{
			this._id = id;
			this._name = name;
			this._anim = anim;
			this._countdown = countdown;
	
			this.currentCountdown = 0.0f;
		}
	}

	private Dictionary<int, Skill> skills;
	
	public Skill GetSkill(int id)
	{
		Skill skill;
		if (skills.TryGetValue(id, out skill))
		{
			return skill;
		}
		
		return null;
	}
	
	public bool IsSkillDisponible(int id)
	{
		Skill skill = GetSkill(id);
		
		if (skill != null)
		{
			return skill.currentCountdown <= 0;
		}
		
		return false;
	}
	
	public void ExecuteSkill(int id)
	{
		Skill skill = GetSkill(id);
		
		if (skill != null)
		{
			skill.currentCountdown = skill.Countdown;
			var anim = (AnimationPlayer) GetNode("anim");

			anim.Play(skill.Anim, 0.1f);
			//anim.Queue("normal_attack_blocked");
			anim.SetBlendTime(skill.Anim, "idle", 0.2f);
			anim.SetBlendTime("normal_attack_blocked", "idle", 0.3f);
			anim.Queue("idle");
		}
	}

	public void Damage(float damage)
	{
		this.currentHP -= damage;
		
		if (this.currentHP <= 0)
		{
			GD.Print("Died: " + this.Id);
			QueueFree();
		}
	}

	private void RequestSkill(int id)
	{
		if (IsSkillDisponible(id))
		{
			GD.Print("request skill");
			GD.Print("skill " + Id);
			
			Rpc("OnRequestAttack", this.Id, id);
		}
	}
	
	private void _UpdateSkill(float delta)
	{
		foreach (KeyValuePair<int, Skill> entry in skills)
		{
			Skill skill = entry.Value;

			if (skill.currentCountdown > 0)
			{
				skill.currentCountdown -= delta;

				if (skill.currentCountdown < 0)
				{
					skill.currentCountdown = 0;
				}
			}
		}
	}
}
