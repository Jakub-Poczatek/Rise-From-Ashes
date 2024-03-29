using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Citizen : MonoBehaviour
{
    private Animator anim;
    private AnimatorStateInfo info;
    private NavMeshAgent agent;
    private Vector3 target;
    private State state = State.Roaming;
    private GameObject workBuilding;
    private GameObject houseBuilding = null;
    private float workingTime = 100f;
    private bool isWorking = false;
    private bool stateRunning = false;
    private GameObject model;
    public CitizenData citizenData;
    public GameObject[] models;

    private readonly int baseWorkRestSplit = 50;
    private readonly int baseFood = 5;

    public GameObject HouseBuilding { get => houseBuilding; set => houseBuilding = value; }

    private void Awake()
    {
        model = Instantiate(models[Random.Range(0, models.Length)]);
        model.transform.SetParent(transform);
        model.transform.localPosition = new Vector3(0, 0, 0);
        Destroy(transform.GetChild(0).gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateCitizenData();
        target = new(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        agent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(anim == null)
            anim = GetComponentInChildren<Animator>();
        info = anim.GetCurrentAnimatorStateInfo(0);

        if (state == State.Working && workBuilding == null)
        {
            ResetBehaviour();
            stateRunning = false;
        }

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
                if (Vector3.Distance(this.transform.position, target) < 0.5f) {
                    if (info.IsName("TravelToWork"))
                    {
                        agent.enabled = false;
                        transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                        transform.GetComponentInChildren<CapsuleCollider>().enabled = false;
                        anim.SetBool("isWorking", true);
                        anim.SetBool("isTravellingWork", false);

                        workBuilding.GetComponent<WorkableStructure>().StartWorking(this.gameObject);
                        InvokeRepeating(nameof(UpdateExperience), 0, 1);
                        yield return new WaitForSeconds(citizenData.WorkRestRatio.Item1);
                        CancelInvoke(nameof(UpdateExperience));
                        workBuilding.GetComponent<WorkableStructure>().StopWorking(this.gameObject);

                        agent.enabled = true;
                        transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                        transform.GetComponentInChildren<CapsuleCollider>().enabled = true;
                        anim.SetBool("isWorking", false);
                        anim.SetBool("isTravellingHome", true);
                        target = houseBuilding.transform.position;
                    }
                    else if (info.IsName("TravelToHome"))
                    {
                        agent.enabled = false;
                        transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                        transform.GetComponentInChildren<CapsuleCollider>().enabled = false;
                        anim.SetBool("isResting", true);
                        anim.SetBool("isTravellingHome", false);

                        yield return new WaitForSeconds(citizenData.WorkRestRatio.Item2);

                        agent.enabled = true;
                        transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                        transform.GetComponentInChildren<CapsuleCollider>().enabled = true;
                        anim.SetBool("isResting", false);
                        anim.SetBool("isTravellingWork", true);
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
            ResetBehaviour();

            // Remove previous workplace
            if (workBuilding != null)
            {
                workBuilding.GetComponent<WorkableStructure>().RemoveCitizen(this.gameObject);
            }

            workBuilding = building;
            target = workBuilding.transform.position;
            anim.SetBool("isUnemployed", false);
            anim.SetBool("isTravellingWork", true);
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
                citizenData.skills.Gold.ProductionExp++;
                break;
            case ResourceType.Food:
                citizenData.skills.Food.ProductionExp++;
                break;
            case ResourceType.Wood:
                citizenData.skills.Wood.ProductionExp++;
                break;
            case ResourceType.Stone:
                citizenData.skills.Stone.ProductionExp++;
                break;
            case ResourceType.Metal:
                citizenData.skills.Metal.ProductionExp++;
                break;
        }
    }

    private enum State
    {
        Roaming,
        Working
    }

    public void UpdateHappiness()
    {
        citizenData.Happiness =
            50 +
            (baseWorkRestSplit - citizenData.WorkRestRatio.Item1) +
            ((citizenData.Food - baseFood) * 5);
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
                citizenData.occupation = Occupation.Stonemason;
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
        string firstName;
        if (model.name.Contains("Male"))
            firstName = NameData.firstNamesMale[Random.Range(0, NameData.firstNamesMale.Length)];
        else
            firstName = NameData.firstNamesFemale[Random.Range(0, NameData.firstNamesFemale.Length)];
        string lastName = NameData.lastNames[Random.Range(0, NameData.lastNames.Length)];
        Skills skills = new();
        citizenData = new(
            firstName + " " + lastName,
            Occupation.Citizen,
            100,
            1,
            (50, 50),
            30,
            500,
            skills,
            this
        );
        Skill[] skillList = new Skill[5] { 
            citizenData.skills.Gold,
            citizenData.skills.Food,
            citizenData.skills.Wood,
            citizenData.skills.Stone,
            citizenData.skills.Metal,
        };
        skillList = RandomListShuffle(skillList);
        float totalExpToAllocate = 50;
        float tempExp = Random.Range(0, totalExpToAllocate);
        totalExpToAllocate -= tempExp;
        IncreaseSkillExp(tempExp, skillList[0]);
        tempExp = Random.Range(0, totalExpToAllocate);
        totalExpToAllocate -= tempExp;
        IncreaseSkillExp(tempExp, skillList[1]);
        tempExp = Random.Range(0, totalExpToAllocate);
        totalExpToAllocate -= tempExp;
        IncreaseSkillExp(tempExp, skillList[2]); ;
        tempExp = Random.Range(0, totalExpToAllocate);
        totalExpToAllocate -= tempExp;
        IncreaseSkillExp(tempExp, skillList[3]);
        IncreaseSkillExp(totalExpToAllocate, skillList[4]);
    }

    private Skill[] RandomListShuffle(Skill[] skills)
    {
        System.Random random = new();
        for (int i = skills.Length - 1; i >= 1; i--)
        {
            int j = random.Next(i + 1);
            (skills[i], skills[j]) = (skills[j], skills[i]);
        }
        return skills;
    }

    private void IncreaseSkillExp(float tempExp, Skill skill)
    {
        float tempDiff;
        do
        {
            tempDiff = skill.GetExpDiff();
            skill.ProductionExp += tempExp;
            tempExp -= tempDiff;
        } while (tempExp > 0);
    }

    public void Die()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.death);
        ResetBehaviour();
        if (workBuilding != null)
            workBuilding.GetComponent<Structure>().RemoveCitizen(this.gameObject);
        if (houseBuilding != null)
            houseBuilding.GetComponent<Structure>().RemoveCitizen(this.gameObject);
        GameManager.Instance.uiController.citizenListPanelHelper.DestroyCitizenEntry(this);
        ResourceManager.Instance.CurrentCitizenCapacity--;
        Destroy(this.gameObject);
    }

    private void ResetBehaviour()
    {
        CancelInvoke();
        StopAllCoroutines();
        target = new(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        transform.localScale = Vector3.one;
        citizenData.occupation = Occupation.Citizen;
        state = State.Roaming;
        anim.SetBool("isUnemployed", true);
        anim.SetBool("isTravellingWork", false);
        anim.SetBool("isWorking", false);
        anim.SetBool("isResting", false);
        anim.SetBool("isTravellingHome", false);
    }
}


