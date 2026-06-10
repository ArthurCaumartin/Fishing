using System.Collections;
using UnityEngine;

public class TargetFrameRate : MonoBehaviour
{
    [SerializeField] private int _target = 60;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(5);
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = _target;
    }
}