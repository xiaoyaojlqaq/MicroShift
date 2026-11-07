using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControl : MonoBehaviour
{
    [SerializeField, Min(0f)] private float runSpeed = 10f;
    [SerializeField, Min(0f)] private float jumpForce = 5f;
    [SerializeField, Min(0f)] private float strengthJumpForce = 10f;
    [SerializeField]private float currJumpForce;

    [SerializeField, Range(0.01f, 2f)] private float bodyShapeShiftScale = 0.5f;
    [SerializeField, Range(0.01f, 2f)] private float bodyShapeNumShiftScale = 0.5f;
    float currBodyShapeNumShiftScale;
    public Transform canvas;

    [Header("Shape Shift")]
    public int maxShapeShiftTime;
    float currShapeShiftTime;
    public Image shapeShiftTimeBar;
    public Image shapeShiftTimeImage;

    [FormerlySerializedAs("collider2D")]
    [SerializeField, Tooltip("Collider used for ground checks (usually the foot collider).")]
    private Collider2D groundCollider;
    [SerializeField] private ParticleSystem moveEffect;
    [SerializeField] private ParticleSystem jumpEffect;

    private Rigidbody2D body;
    private InputManager inputManager;
    private Vector3 initialScale;
    [SerializeField]private float facingDirection;
    private int groundLayers;
    private Vector2 pendingMovement;



    public bool isInGround;
    private bool isBodyShifted;
    private bool jumpRequested;
    private bool inputsBound;
    private bool isStrengthJump;
    private bool canMove;

    //time space shift
    [SerializeField]private Vector2 originPos;
    [SerializeField]private int originScene;

    [SerializeField]private Vector2 shiftPos;
    [SerializeField]private int shiftScene;

    bool isInOriginScene;
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        initialScale = transform.localScale;
        isInOriginScene = true;
        facingDirection = Mathf.Sign(initialScale.x);
        if (Mathf.Approximately(facingDirection, 0f))
        {
            facingDirection = 1f;
        }

        if (groundCollider == null)
        {
            groundCollider = GetComponentInChildren<CapsuleCollider2D>();
        }

        if (groundCollider == null)
        {
            groundCollider = GetComponentInChildren<Collider2D>();
        }

        if (moveEffect == null)
        {
            moveEffect = GetComponentInChildren<ParticleSystem>();
        }

        groundLayers = LayerMask.GetMask("Ground");
    }

    private void Start()
    {
        canMove = true;
        currJumpForce = jumpForce;
        currShapeShiftTime = maxShapeShiftTime;
        BindInput();
        if (inputManager == null)
        {
            Debug.LogError("InputManager instance was not found in the scene.", this);
            enabled = false;
        }
    }
/*
    private void OnEnable()
    {
        BindInput();
    }
*/
    private void BindInput()
    {
        if (inputsBound)
        {
            return;
        }

        inputManager = InputManager.Instance;
        if (inputManager == null)
        {
            return;
        }

        inputManager.MovePerformed += Move;
        inputManager.MoveCanceled += MoveEnd;
        inputManager.BodyShapeShiftPerformed += BodyShapeShiftPerformed;
        inputManager.BodyShapeShiftCanceled += BodyShapeShiftCanceled;
        //inputManager.BodyShapeShiftPerformed += BodyShapeShiftPerformed;
        inputManager.Time_spaceShiftPerformed += Time_spaceShift;
        inputManager.JumpPerformed += JumpPerformed;
        inputManager.JumpCanceled += JumpCanleled;
        inputManager.JumpStarted += JumpStarted;
        inputsBound = true;
    }
    private void Update()
    {
        ShapeShiftTimeReStore();
        BodyShapeShiftUpdated();

    }

    private void FixedUpdate()
    {
        if (canMove) body.velocity = new Vector2(pendingMovement.x * runSpeed * currBodyShapeNumShiftScale, body.velocity.y);
        else
        {
            body.velocity = new Vector2(0, body.velocity.y);
            StopMoveEffect();
        }

        if (jumpRequested && isInGround)
        {
            jumpRequested = false;
            body.AddForce(Vector2.up * currJumpForce * currBodyShapeNumShiftScale, ForceMode2D.Impulse);
        }
        else if (isStrengthJump && isInGround)
        {
            isStrengthJump = false;
            body.AddForce(Vector2.up * currJumpForce * currBodyShapeNumShiftScale, ForceMode2D.Impulse);
        }
        else
        {
            jumpRequested = false;
            isStrengthJump = false;
        }
    }

    private void LateUpdate()
    {
        canvas.position = transform.position;
    }
    /*
    private void OnDisable()
    {
        if (inputsBound && inputManager != null)
        {
            inputManager.MovePerformed -= Move;
            inputManager.MoveCanceled -= MoveEnd;
            inputManager.BodyShapeShiftStarted -= BodyShapeShift;
        }

        inputsBound = false;
        pendingMovement = Vector2.zero;
        jumpRequested = false;
        StopMoveEffect();
    }
    */
    private void Move(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();

        if (Mathf.Abs(movement.x) > 0.01f)
        {
            if (isInGround) PlayMoveEffect();
            else StopMoveEffect();
            facingDirection = Mathf.Sign(movement.x);
            ApplyScale();
        }

        pendingMovement = movement;
            
    }

    private void MoveEnd(InputAction.CallbackContext context)
    {
        pendingMovement = Vector2.zero;
        StopMoveEffect();
    }

    private void JumpStarted(InputAction.CallbackContext context)
    {
        //if (context.interaction is TapInteraction)
        //{
        //    currJumpForce = jumpForce;
        //    jumpRequested = true;
        //}
        //if(context.duration < 0.5)
        //{
        //    Debug.Log("started");
        //    jumpRequested = true;
        //}
    }
    private void JumpPerformed(InputAction.CallbackContext context)
    {
        if (context.interaction is TapInteraction)
        {
            //Debug.Log("JumpPerformed");
            //currJumpForce = jumpForce;
            //if (context.duration < 0.5)
            //{
            //    Debug.Log("0.5");
            //    jumpRequested = true;
            //}
            //else if (context.duration < 5 && context.duration >= 0.5)
            //{
            //    currJumpForce += (float)context.duration * 3;
            //    currJumpForce = Mathf.Clamp(currJumpForce, jumpForce, strengthJumpForce);
            //    isStrengthJump = true;
            //    jumpEffect.Stop();
            //}


            Debug.Log("JumpPerformed");
            currJumpForce = jumpForce;
            jumpRequested = true;

        }
        if(context.interaction is HoldInteraction)
        {
            currJumpForce = jumpForce;
            currJumpForce += (float)context.duration * 3;
            currJumpForce = Mathf.Clamp(currJumpForce, jumpForce, strengthJumpForce);
            isStrengthJump = true;
            jumpEffect.Stop();
            canMove = true;
        }
    }

    void JumpCanleled(InputAction.CallbackContext context)
    {
        if (context.interaction is HoldInteraction)
        {
            if (context.duration <= 5 && context.duration >= 0.5)
            {
                currJumpForce = jumpForce;
                currJumpForce += (float)context.duration * 3;
                currJumpForce = Mathf.Clamp(currJumpForce, jumpForce, strengthJumpForce);
                isStrengthJump = true;
                jumpEffect.Stop();
                canMove = true;
            }
        }
        if (context.interaction is TapInteraction)
        {
            jumpEffect.Play();
            canMove = false;
        }

    }


    private void BodyShapeShiftPerformed(InputAction.CallbackContext context)
    {
        shapeShiftTimeBar.gameObject.SetActive(true);
        isBodyShifted = true;
        ApplyScale();
    }
    private void BodyShapeShiftCanceled(InputAction.CallbackContext context)
    {
        isBodyShifted = false;
        ApplyScale();
    }
    private void BodyShapeShiftUpdated()
    {
        if (isBodyShifted && currShapeShiftTime > 0)
        {
            currShapeShiftTime -= Time.deltaTime;
            currShapeShiftTime = Mathf.Clamp(currShapeShiftTime, 0, maxShapeShiftTime);
        }
        if (currShapeShiftTime <= 0)
        {
            isBodyShifted = false;
            ApplyScale();
        }
    }

    void ShapeShiftTimeReStore()
    {
        if (!isBodyShifted && currShapeShiftTime < maxShapeShiftTime)
        {
            currShapeShiftTime += Time.deltaTime;
            currShapeShiftTime = Mathf.Clamp(currShapeShiftTime, 0, maxShapeShiftTime);
        }
        else if(!isBodyShifted && currShapeShiftTime >= maxShapeShiftTime)
        {
            shapeShiftTimeBar.gameObject.SetActive(false);
        }
        shapeShiftTimeImage.fillAmount = currShapeShiftTime / maxShapeShiftTime;
    }

    private void ApplyScale()
    {
        currBodyShapeNumShiftScale = isBodyShifted ? bodyShapeNumShiftScale : 1f;
        float shapeScale = isBodyShifted ? bodyShapeShiftScale : 1f;
        transform.localScale = new Vector3(
            Mathf.Abs(initialScale.x) * facingDirection * shapeScale,
            initialScale.y * shapeScale,
            initialScale.z);
    }

    private void PlayMoveEffect()
    {
        if (moveEffect != null)
        {
            moveEffect.Play();
        }
    }

    private void StopMoveEffect()
    {
        if (moveEffect != null)
        {
            moveEffect.Stop();
        }
    }

    public void Time_spaceShift(InputAction.CallbackContext ctx) 
    {
        Time_spaceShift(shiftScene, shiftPos);
    }
    public void Time_spaceShift(int shiftScene, Vector2 shiftPosition)
    {
        if (isInOriginScene)
        {
            originPos = new Vector2(transform.position.x, transform.position.y);
            originScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(shiftScene);
            transform.position = shiftPosition;
        }
        else
        {
            shiftPos = new Vector2(transform.position.x, transform.position.y);
            this.shiftScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(originScene);
            transform.position = originPos;
        }
        isInOriginScene = !isInOriginScene;
    }
    private void OnValidate()
    {
        // Keep a zero or negative shape scale from making the player disappear in play mode.
        if (bodyShapeShiftScale <= 0f)
        {
            bodyShapeShiftScale = 0.01f;
        }
    }
}
