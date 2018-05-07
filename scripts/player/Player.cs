using Godot;
using System;
using System.Collections.Generic;

public partial class Player : KinematicBody
{
	private int _id;
	private bool _master;
	private float maxHP;
	private float currentHP;

	public int Id
	{
		get { return _id; }
		set { this._id = value; }
	}

	public bool Master
	{
		get { return _master; }
		set {
			this._master = value;

			Camera camera = (Camera) GetNode("Camera");

			if (value)
			{
				camera.SetCurrent(true);
			}
			else
			{
				camera.QueueFree();
			}
		}
	}
	
	public float CurrentHP
	{
		get { return currentHP; }
	}
	
	public float MaxHP
	{
		get { return maxHP; }
	}

    public override void _Ready()
    {
        velocity = new Vector3();

		skills = new Dictionary<int, Skill>()
		{
			{1, new Skill(1, "Normal Attack", "skill_normal_attack", 1f)}
		};

		this.maxHP = 100;
		this.currentHP = this.maxHP;

		this.SetEquipment("sword", Equipment.WEAPON);

		AddToGroup("Actor");
    }

	public override void _PhysicsProcess(float delta)
	{
		if (Master)
		{
			_MovementControl(delta);
		}

		_UpdateSkill(delta);
	}

	public override void _Input(InputEvent ev)
	{
		if (Master)
		{
			if (Input.IsActionPressed("ui_normal_attack"))
			{
				this.RequestSkill(1);
			}
		}
	}
}