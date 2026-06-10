using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private string _hubSceneName = "Hub";
    [SerializeField] private string _diveSceneName = "Dive";
    [Space]
    [SerializeField] private Transform _hubCameraTarget;
    [SerializeField] private Transform _playerCameraTatget;
    [SerializeField] private CameraControler _cameraControler;
    [Space]
    [SerializeField] private GameObject _hubEnviroPivot;
    private DiveControler _diveControler;

    void Awake()
    {
        if (Instance)
            Destroy(Instance.gameObject);
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        CanvasManager.Instance.ShowHubUI();
    }

    public void StartDive()
    {
        if (_diveControler) return;
        CanvasManager.Instance.HideHubUI();
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(_diveSceneName, LoadSceneMode.Additive);
        loadOp.completed += (AsyncOperation op) =>
        {
            _hubEnviroPivot.SetActive(false);
            print("Dive scene load !");
            _diveControler = FindObjectOfType<DiveControler>();
            _diveControler.InitDive(_cameraControler);
        };
    }
}
