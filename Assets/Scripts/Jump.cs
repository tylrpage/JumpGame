using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpDuration;
    [SerializeField] private float maxChargeAmount;
    [SerializeField] [Range(0,1)] private float jumpApexLocation;
    [SerializeField] private float allowedJumpError;
    
    private bool _charging;
    private float _chargeAmount;
    private bool _isJumping;
    private LilyManager _lilyManager;
    private CameraManager _cameraManager;
    private float _previousJumpError; // positive means we jumped too far

    public float ChargeAmountNorm => _chargeAmount / maxChargeAmount;

    private void Awake()
    {
        _lilyManager = ServiceLocator.Instance.GetService<LilyManager>();
        _lilyManager.CreateNextLily();
        
        _cameraManager = ServiceLocator.Instance.GetService<CameraManager>();
    }

    private void Update()
    {
        if (_charging)
        {
            _chargeAmount += Time.deltaTime * chargeSpeed;
            _chargeAmount = Mathf.Min(maxChargeAmount, _chargeAmount);
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && !_charging && !_isJumping)
        {
            _charging = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space) && _charging)
        {
            _charging = false;
            
            // jump
            Vector3 start = transform.position;
            Vector3 end = transform.position + transform.forward * _chargeAmount;
            // Factor our error from jumping onto our current lily when calculating how far we are off jumping to the next
            _previousJumpError = _previousJumpError + _chargeAmount - _lilyManager.NextLilyDistance;
            bool madeJump = Mathf.Abs(_previousJumpError) < allowedJumpError;
            StartCoroutine(JumpRoutine(start, end, madeJump));

            // reset for next jump
            _chargeAmount = 0;
        }
    }

    private IEnumerator JumpRoutine(Vector3 start, Vector3 end, bool madeJump)
    {
        _isJumping = true;
        Vector3 middle = Vector3.Lerp(start, end, jumpApexLocation) + Vector3.up * jumpHeight;
        for (float t = 0; t < 1; t += Time.deltaTime / jumpDuration)
        {
            transform.position = EvaluateBezier(start, middle, end, t);
            yield return null;
        }

        _isJumping = false;
        
        if (madeJump)
        {
            _lilyManager.CreateNextLily();
            _cameraManager.ReFocus();
        }
    }

    private Vector3 EvaluateBezier(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 x = Vector3.Lerp(a, b, t);
        Vector3 y = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(x, y, t);
    }
}
