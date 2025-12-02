using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        // Assign button click listeners
        startButton.onClick.AddListener(OnStartButtonClicked);
        //optionsButton.onClick.AddListener(OnOptionsButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        SceneManager.LoadScene("Hub");
    }

    private void OnOptionsButtonClicked()
    {
        Debug.Log("Options Button Clicked");
        // Add logic to open options menu
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }
}