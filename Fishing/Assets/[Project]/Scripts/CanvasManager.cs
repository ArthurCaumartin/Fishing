using UnityEngine;
using UnityEngine.UI;

//TODO faire des scriptable pour le load des QTE / interaction
//TODO TOUTE l'ui passe par ici

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;
    [SerializeField] private GameObject _hubUI;

    private void Awake()
    {
        if (Instance)
            Destroy(Instance.gameObject);
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ShowHubUI()
    {
        _hubUI.SetActive(true);
    }

    public void HideHubUI()
    {
        _hubUI.SetActive(false);
    }
}