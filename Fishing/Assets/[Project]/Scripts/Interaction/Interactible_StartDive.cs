using UnityEngine;

public class Interactible_StartDive : Interactible
{
    [SerializeField] private FishingSequence _fishingSequence;

    public override void Interact()
    {
        base.Interact();
        _fishingSequence.StartFishSequence();
    }
}
