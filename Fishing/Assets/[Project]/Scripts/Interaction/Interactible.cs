using UnityEngine;

public abstract class Interactible : MonoBehaviour
{
    [SerializeField] public string interactionText = "clic to interact";
    public virtual void Interact() { }
}
