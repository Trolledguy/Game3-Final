using UnityEngine;
using UnityEngine.InputSystem;
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
    public GameObject pauseMenu;
    public Button resumeButton;
    public Button quitButton;
    
    public bool isPaused = false;

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

    

    private void Setup()
    {
        if(instance != this)
        {
            instance = this;
        }

        resumeButton.onClick.AddListener(() => TogglePauseMenu(false));
        quitButton.onClick.AddListener(() => Application.Quit());
        
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