using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum AIState
    {
        Stopped,
        Searching,
        Moving,
        Attacking
    }
    
    protected string target;
    protected float rotateSpeed;
    protected float moveSpeed;
    protected float health;

    protected Animator anim;

    protected AIState state;
    protected GameObject targetObject;

    // Start is called before the first frame update
    void Start()
    {
        target = "EnemyUnit";
        rotateSpeed = 120 * (Random.Range(-1, 1) == 0 ? 1 : -1);
        moveSpeed = 1.0f;
        health = 100;
        anim = GetComponent<Animator>();

        state = AIState.Stopped;

        if (target != "")
        {
            state = AIState.Searching;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case AIState.Searching:
                {
                    LookForTarget();
                }
                break;
            case AIState.Moving:
                {
                    if (targetObject)
                        MoveToTarget();
                    else
                        ResetAI();
                }
                break;
            case AIState.Attacking:
                {
                    if (targetObject)
                        Attack();
                    else
                        ResetAI();
                }
                break;
            default:
                break;
        }
        //Look for target
        
        //Move To Target
        //Attack
    }

    private void LookForTarget()
    {
        Debug.DrawRay(transform.position + Vector3.up * 0.5f, transform.forward * 50, Color.blue);

        RaycastHit[] hits = Physics.RaycastAll(transform.position + Vector3.up * 0.5f, transform.forward, 50);

        foreach(RaycastHit hit in hits)
        {
            if (hit.transform.CompareTag(target))
            {
                targetObject = hit.transform.gameObject;
                state = AIState.Moving;
                anim.SetBool("Walk", true);
                return;
            }
        }

        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }

    private void MoveToTarget()
    {
        if(Vector3.Distance(transform.position, targetObject.transform.position) > 1.0f)
        {
            //Turn to target
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetObject.transform.position - transform.position), 10 * rotateSpeed * Time.deltaTime);

            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        else
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Attack", true);
            state = AIState.Attacking;
        }
    }

    private void Attack()
    {
        StartCoroutine(KeepAttacking());
    }

    private IEnumerator KeepAttacking()
    {
        yield return new WaitForSeconds(1.0f);
        if (targetObject && targetObject.GetComponent<Character>().Hit(30))
        {
            //Returns true if target dead
            targetObject = null;
            anim.SetBool("Attack", false);
            state = AIState.Searching;

            yield return new WaitForEndOfFrame();
        }
    }

    public bool Hit(float damage)
    {
        health -= damage;
        Debug.Log(gameObject.name + health);

        if(health < 0)
        {
            Destroy(gameObject);
            return true;
        }

        return false;
    }

    private void ResetAI()
    {
        state = AIState.Searching;
        anim.SetBool("Walk", false);
        anim.SetBool("Attack", false);
        targetObject = null;
    }
}
