using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    private PlayerBase enemyTarget;

    [SerializeField] private int enemyDamage = 10;
    [SerializeField] private int enemyAttackTime = 5;

    private NavMeshAgent agent;

    enum ActionState
    {
        Move,
        Attack,
    };

    private ActionState enemyActionState;
    private ActionState EnemyActionState
    {
        get{return enemyActionState;}
        set
        {
            enemyActionState = value;
            switch (value)
            {
                case ActionState.Move:
                    MoveToNewTarget();
                    break;
                case ActionState.Attack:
                    agent.isStopped = true;
                    StartCoroutine(AttackPlayerBase());
                    break;
            }
        }
    }

    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        EnemyActionState = ActionState.Move;
    }

    private void AttackNewBase()
    {
        if (BaseManager.instance.PlayerBases.Count > 0) 
        {
            enemyTarget = FindClosestTarget();
            Debug.Log("AttackNewBase:" + enemyTarget.gameObject.name);
            enemyTarget.OnPlayerBaseDestroyed += AttackNewBase;
            agent.isStopped = false;
            agent.destination = enemyTarget.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // See if we hit a playerBase, stop moving, and attack the playerBase.
        PlayerBase foundBase = other.gameObject.GetComponent<PlayerBase>();
        if (foundBase != null && enemyTarget == foundBase && EnemyActionState == ActionState.Move)
        {
            EnemyActionState = ActionState.Attack;
        }
    }

    IEnumerator AttackPlayerBase()
    {
        while (enemyActionState == ActionState.Attack)
        {
            bool killedTarget = enemyTarget.Damage(enemyDamage);
            if (killedTarget)
            {
                EnemyActionState = ActionState.Move;
                yield return null;
            }

            yield return new WaitForSeconds(enemyAttackTime);
        }
    }

    private void MoveToNewTarget()
    {
        // Find target and start moving towards it.
        enemyTarget = FindClosestTarget();
        if (enemyTarget)
        {
            enemyTarget.OnPlayerBaseDestroyed += AttackNewBase;
            agent.destination = enemyTarget.transform.position;
        }
        else
            Debug.Log("Did not find a target");
    }

    private PlayerBase FindClosestTarget()
    {
        PlayerBase closestPlayerBase = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (PlayerBase playerBase in BaseManager.instance.PlayerBases)
        {
            float dist = Vector3.Distance(playerBase.transform.position, currentPos);
            if (dist < minDist)
            {
                closestPlayerBase = playerBase;
                minDist = dist;
            }
        }
        return closestPlayerBase;
    }
}