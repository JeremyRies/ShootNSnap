using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private SpriteRenderer _inputBaseHandle;
    [SerializeField] private SpriteRenderer _inputPullHandle;

    [SerializeField] private SpriteRenderer _back;
    [SerializeField] private SpriteRenderer _front;
    
    [SerializeField] private Rigidbody2D _backRb;
    [SerializeField] private Rigidbody2D _frontRb;

    [SerializeField] private Transform _rotationTransform;
    [SerializeField] private SpriteRenderer _arrow;

    private bool _inputIsAllowed = true;

    private void Setup()
    {
    }

    private void Awake()
    {
        _back.transform.SetParent(_rotationTransform);
        _front.transform.SetParent(_rotationTransform);

        _frontRb.bodyType = RigidbodyType2D.Static;
        _backRb.bodyType = RigidbodyType2D.Static;
    }

    private void Update()
    {
        var mousePos = Input.mousePosition;
        mousePos = _camera.ScreenToWorldPoint(mousePos);

        if (Input.GetKeyDown(KeyCode.Mouse0) && _inputIsAllowed)
        {
            _inputBaseHandle.transform.position = new Vector3(mousePos.x, mousePos.y);
        }

        if (Input.GetKey(KeyCode.Mouse0) && _inputIsAllowed)
        {
            _inputPullHandle.transform.position = new Vector3(mousePos.x, mousePos.y);

            var angle = CalculateAngle(_inputBaseHandle.transform.position, _inputPullHandle.transform.position);
            _rotationTransform.rotation = Quaternion.Euler(0, 0, angle+180);
            _arrow.transform.localScale = new Vector3(1, (_inputBaseHandle.transform.position - _inputPullHandle.transform.position).magnitude);
            Debug.Log(angle);
        }
    }
    
    public static float CalculateAngle(Vector3 from, Vector3 to) {
 
        return Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;
 
    }
}