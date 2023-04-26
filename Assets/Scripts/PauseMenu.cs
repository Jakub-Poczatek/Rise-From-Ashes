using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseScreen;
    public Button exitBtn, resumeBtn;

    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        pauseScreen.SetActive(false);
        exitBtn.onClick.AddListener(Exit);
        resumeBtn.onClick.AddListener(() => Pause(!isPaused));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Pause(!isPaused);
    }

    private void Pause(bool paused)
    {
        isPaused = !isPaused;
        if (paused) GameManager.Instance.uiController.DisableAllPanels();
        Time.timeScale = paused ? 0 : 1;
        pauseScreen.SetActive(paused);
        //Cursor.visible = paused;
        //Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
