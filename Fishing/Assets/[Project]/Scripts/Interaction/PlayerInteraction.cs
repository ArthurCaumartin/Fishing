using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float _detectionRange = 5;
    private Interactible _currentInteractible;

    private void Update()
    {
        Interactible newInteractible = GetNearestInteractible();
        if(_currentInteractible == newInteractible) return;
        _currentInteractible = newInteractible;
        CanvasManager.Instance.SetInteractionButton(_currentInteractible);
    }

    private Interactible GetNearestInteractible()
    {
        Interactible toReturn = null;
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, _detectionRange);
        if (cols.Length == 0) return null;

        float minDistance = float.MaxValue;
        for (int i = 0; i < cols.Length; i++)
        {
            Interactible interactible = cols[i].GetComponent<Interactible>();
            if (!interactible) continue;

            float currentDistance = Vector2.Distance(transform.position, interactible.transform.position);
            if (currentDistance < minDistance)
            {
                toReturn = interactible;
                minDistance = currentDistance;
            }
        }
        return toReturn;
    }

    private void OnDrawGizmos()
    {
        if(enabled)
        {
            Gizmos.DrawWireSphere(transform.position, _detectionRange);
        }
    } 
}