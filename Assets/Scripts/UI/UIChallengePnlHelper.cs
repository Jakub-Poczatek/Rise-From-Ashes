using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIChallengePnlHelper : MonoBehaviour
{
    public Transform contentParent;
    public Button cancelBtn;
    private GameObject challengeGO;
    private Dictionary<Challenge, GameObject> challenges;

    // Start is called before the first frame update
    void Start()
    {
        challenges = new Dictionary<Challenge, GameObject>();
        challengeGO = contentParent.GetChild(0).gameObject;
        CreateChallenges();
        Destroy(contentParent.GetChild(0).gameObject);

        foreach (KeyValuePair<Challenge, GameObject> kvp in challenges)
        {
            Button tempB = kvp.Value.transform.Find("ClaimBtn").GetComponent<Button>();
            tempB.GetComponent<HoverTip>().tipToShow = kvp.Key.reward.ToString();
            tempB.onClick.AddListener(() => ClaimReward(kvp));
        }
        Hide();
    }

    private void ClaimReward(KeyValuePair<Challenge, GameObject> kvp)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.claim);
        ResourceManager.Instance.EarnResources(kvp.Key.reward);
        challenges.Remove(kvp.Key);
        Destroy(kvp.Value);
    }

    public void DisplayChallengesMenu()
    {
        foreach(KeyValuePair<Challenge, GameObject> kvp in challenges)
        {
            kvp.Value.transform.Find("ClaimBtn").GetComponent<Button>().interactable = kvp.Key.CheckConditions();
        }
        Show();
    }

    private void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    private void CreateChallenges()
    {
        foreach(Challenge c in ChallengeManager.Instance.challenges)
        {
            GameObject challenge = Instantiate(challengeGO);
            challenge.transform.parent = contentParent;
            challenge.transform.Find("ChallengeName").GetComponent<TMP_Text>().text = c.name;
            challenge.transform.Find("ChallengeMessage").GetComponent<TMP_Text>().text = c.message;
            challenges.Add(c, challenge);
        }
    }
}
