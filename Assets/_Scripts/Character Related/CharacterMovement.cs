using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float dragThreshold = 0.04f;
    [SerializeField] private LayerMask groundLayerMask;
    private CharAttrSO charAttrSO;
    private float runSpeed, horSpeed, charSize; // These variables must initialized from Character script
    private float horDir, rayDistance = 1f;
    private bool isDragging, runEnabled;
    private Vector3 startPosition, targetPosition;
    private Animator animator;
    private Rigidbody rb;
    private Character character;
    private GameManager.GameState curGameState;

    //event gelecek buraya statelerle alakali

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        character = GetComponent<Character>(); // get runspeed horspeed charsize

        if (!animator)
            Debug.Log(this.name + " missing Animator component");
        if (!rb)
            Debug.Log(this.name + " missing Rigidbody component");
        if (!character)
            Debug.Log(this.name + " missing Character script component");

        charAttrSO = character.GetCharAttributes();

        runSpeed = charAttrSO.runSpeed;
        horSpeed = charAttrSO.horSpeed;
        charSize = charAttrSO.charsize;

        curGameState = GameManager.Instance.GetGameState();
    }

    void Update()
    {
        if (GameManager.Instance.GetGameState() == GameManager.GameState.Run)
            JoystickLogic();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.GetGameState() == GameManager.GameState.Run)
            Move();
    }

    void Move()
    {
        if (runEnabled)
        {
            if (isDragging)
            {
                bool[] feetOnGround = Helper.CheckSides(transform, charSize, rayDistance, groundLayerMask);

                if (horDir > 0 && feetOnGround[0]) // Right
                {
                    horDir = 1;
                }
                else if (horDir < 0 && feetOnGround[1]) // Left
                {
                    horDir = -1;
                }
                else
                {
                    horDir = 0;
                }
            }

            if (rb)
                rb.MovePosition(new Vector3(transform.position.x + horSpeed * horDir * Time.fixedDeltaTime, transform.position.y, transform.position.z + runSpeed * Time.fixedDeltaTime));
        }
    }



    void JoystickLogic()
    {
        if (Input.GetMouseButtonDown(0)) // Initiating touch
        {
            startPosition = Input.mousePosition;
            targetPosition = startPosition;

            runEnabled = true;

            if (animator)
                animator.SetBool("Running", true);
        }
        else if (Input.GetMouseButton(0)) // While holding touch
        {
            targetPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0)) // Holding up touch
        {
            runEnabled = false;
            startPosition = targetPosition;

            if (animator)
                animator.SetBool("Running", false);
        }

        if (runEnabled) 
            CalcDrag();
    }

    private void CalcDrag() // Detects drag using the dragThreshold
    {
        Vector3 WPSP = Camera.main.ScreenToWorldPoint(startPosition + new Vector3(0, 0, 1));
        Vector3 WPTP = Camera.main.ScreenToWorldPoint(targetPosition + new Vector3(0, 0, 1));

        horDir = WPTP.x - WPSP.x;

        //Debug.Log(horDir);
        isDragging = Mathf.Abs(horDir) >= dragThreshold ? true : false;
    }
}
