using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{

    public float speed;

    public float minDirection = 0.5f;

    public GameObject sparksVFX;

    private Vector3 direction;
    private Rigidbody rb;

    private bool stopped = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // method 1
        //transform.position += direction * speed * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (stopped)
            return;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        bool hit = false;


        if(other.CompareTag("Wall"))
        {
            direction.z = -direction.z;
            hit = true;
        }

        if (other.CompareTag("Racket"))
        {
            Vector3 newDirection = (transform.position - other.transform.position).normalized;

            newDirection.x = Mathf.Sign(newDirection.x) * Mathf.Max(Mathf.Abs(newDirection.x), minDirection);
            newDirection.z = Mathf.Sign(newDirection.z) * Mathf.Max(Mathf.Abs(newDirection.z), minDirection);

            direction = newDirection;
            hit = true;
        }

        if(hit)
        {
            GameObject sparks = Instantiate(sparksVFX, transform.position, transform.rotation);
            Destroy(sparks, 4f);
        }
    }

    private void ChooseDirection()
    {
        float signX = Mathf.Sign(Random.Range(-1f, 1f));
        float signZ = Mathf.Sign(Random.Range(-1f, 1f));
        this.direction = new Vector3(0.5f * signX, 0, 0.5f * signZ);
    }

    public void Stop()
    {
        stopped = true;
    }

    public void Go()
    {
        ChooseDirection();
        stopped = false;
    }
}
