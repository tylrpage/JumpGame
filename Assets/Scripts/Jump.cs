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
    
    private bool _charging;
    private float _chargeAmount;
    
    private void Update()
    {
        if (_charging)
        {
            _chargeAmount += Time.deltaTime * chargeSpeed;
            _chargeAmount = Mathf.Min(maxChargeAmount, _chargeAmount);
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && !_charging)
        {
            _charging = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space) && _charging)
        {
            _charging = false;
            
            // jump
            Vector3 start = transform.position;
            Vector3 end = transform.position + transform.forward * _chargeAmount;
            StartCoroutine(JumpRoutine(start, end));
            
            // reset for next jump
            _chargeAmount = 0;
        }
    }

    private IEnumerator JumpRoutine(Vector3 start, Vector3 end)
    {
        Vector3 middle = Vector3.Lerp(start, end, jumpApexLocation) + Vector3.up * jumpHeight;
        for (float t = 0; t < 1; t += Time.deltaTime / jumpDuration)
        {
            transform.position = EvaluateBezier(start, middle, end, t);
            yield return null;
        }
    }

    private Vector3 EvaluateBezier(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 x = Vector3.Lerp(a, b, t);
        Vector3 y = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(x, y, t);
    }
}
