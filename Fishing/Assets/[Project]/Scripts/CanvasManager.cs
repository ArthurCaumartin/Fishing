using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;

    [Header("Hub : ")]
    [SerializeField] private GameObject _hubUI;
    [SerializeField] private Button _startFishButton;
    [Header("InGame : ")]
    [SerializeField] private GameObject _inGameUI;
    [SerializeField] private Button _leaveToHubButton;

    private void Awake()
    {
        if (Instance)
            Destroy(Instance.gameObject);
        Instance = this;

        _startFishButton.onClick.AddListener(SetupFishButton);
        _leaveToHubButton.onClick.AddListener(SetupHubButton);
    }

    public void SetupFishButton()
    {
        GameManager.Instance.SetupFish();
        _hubUI.SetActive(false);
        _inGameUI.SetActive(true);
    }

    public void SetupHubButton()
    {
        GameManager.Instance.SetupHub();
        _inGameUI.SetActive(false);
        _hubUI.SetActive(true);
    }
}