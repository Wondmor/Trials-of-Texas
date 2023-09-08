using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System;


public class PlayerController : MonoBehaviour
{

    [Header("Components")]
	public CharacterController controller;
	public SkeletonAnimation skeletonAnimation;

    [Header("Controls")]
	public string XAxis = "Horizontal";
	public string YAxis = "Vertical";
	public string JumpButton = "Jump";

	[Header("Attributes")]
	public float jumpSpeed = 25;
	public float maxJumpDuration = 0.2f;
	public float attackSpeed = 1f;

    public float gravityScale = 6.6f;


    Vector2 input = default(Vector2);
	Vector3 velocity = default(Vector3);
	bool wasGrounded = false;

	float jumpTime = 0;
	float attackTime = 0;
	float attackStart = 0;

	float maxGameTime = 360;

	bool doJump = false;
	bool onAttack = false;
	bool isVelocityLocked = false;
	bool onSkill = false;
	bool earthQuakeSkill = false;

    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
		//skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
    }

    void Update()
    {
        bool isGrounded = controller.isGrounded;
		input.x = Input.GetAxis(XAxis);
		input.y = Input.GetAxis(YAxis);
        Vector3 gravityDeltaVelocity = Physics.gravity * gravityScale * Time.deltaTime;


		if(Input.GetButtonDown(JumpButton)&&isGrounded)//跳跃
		{
			doJump = true;
			skeletonAnimation.AnimationState.SetAnimation(0, "Start_3", false);
			skeletonAnimation.AnimationState.AddAnimation(0, "Idle", true,0);
		}
		if(Input.GetButtonUp(JumpButton)&&!isGrounded)
		{
			doJump = false;
		}
        if (doJump) 
        {
			velocity.y = jumpSpeed;
			jumpTime += Time.deltaTime;
			if(jumpTime>maxJumpDuration)
			{
				doJump = false;
				jumpTime = 0;
			}
		} 



		Attack attackScript = GetComponent<Attack>();
		if(Input.GetButtonDown(XAxis)&&!onAttack)//攻击
		{
			
			attackStart = Time.time;
			onAttack = true;
			skeletonAnimation.AnimationState.TimeScale = attackSpeed;
			attackTime = 1 / attackSpeed * GetAnimationDurationByName("Attack") * 0.5f;
			if(input.x>0)
			{
				skeletonAnimation.Skeleton.ScaleX = 1;
				skeletonAnimation.AnimationState.SetAnimation(0, "Attack", false);
				Vector3 fireDestination = transform.position;
				fireDestination.y += 3;
				fireDestination.x += 2;
				attackScript.FireAmmo(fireDestination);
			}
			if(input.x<0)
			{
				skeletonAnimation.Skeleton.ScaleX = -1;
				skeletonAnimation.AnimationState.SetAnimation(0, "Attack", false);
				Vector3 fireDestination = transform.position;
				fireDestination.y += 3;
				fireDestination.x -= 2;
				attackScript.FireAmmo(fireDestination);
			}



		}
		if(Time.time-attackStart > attackTime)
		{
			onAttack = false;
			skeletonAnimation.AnimationState.TimeScale = 1;
			skeletonAnimation.AnimationState.AddAnimation(0, "Idle", true,0);
		}


		if (!isGrounded&&!doJump&&!onSkill)//空中技能
		{
			if(Input.GetButtonDown(JumpButton))
			{
				onSkill = true;
				onAttack = true;
				attackTime = 100;
				skeletonAnimation.AnimationState.TimeScale = attackSpeed;
				LockVelocity(GetAnimationDurationByName("Skill_2_Loop")*2/attackSpeed - 0.4f/attackSpeed);
				skeletonAnimation.AnimationState.SetAnimation(0, "Skill_2_Loop", false);
				Invoke("FlixSkeleton", GetAnimationDurationByName("Skill_2_Loop")/attackSpeed);
				skeletonAnimation.AnimationState.AddAnimation(0, "Skill_2_Loop", false,0);
				skeletonAnimation.AnimationState.AddAnimation(0, "Start_3", false,0);
				skeletonAnimation.AnimationState.AddAnimation(0, "Idle", true,0);
				//发射投射物
				Vector3 fireDestination = transform.position;
				fireDestination.y += 3;
				fireDestination.x += 2;
				attackScript.FireAmmo(fireDestination);
				attackScript.FireAmmo(fireDestination);
				fireDestination.x -= 4;
				attackScript.FireAmmo(fireDestination);
				attackScript.FireAmmo(fireDestination);

			}

		}
		
		

		if (!isGrounded&&!doJump&&!onSkill)//下坠技能
		{
			if(Input.GetKeyDown(KeyCode.S))
			{
				earthQuakeSkill = true;
				onSkill = true;
				velocity.y = -30f;
				skeletonAnimation.AnimationState.SetAnimation(0, "Start", false);
			}
		}
		if(isGrounded&&!wasGrounded&&earthQuakeSkill)
		{
			//发射投射物
			Vector3 fireDestination = transform.position;
			fireDestination.y += 3;
			fireDestination.x += 2;
			attackScript.FireAmmo(fireDestination);
			attackScript.FireAmmo(fireDestination);
			attackScript.FireAmmo(fireDestination);
			fireDestination.x -= 4;
			attackScript.FireAmmo(fireDestination);
			attackScript.FireAmmo(fireDestination);
			attackScript.FireAmmo(fireDestination);
			earthQuakeSkill = false;
			StartCoroutine(ResetSkill(0));
		}

		//重置技能
		if(isGrounded&&onSkill)
		{
			StartCoroutine(ResetSkill(0));
		}

		if (!isGrounded&&!doJump&&!isVelocityLocked) 
        {
			if (wasGrounded) 
			{
				if (velocity.y < 0)
					velocity.y = 0;
			} 
            else 
            {
				velocity += gravityDeltaVelocity;
			}
		}
		else if(isVelocityLocked)
		{
			velocity.y = 0;
		}


        controller.Move(velocity * Time.deltaTime);
		wasGrounded = isGrounded;
        

		//测试器
		if(Input.GetKeyDown(KeyCode.K))
		{
			StartCoroutine(GetComponent<Player>().DamageCharacter(1, 0.0f));
		}
		if(Input.GetKeyDown(KeyCode.L))
		{
			StartCoroutine(GetComponent<Player>().DamageCharacter(-1, 0.0f));
		}

    }

	float GetAnimationDurationByName(string animationName)//查询动画时长
    {
        // 获取动画对象
        Spine.Animation animation = skeletonAnimation.SkeletonDataAsset.GetAnimationStateData().SkeletonData.FindAnimation(animationName);

        // 获取动画的时长（以秒为单位）
        float duration = animation.Duration;
        return duration;
    }

	private void LockVelocity(float time)//静止
	{
		isVelocityLocked = true;
		Invoke("UnlockVelocity", time);
	}

	private void UnlockVelocity()
    {
        isVelocityLocked = false;
		skeletonAnimation.AnimationState.TimeScale = 1;
		velocity.y = -2;
		
    }

	private void FlixSkeleton()
	{
		float i = skeletonAnimation.Skeleton.ScaleX;
		if(i>0)
		{
			skeletonAnimation.Skeleton.ScaleX = -1;
		}

		else
		{
			skeletonAnimation.Skeleton.ScaleX = 1;
		}
	}

	private IEnumerator ResetSkill(float time)
	{
		yield return new WaitForSeconds(time);
		onSkill = false;
		onAttack = false;
		attackTime = 0;
		print("1");
	}
}

