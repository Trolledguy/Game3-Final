using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Player")]
    public Player playerUI;

    [Header("Player Stats UI")]
    public Image hpIcon;
    public Texture2D cursorTexture;

    [Header("Pause Menu")]
    public bool isPaused = false;

    public GameObject pauseMenu;
    public Button resumeButton;
    public Button quitButton;

    [Header("Win Screen")]
    public GameObject winScreen;
    public Button winQuitButton;
    
    
    void Awake()
    {
        TogglePauseMenu(false);
        Setup();
    }

    void FixedUpdate()
    {
        HandlePlayerUI();
    }

    private void HandlePlayerUI()
    {
        if(playerUI != null && playerUI.gameObject.activeSelf)
        {
            hpIcon.fillAmount = (float)playerUI.currentHealth / playerUI.baseHealth;
        }
    }

    public void ShowWinScreen()
    {
        Time.timeScale = 0f;
        winScreen.SetActive(true);
    }

    public void ShowLoseScreen()
    {
        Time.timeScale = 0f;
        TogglePauseMenu(true);
    }

    private void Setup()
    {
        if(instance != this)
        {
            instance = this;
        }

        resumeButton.onClick.AddListener(delegate 
        { 
            if(playerUI == null)
            {
                SceneManager.LoadScene(0);
                return;
            }
            else
            {
                TogglePauseMenu(false);
            }
        });
        winQuitButton.onClick.AddListener(() => SceneManager.LoadScene(0));
        quitButton.onClick.AddListener(() => SceneManager.LoadScene("Menu Scene"));


        winScreen.SetActive(false);
        
    }

    public void TogglePauseMenu(bool _state)
    {
        isPaused = _state;

        pauseMenu.SetActive(isPaused);
        

        if(isPaused)
        {
            Time.timeScale = 0f;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        }
    }

}