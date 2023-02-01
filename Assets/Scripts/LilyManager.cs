using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LilyManager : MonoBehaviour, IService
{
    [Serializable]
    public struct MinMaxRange
    {
        public float Min;
        public float Max;
    }
    
    [SerializeField] private GameObject lilyPrefab;
    [SerializeField] private MinMaxRange lilyPadDistance;
    [SerializeField] private Transform frog;
    [SerializeField] private GameObject firstLily;

    private GameObject _previousLily;

    private void Awake()
    {
        _previousLily = firstLily;
    }

    public void CreateNextLily()
    {
        Vector3 direction = frog.forward;
        Vector3 newLilyPosition =
            _previousLily.transform.position + direction * Random.Range(lilyPadDistance.Min, lilyPadDistance.Max);
        _previousLily = Instantiate(lilyPrefab, newLilyPosition, Quaternion.identity);
        //todo: random lily size
    }
}
