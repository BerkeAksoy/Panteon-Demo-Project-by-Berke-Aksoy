using UnityEngine;

public class ShiningObs : MonoBehaviour
{
    [SerializeField] private float offset = 0.1f;
    [SerializeField] [Range(0f, 2f)] private float horSpeed = 1f;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private bool stableVelocity;
    private Rigidbody rb;
    private int direction = 1;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!rb)
            Debug.LogWarning("Rigidbody is missing");

        rb.velocity = new Vector3(horSpeed * direction, rb.velocity.y, rb.velocity.z);
    }

    private void FixedUpdate()
    {
        bool[] sidesOnGround = Helper.CheckSides(transform, offset, 10f, groundLayerMask);

       
        if(direction == 1 && !sidesOnGround[0])
        {
            ChangeDir();
        }
        else if(direction == -1 && !sidesOnGround[1])
        {
            ChangeDir();
        }

        if(stableVelocity)
            rb.velocity = new Vector3(horSpeed * direction, rb.velocity.y, rb.velocity.z);
    }

    private void ChangeDir()
    {
        direction *= -1;

        if(!stableVelocity)
            rb.velocity = new Vector3(horSpeed * direction, rb.velocity.y, rb.velocity.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Character>())
        {
            var mainModule = ps.main;
            Color randomColor = new Color(Random.value, Random.value, Random.value);

            mainModule.startColor = randomColor;
        }
    }
}
