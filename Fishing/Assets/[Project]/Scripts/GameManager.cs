using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private CameraControler _cameraControler;


    void Awake()
    {
        if (Instance)
            Destroy(Instance.gameObject);
        Instance = this;
    }

    private void Start()
    {
        CanvasManager.Instance.SetupHubButton();
    }

    public void SetupFish()
    {
        _cameraControler.TargetPlayer();
    }

    public void SetupHub()
    {
        _cameraControler.TargetHub();
    }
}
