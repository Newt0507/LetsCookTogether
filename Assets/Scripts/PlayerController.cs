using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";

    [SerializeField] private float _speed;

    private Camera _mainCamera;
    private Animator _anim;
    private bool _isWalking;
    private Vector3 _targetPosition;
    private Transform _targetTransform;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _anim = gameObject.GetComponentInChildren<Animator>();
        _targetPosition = transform.position;
    }

    private void Update()
    {
        GetInputByTouch();
        GetInputByClick();
        Animation();
    }

    private void FixedUpdate()
    {
        if(_isWalking) Move();
    }

    private void GetInputByTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                GetTargetPosition(touch.position);
            }
        }
    }

    private void GetInputByClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetTargetPosition(Input.mousePosition);
        }
    }

    private void GetTargetPosition(Vector3 inputPosition)
    {
        Ray ray = _mainCamera.ScreenPointToRay(inputPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            _targetTransform = hit.transform;
            _targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            _isWalking = true;
        }
    }

    private void Move()
    {
        transform.LookAt(_targetPosition);
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _targetPosition) < 0.1f)
            _isWalking = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == _targetTransform)
        {
            Debug.Log(collision.transform);
            _isWalking = false;
        }
    }

    private void Animation()
    {
        _anim.SetBool(IS_WALKING, _isWalking);
    }
}
