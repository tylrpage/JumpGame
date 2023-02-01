using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour, IService
{
    [SerializeField] private GameObject frog;

    private Vector3 _offsetFromFrog;
    private Vector3 _targetPosition;
    private float _t;
    private Vector3 _startPosition; // Where we lerp from

    private void Awake()
    {
        _startPosition = transform.position;
        _targetPosition = transform.position;
        _offsetFromFrog = transform.position - frog.transform.position;
    }

    public void ReFocus()
    {
        _t = 0;
        _startPosition = transform.position;
        _targetPosition = frog.transform.position + _offsetFromFrog;
    }

    private void Update()
    {
        _t += Time.deltaTime;
        transform.position = Vector3.Lerp(_startPosition, _targetPosition, _t);
    }
}
