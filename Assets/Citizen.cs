using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class Citizen : MonoBehaviour
{
    private Animator anim;
    private AnimatorStateInfo info;
    private NavMeshAgent agent;
    private Vector3 target;
    private State state = State.Idle;

    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        target = new(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        agent = this.GetComponent<NavMeshAgent>();
        anim.SetTrigger("triggerRoaming");
    }

    // Update is called once per frame
    void Update()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);
        Debug.Log(gameObject.name);
        if (info.IsName("Roaming"))
            state = State.Roaming;
        UpdateNavigation();
    }

    private void UpdateNavigation()
    {
        switch (state)
        {
            case State.Roaming:
                if (Vector3.Distance(this.transform.position, target) < 1.0f)
                {
                    Vector3 destination;
                    NavMeshPath tempPath;
                    do
                    {
                        tempPath = new NavMeshPath();
                        destination = new(this.transform.position.x + Random.Range(-50, 50),
                                                this.transform.position.y,
                                                this.transform.position.z + Random.Range(-50, 50));
                    }
                    while (Physics.CheckSphere(destination, 1, LayerMask.GetMask("Structures")) || !agent.CalculatePath(destination, tempPath));
                    target = destination;
                    agent.SetDestination(target);
                }
                break;
            default:
                break;
        }
    }

    private enum State
    {
        Idle,
        Roaming
    }
}


