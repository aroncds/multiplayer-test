using Godot;
using System;
using System.Collections.Generic;

public partial class Player : KinematicBody
{
	enum PlayerState
	{
		IDLE = 1,
		WALK,
		SKILL
	}
	
	enum State
	{
		PRIMARY = 1,
		SECONDARY
	}
	
	private PlayerState[] state;

	/*interface IState
	{
		void OnEnter(Player player, AnimationPlayer anim);
		void OnChange(Player player, State newAnimation);
		void OnFinished(Player player);
	}
	
	abstract class State: IState
	{
		public abstract string GetName();
		
		public bool IsNecessaryFinished()
		{
			return true;
		}
		
		public virtual void OnEnter(Player player, AnimationPlayer anim)
		{
			anim.Play(this.GetName());
		}
		
		public virtual void OnChange(Player player, State newAnimation)
		{
			anim.SetBlendTime(this.GetName(), newAnimation.GetName(), 0.2f);
		}
	}
	
	class IdleState: State
	{
		public string GetName()
		{
			return "idle";
		}
		
		public override void OnFinished(Player player)
		{
			
		}
	}
	
	class NormalAttackState: State
	{
		public string GetName()
		{
			return "skill_normal_attack";
		}
		
		public override void OnEnter(Player player, AnimationPlayer anim)
		{
			
		}
		
		public override void OnFinished(Player player)
		{
			player.ChangeState(new IdleState());
		}
	}
	
	private State[] states;
	private AnimationPlayer animPlayer;
	
	private void ChangeState(State newState, int index)
	{
		if (state != null && state.Name == newState.Name)
		{
			state.OnChange(this, newState);
			newState.OnEnter(this, animPlayer);
			state = newState;
		}
	}
	
	private void _InitializeState()
	{
		state = new IdleState();
		animPlayer = (AnimationPlayer) GetNode("anim");
		animPlayer.Connect("animation_started", this, "OnAnimationStarted");
		animPlayer.Connect("animation_finished", this, "OnAnimationFinished");
	}
	
	private void OnAnimationStarted(string animName)
	{
		if (state != null)
		{
			//state.OnStarted(this, this.animPlayer);
		}
	}
	
	private void OnAnimationFinished(string animName)
	{
		if (state != null)
		{
			state.OnFinished(this);
		}
	}*/
}
