using System.Collections;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] private float forwardForce;
    [SerializeField] private float upwardForce;

    private Rigidbody rb;
    private bool spacePressed;
    private Stopwatch timer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        timer = new Stopwatch();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb.isKinematic = true; 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spacePressed = true;
            timer.Start();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            spacePressed = false;
            timer.Stop();
        }
    }
    private void FixedUpdate()
    {
        if (spacePressed)
        {
            rb.isKinematic = false;
            float force = ((float)timer.Elapsed.TotalSeconds * forwardForce);
            rb.AddForce((Vector3.forward + (Vector3.up * upwardForce)) * force);
            Debug.Log($"Force to apply: {force}");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Box") || collision.gameObject.CompareTag("Floor"))
        {
           StartCoroutine(RestartLevelAfterSeconds(3f));
        }
    }

    private IEnumerator  RestartLevelAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
