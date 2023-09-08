using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System;

public class EnemyController : MonoBehaviour
{
    [Header("Components")]
    public CharacterController controller;
	public SkeletonAnimation skeletonAnimation;

    [Header("Attributes")]
    public float moveSpeed;
    public float attackSpeed;
    public string enemyType = "Ground";

    public float gravityScale = 6.6f;
    Vector3 velocity = default(Vector3);
    
    bool onRest = false;
    bool onAttack = true;
    bool wasGrounded = false;

    public Transform[] pathPoints;
    int pathIndex = 0;



    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        //PlayAnimation("Move", 0, moveSpeed, true);
        
    }


    void Update()
    {
        bool isGrounded = controller.isGrounded;
        Vector3 gravityDeltaVelocity = Physics.gravity * gravityScale * Time.deltaTime;

        if(!onRest)//移动
        {
            if(pathIndex >= pathPoints.Length)
            {
                pathIndex = pathIndex - 1;
            }
            else
            {
                MoveTo(pathPoints[pathIndex]);
            }
        }
        if (!onRest && Vector3.Distance(transform.position, pathPoints[pathIndex].position) < 0.5f)
        {
            pathIndex = pathIndex + 1;
            
            if(pathIndex == pathPoints.Length)
            {
                StartCoroutine(attack());
            }

            else
            {
                Rest(1);
            }

            
        }



        if (!isGrounded)//重力
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
        controller.Move(velocity * Time.deltaTime);//移动
        if(velocity.x > 0)
        {
            skeletonAnimation.Skeleton.ScaleX = 1;
        }
        else if(velocity.x < 0)
        {
            skeletonAnimation.Skeleton.ScaleX = -1;
        }
		wasGrounded = isGrounded;

        
        //Debug.Log(velocity.x);
    }

    void PlayAnimation(string name, int type, float speed, bool loop)
    {
        
        if(type == 0)
        {
        skeletonAnimation.AnimationState.TimeScale = speed;
        skeletonAnimation.AnimationState.SetAnimation(0, name, loop);
        }

        if(type == 1)
        {
        skeletonAnimation.AnimationState.TimeScale = speed;
        skeletonAnimation.AnimationState.AddAnimation(0, name, loop, 0);
        }
    }

    void MoveTo(Transform targetPoint)
    {
        Vector3 moveDirection = (targetPoint.position - transform.position).normalized;
        velocity += moveDirection * (moveSpeed - Mathf.Abs(velocity.x)) * Time.deltaTime;
        
        //transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
    }

    void MoveToNextPoint()
    {
        onRest = false;
        PlayAnimation("Move", 0, moveSpeed/2, true);
    }

    void Rest(float time)
    {
        onRest = true;
        PlayAnimation("Idle", 0, moveSpeed/2, true);
        velocity.x = 0;
        velocity.y = 0;
        Invoke("MoveToNextPoint", time);
    }

    private IEnumerator fire(Vector3 fireDestination)
    {
        EnemyAttack attackScript = GetComponent<EnemyAttack>();
        yield return new WaitForSeconds(0.8f/attackSpeed);
        attackScript.FireAmmo(fireDestination);
    }
    private IEnumerator attack()
    {
        EnemyAttack attackScript = GetComponent<EnemyAttack>();
        while(true)
        {
            if(velocity.x>0)
		    {
			    skeletonAnimation.Skeleton.ScaleX = 1;
			    skeletonAnimation.AnimationState.SetAnimation(0, "Attack", false);
			    Vector3 fireDestination = transform.position;
			    fireDestination.y += 2;
		    	fireDestination.x += 2;
			    yield return StartCoroutine(fire(fireDestination));
		    }
            else if(velocity.x<0)
		    {
			    skeletonAnimation.Skeleton.ScaleX = -1;
			    skeletonAnimation.AnimationState.SetAnimation(0, "Attack", false);
			    Vector3 fireDestination = transform.position;
			    fireDestination.y += 2;
			    fireDestination.x -= 2;
			    yield return StartCoroutine(fire(fireDestination));
		    }
            
        }
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        StopCoroutine(attack());
    }

}
