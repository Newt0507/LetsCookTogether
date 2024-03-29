using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    private const string IS_WALKING = "IsWalking";

    [SerializeField] private float _speed;
    [SerializeField] private Transform _holdingPoint;

    private Camera _mainCamera;
    private Animator _anim;
    private bool _isWalking;

    private Vector3 _targetPosition;
    private bool _isInteractingWithCuttingCounter;

    private KitchenObject kitchenObject;

    private AudioSource _audioSource;
    private float _footStepTimer;
    private float _footStepTimerMax = .1f;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _anim = gameObject.GetComponentInChildren<Animator>();
        _targetPosition = transform.position;
        _audioSource = gameObject.GetComponentInChildren<AudioSource>();
    }

    private void Update()
    {
        //GetInputByTouch();
        GetInputByClick();

        Animation();
        MovingSound();
    }

    private void FixedUpdate()
    {
        if(_isWalking) Move();
    }
/*
    private void GetInputByTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                GetInteractTarget(touch.position);
            }
        }
    }*/

    private void GetInputByClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetInteractTarget(Input.mousePosition);
        }
    }

    private void GetInteractTarget(Vector3 inputPosition)
    {
        Ray ray = _mainCamera.ScreenPointToRay(inputPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.TryGetComponent(out BaseCounter baseCounter))
                _targetPosition = new Vector3(baseCounter.GetReactPoint().position.x, transform.position.y, baseCounter.GetReactPoint().position.z);
            else
                _targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);

            _isWalking = true;
        }
    }

    private void Move()
    {
        transform.LookAt(_targetPosition);
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);

        InteractCounter();

        if (Vector3.Distance(transform.position, _targetPosition) < 0.1f)
            _isWalking = false;
    }

    private void InteractCounter()
    {
        Vector3 local = new Vector3(transform.position.x, transform.position.y + transform.lossyScale.y / 2, transform.position.z);

        float interactDistance = 1f;
        bool interact = false;

        if (Physics.Raycast(local, transform.forward, out RaycastHit hit, interactDistance))
        {
            if (hit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                _isWalking = false;

                if (GamePlayingManager.Instance.IsPlayingState())
                {
                    baseCounter.Interact(this);
                }

                if (baseCounter is CuttingCounter)
                    interact = true;                
            }
        }

        _isInteractingWithCuttingCounter = interact;
    }

    public bool IsInteractWithCuttingCounter()
    {
        return _isInteractingWithCuttingCounter;
    }

    private void Animation()
    {
        _anim.SetBool(IS_WALKING, _isWalking);
    }

    private void MovingSound()
    {
        _audioSource.volume = AudioManager.Instance.GetSFXVolume();
        _footStepTimer -= Time.deltaTime;
        if (_footStepTimer < 0f)
        {
            _footStepTimer = _footStepTimerMax;

            if (_isWalking)
                _audioSource.Play();
        }
    }

    public Transform GetSpawnPoint()
    {
        return _holdingPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if(kitchenObject!=null)
            AudioManager.Instance.PlaySFX(SoundEnum.PickUpSound);
    }

    public KitchenObject GetKitchenObject()
    {
        return this.kitchenObject;
    }

    public void ClearKitchenObject()
    {
        this.kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
