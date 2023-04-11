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
    private GameObject workBuilding;
    private GameObject houseBuilding = null;
    private float workingTime = 100f;
    private bool isWorking = false;
    private bool stateRunning = false;
    public CitizenData citizenData;

    private readonly int baseSleep = 8;
    private readonly int baseFood = 5;

    public GameObject HouseBuilding { get => houseBuilding; set => houseBuilding = value; }

    // Start is called before the first frame update
    void Start()
    {
        CreateCitizenData();
        anim = this.GetComponent<Animator>();
        target = new(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        agent = this.GetComponent<NavMeshAgent>();
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
                yield return new WaitForSeconds(0.1f);
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
                        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                        anim.SetBool("isWorking", true);
                        workBuilding.GetComponent<WorkableStructure>().StartWorking(this.gameObject);
                        InvokeRepeating(nameof(UpdateExperience), 0, 1);
                        yield return new WaitForSeconds(citizenData.WorkRestRatio.Item1);
                        CancelInvoke(nameof(UpdateExperience));
                        workBuilding.GetComponent<WorkableStructure>().StopWorking(this.gameObject);
                        anim.SetBool("isWorking", false);
                        transform.localScale = Vector3.one;
                        target = houseBuilding.transform.position;
                    }
                    else
                    {
                        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                        yield return new WaitForSeconds(citizenData.WorkRestRatio.Item2);
                        anim.SetTrigger("triggerWorking");
                        transform.localScale = Vector3.one;
                        target = workBuilding.transform.position;
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
        if (building.GetComponent<WorkableStructure>().HasCapacity())
        {

            // Remove previous workplace
            if (workBuilding != null)
            {
                workBuilding.GetComponent<WorkableStructure>().RemoveCitizen(this.gameObject);
            }

            workBuilding = building;
            target = workBuilding.transform.position;
            anim.SetTrigger("triggerWorking");
            state = State.Working;
            agent.SetDestination(target);
            workBuilding.GetComponent<WorkableStructure>().AddCitizen(this.gameObject);
            AssignNewOccupation(workBuilding.GetComponent<WorkableStructure>().ResourceType);
            stateRunning = false;
        }
    }

    private void UpdateExperience()
    {
        ResourceType workResType = workBuilding.GetComponent<WorkableStructure>().ResourceType;
        switch (workResType)
        {
            case ResourceType.Gold:
                citizenData.skills.GoldProductionExp++;
                break;
            case ResourceType.Food:
                citizenData.skills.FoodProductionExp++;
                break;
            case ResourceType.Wood:
                citizenData.skills.WoodProductionExp++;
                break;
            case ResourceType.Stone:
                citizenData.skills.StoneProductionExp++;
                break;
            case ResourceType.Metal:
                citizenData.skills.MetalProductionExp++;
                break;
        }
    }

    public void UpdateHappiness()
    {
        citizenData.happiness =
            (50 - citizenData.WorkRestRatio.Item1) +
            ((baseFood - citizenData.food) * 5);
    }

    private enum State
    {
        Idle,
        Roaming,
        Working
    }

    private void AssignNewOccupation(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Gold:
                citizenData.occupation = Occupation.Merchant;
                break;
            case ResourceType.Food:
                citizenData.occupation = Occupation.Farmer;
                break;
            case ResourceType.Wood:
                citizenData.occupation = Occupation.Logger;
                break;
            case ResourceType.Stone:
                citizenData.occupation = Occupation.Miner;
                break;
            case ResourceType.Metal:
                citizenData.occupation = Occupation.Miner;
                break;
            default:
                break;
        }
    }

    private void CreateCitizenData()
    {
        string firstName = NameData.firstNames[Random.Range(0, NameData.firstNames.Length)];
        string lastName = NameData.lastNames[Random.Range(0, NameData.lastNames.Length)];
        Skills skills = new(1, 1, 1, 1, 1);
        citizenData = new(
            firstName + " " + lastName,
            Occupation.Citizen,
            100,
            5,
            (50, 50),
            0,
            500,
            skills,
            this
        );
    }
}


