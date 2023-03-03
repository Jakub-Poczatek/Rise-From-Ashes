using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Citizen : MonoBehaviour
{
    private Animator anim;
    private AnimatorStateInfo info;
    private NavMeshAgent agent;
    private Vector3 target;
    private State state = State.Idle;
    private GameObject workBuildingPosition;
    public float workingTime = 5f;
    private bool isWorking = false;
    private Vector3 townHallPos;
    private bool stateRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        target = new(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        agent = this.GetComponent<NavMeshAgent>();
        townHallPos = GameObject.FindGameObjectWithTag("Town Hall").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);
        if (!stateRunning)
        {
            stateRunning = true;
            StartCoroutine(UpdateNavigation());
        }
    }

    private IEnumerator UpdateNavigation()
    {
        switch (state)
        {
            case State.Idle:
                yield return new WaitForSeconds(3);
                anim.SetTrigger("triggerRoaming");
                state = State.Roaming;
                break;
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
            case State.Working:
                if (Vector3.Distance(this.transform.position, target) < 1.0f) {
                    if (info.IsName("TravelToWork"))
                    {
                        anim.SetBool("isWorking", true);
                        workBuildingPosition.GetComponent<WorkableStructure>().StartWorking(this.gameObject);
                        yield return new WaitForSeconds(workingTime);
                        workBuildingPosition.GetComponent<WorkableStructure>().StopWorking(this.gameObject);
                        anim.SetBool("isWorking", false);
                        target = townHallPos;
                    }
                    else
                    {
                        anim.SetTrigger("triggerWorking");
                        target = workBuildingPosition.transform.position;
                    }
                    agent.SetDestination(target);
                }
                break;
            default:
                break;
        }
        stateRunning = false;
    }

    public void AssignWork(GameObject building)
    {
        workBuildingPosition = building;
        target = workBuildingPosition.transform.position;
        anim.SetTrigger("triggerWorking");
        state = State.Working;
        agent.SetDestination(target);
        building.GetComponent<WorkableStructure>().AddWorker(this.gameObject);
        stateRunning = false;
    }

    private enum State
    {
        Idle,
        Roaming,
        Working
    }
}


