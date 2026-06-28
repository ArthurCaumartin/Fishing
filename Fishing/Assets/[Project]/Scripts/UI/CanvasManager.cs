using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;
    [SerializeField] private GameEventListener _listenerDiveStart;
    [SerializeField] private GameEventListener _listenerDiveEnd;
    [SerializeField] private PanelInput _panelInputLeft;
    [SerializeField] private PanelInput _panelInputRight;
    [Space]
    [SerializeField] private Button _buttonInteraction;
    [SerializeField] private TextMeshProUGUI _textInteractionButton;


    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        else
            Instance = this;
    }

    private void Start()
    {
        SetInteractionButton(null);

        _listenerDiveStart.Sub(() => _buttonInteraction.gameObject.SetActive(false));
        _listenerDiveEnd.Sub(() => _buttonInteraction.gameObject.SetActive(true));
    }

    public void SetInteractionButton(Interactible interactible)
    {
        _buttonInteraction.onClick.RemoveAllListeners();
        if (interactible == null)
        {
            _buttonInteraction.gameObject.SetActive(false);
        }
        else
        {
            _buttonInteraction.gameObject.SetActive(true);
            _textInteractionButton.text = interactible.interactionText;
            _buttonInteraction.onClick.AddListener(() => { interactible.Interact(); });
        }
    }

    public void EnableInputPanels(bool isActive)
    {
        _panelInputRight.gameObject.SetActive(isActive);
        _panelInputLeft.gameObject.SetActive(isActive);
    }

    public void SetPanelInputAction(Action onDownLeft, Action onUpLeft, Action onDownRight, Action onUpRight)
    {
        _panelInputLeft.onDown = onDownLeft;
        _panelInputLeft.onUp = onUpLeft;

        _panelInputRight.onDown = onDownRight;
        _panelInputRight.onUp = onUpRight;
    }
}
