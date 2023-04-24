using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    public ScrollRect credits;
    public float scrollSpeed = 0.1f;
    private float newPosition;

    // Start is called before the first frame update
    void Start()
    {
        newPosition = 1 - Mathf.Repeat(Time.time * scrollSpeed, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (newPosition > 0.01f)
        {
            newPosition = 1 - Mathf.Repeat(Time.time * scrollSpeed, 1f);
            credits.verticalNormalizedPosition = newPosition;
        }
        else
            SceneManager.LoadScene("EndScreen");
    }
}
