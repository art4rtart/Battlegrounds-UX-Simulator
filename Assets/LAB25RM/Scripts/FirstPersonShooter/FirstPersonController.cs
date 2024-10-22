﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public static FirstPersonController Instance
    {
        get
        {
            if (instance != null)
                return instance;
            instance = FindObjectOfType<FirstPersonController>();
            return instance;
        }
    }
    private static FirstPersonController instance;

    Camera cam;
    CharacterController characterController;

    [Header("Move")]
    [SerializeField] bool isWalking = false;
	public float walkSpeed = 0;
	public float runSpeed = 0;
	public float moveSpeed = 1f;
	[SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
    private Vector2 input;

    [Header("Mouse")]
    public Mouse mouseManager = new Mouse();

    [Header("FovKick")]
    [SerializeField] private bool useFovKick;
    [SerializeField] private FOVKick fovKick = new FOVKick();

    [Header("Bob")]
    [SerializeField] private bool useHeadBob;
    [SerializeField] private CurveControlledBob headBob = new CurveControlledBob();
    [SerializeField] private LerpControlledBob jumpBob = new LerpControlledBob();
    private Vector3 originalCameraPos;

    //[Header("FootStep")]
    private CollisionFlags m_CollisionFlags;
    [SerializeField] private float m_GravityMultiplier;
    private bool jumping = false;
    private bool jump = false;
    [SerializeField] private float jumpSpeed = 0f;
    [SerializeField] private float m_StickToGroundForce;
    bool m_PreviouslyGrounded;
    private Vector3 moveDir = Vector3.zero;

    [Header("RagDoll")]
    public Transform ragDoll;
    public Transform ragDollHead;

    [HideInInspector] public Transform target;

	[Header("AudioClips")]
	AudioSource audioSource;
	private float m_StepCycle;
	private float m_NextStep;
	private bool m_Jumping;
	[SerializeField] private float m_StepInterval;
	[SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
	[SerializeField] private AudioClip jumpSound;           // the sound played when character leaves the ground.
	[SerializeField] private AudioClip landSound;           // the sound played when character touches back on ground.

	void Start()
    {
        cam = Camera.main;
        characterController = GetComponent<CharacterController>();
		audioSource = GetComponent<AudioSource>();


		fovKick.Setup(cam);
        originalCameraPos = cam.transform.localPosition;
        mouseManager.Init(this.transform, cam.transform);
    }

    void Update()
    {
        if (GameTimeController.isPaused || isDead) return;

        RotateView();

        if (!jump) jump = Input.GetButtonDown("Jump");

        if (!characterController) return;

        if (!m_PreviouslyGrounded && characterController.isGrounded)
        {
            StartCoroutine(jumpBob.DoBobCycle());
            PlayLandingSound();
            moveDir.y = 0f;
            jumping = false;
        }

        if (!characterController.isGrounded && !jumping && m_PreviouslyGrounded) moveDir.y = 0f;

        m_PreviouslyGrounded = characterController.isGrounded;
    }

    void FixedUpdate()
    {
        if (characterController == null || isDead) return;

        float speed;
        MoveInput(out speed);
        Vector3 desiredMove = transform.forward * input.y + transform.right * input.x;

        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out hitInfo,
                           characterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        moveDir.x = desiredMove.x * speed;
        moveDir.z = desiredMove.z * speed;

        if (characterController.isGrounded)
        {
            moveDir.y = -m_StickToGroundForce;

            if (jump)
            {
                moveDir.y = jumpSpeed;
                PlayJumpSound();
                jump = false;
                jumping = true;
            }
        }
        else
        {
            moveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
        }

        m_CollisionFlags = characterController.Move(moveDir * Time.fixedDeltaTime);

        ProgressStepCycle(speed, new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
        UpdateCameraPosition(speed);
        mouseManager.UpdateCursorLock();
    }

    private void MoveInput(out float speed)
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        bool waswalking = isWalking;
        isWalking = !Input.GetKey(KeyCode.LeftShift);

        speed = isWalking || WeaponController.Instance.isReloading ? walkSpeed : runSpeed;
        input = new Vector2(horizontal, vertical);

        if (input.sqrMagnitude > 1) input.Normalize();

        if (isWalking != waswalking && useFovKick && characterController.velocity.sqrMagnitude > 0)
        {
            StopAllCoroutines();
            StartCoroutine(!isWalking ? fovKick.FOVKickUp() : fovKick.FOVKickDown());
        }
    }

    private void UpdateCameraPosition(float speed)
    {
        Vector3 newCameraPosition;
        if (!useHeadBob)
        {
            return;
        }
        if (characterController.velocity.magnitude > 0 && characterController.isGrounded)
        {
            cam.transform.localPosition =
                headBob.DoHeadBob(characterController.velocity.magnitude +
                                  (speed * (isWalking ? 1f : m_RunstepLenghten)));
            newCameraPosition = cam.transform.localPosition;
            newCameraPosition.y = cam.transform.localPosition.y - jumpBob.Offset();
        }
        else
        {
            newCameraPosition = cam.transform.localPosition;
            newCameraPosition.y = originalCameraPos.y - jumpBob.Offset();
        }
        cam.transform.localPosition = newCameraPosition;
    }

	private void ProgressStepCycle(float speed, Vector2 m_Input)
	{
		if (characterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
		{
			m_StepCycle += (characterController.velocity.magnitude + (speed * (isWalking ? 1f : m_RunstepLenghten))) *
						 Time.fixedDeltaTime;
		}
		if (!(m_StepCycle > m_NextStep))
		{
			return;
		}

		m_NextStep = m_StepCycle + m_StepInterval;

		PlayFootStepAudio();
	}

	void RotateView()
    {
        mouseManager.LookRotation(this.transform, cam.transform);
    }

    public void Die()
    {
        StartCoroutine(DieEffect());
		PlayerInfoManager.Instance.PlayerDied();
	}

    public Material DieVignette;

    public bool isDead = false;
    public float turnSpeed = 500f;

    IEnumerator DieEffect()
    {
        float time = 0;
        float radius = -2.75f;
        Color color = DieVignette.GetColor("_MainColor");

        // got caught by zombie
        isDead = true;
        Vector3 dirToLookTarget = (target.transform.position - this.transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

        float playerEulerAngleY = this.transform.eulerAngles.y > 0 ? this.transform.eulerAngles.y : 360 + this.transform.eulerAngles.y;
        float targetEulerAngleY = target.transform.eulerAngles.y > 0 ? target.transform.eulerAngles.y : 360 + target.transform.eulerAngles.y;

        if (playerEulerAngleY > Mathf.Clamp(targetEulerAngleY -200f, 0f, 180f) && playerEulerAngleY < targetEulerAngleY + 200f)
        {
            while (Mathf.Abs(Mathf.DeltaAngle(this.transform.eulerAngles.y, targetAngle)) > 0.05f)
            {
                turnSpeed -= Time.deltaTime;
                float angle = Mathf.MoveTowardsAngle(this.transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
                this.transform.eulerAngles = Vector3.up * angle;
                yield return null;
            }
        }

        //while (Mathf.Abs(Mathf.DeltaAngle(this.transform.eulerAngles.y, targetAngle)) > 0.05f)
        //{
        //    turnSpeed -= Time.deltaTime;
        //    float angle = Mathf.MoveTowardsAngle(this.transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
        //    this.transform.eulerAngles = Vector3.up * angle;
        //    yield return null;
        //}

        // holster true
        WeaponController.Instance.weaponAnimator.SetBool("Holster", true);
        if(WeaponController.Instance.isZooming) WeaponController.Instance.Zoom();

        yield return new WaitForSeconds(.85f);

        while (time < 2) {
            CameraShakeController.Instance.CameraShake();
            DieVignette.SetFloat("_HitHardness",  Mathf.Lerp(radius, -3.5f, time));
            DieVignette.SetColor("_MainColor", Color.Lerp(color, Color.red, time * 3f));
            time += Time.deltaTime;
            yield return null;
        }

		PauseMenuController.Instance.gameObject.SetActive(false);

        // Rag Doll active
        ragDoll.transform.position = this.transform.position;
        ragDoll.gameObject.SetActive(true);
        Camera.main.transform.SetParent(ragDollHead);
        Camera.main.transform.localPosition = Vector3.zero ;
        Camera.main.transform.localRotation = Quaternion.identity;
        Camera.main.transform.LookAt(target);
    }

	private void PlayFootStepAudio()
	{
		if (!characterController.isGrounded)
		{
			return;
		}
		// pick & play a random footstep sound from the array,
		// excluding sound at index 0
		int n = Random.Range(1, m_FootstepSounds.Length);
		audioSource.clip = m_FootstepSounds[n];
		audioSource.PlayOneShot(audioSource.clip);
		// move picked sound to index 0 so it's not picked next time
		m_FootstepSounds[n] = m_FootstepSounds[0];
		m_FootstepSounds[0] = audioSource.clip;
	}

	private void PlayJumpSound()
	{
		audioSource.clip = jumpSound;
		audioSource.Play();
	}

	private void PlayLandingSound()
	{
		audioSource.clip = landSound;
		audioSource.Play();
	}


	public void LockHeadMovement()
    {
        mouseManager.XSensitivity = 0f;
        mouseManager.YSensitivity = 0f;
	}


    public void UnLockHeadMovement()
    {
        mouseManager.XSensitivity = 0.75f;
        mouseManager.YSensitivity = 0.75f;
    }

	private void OnEnable()
	{
		audioSource = GetComponent<AudioSource>();
		audioSource.enabled = true;
	}

	private void OnDisable()
    {
        if (!DieVignette) return;
        DieVignette.SetFloat("_HitHardness", 0);
        DieVignette.SetColor("_MainColor", Color.black);
    }
}
