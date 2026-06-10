using Unity.VisualScripting;
using UnityEngine;


public class LevelGrid : MonoBehaviour
{
    [SerializeField] private LevelCase _casePrefab;
    [SerializeField] private Vector2Int _size = new Vector2Int(4, 10);
    [SerializeField] private float _caseSize = 1;
    private LevelCase[,] _caseArray;

    private void Start()
    {
        BuildGrid();
    }

    public void BuildGrid()
    {
        _caseArray = new LevelCase[_size.x, _size.y];

        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                Vector2 casePosition = -new Vector2(
                    _caseSize * x,
                    _caseSize * y
                ) + (Vector2)transform.position;
                casePosition.x += _caseSize * (_size.x - 1) / 2f;


                Quaternion rot = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));
                _caseArray[x, y] = Instantiate(_casePrefab, casePosition, rot);
                _caseArray[x, y].transform.parent = transform;
            }
        }
    }


    private void OnDrawGizmos()
    {
        if (_caseArray == null) return;
        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                Gizmos.matrix = _caseArray[x, y].transform.localToWorldMatrix;
                Gizmos.DrawWireCube(Vector3.zero, Vector2.one * _caseSize);
            }
        }
    }
}
