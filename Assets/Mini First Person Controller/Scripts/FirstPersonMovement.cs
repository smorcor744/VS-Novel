using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5f;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9f;
    public KeyCode runningKey = KeyCode.LeftShift;

    Rigidbody rb;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();



    void Awake()
    {
        // Get the rigidbody on this and ensure it's present.
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogWarning("FirstPersonMovement: No Rigidbody found; adding one dynamically.");
            rb = gameObject.AddComponent<Rigidbody>();
        }
        // Prevent physics from rotating the character.
        rb.freezeRotation = true;
        rb.constraints |= RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        // Update IsRunning from input.
        IsRunning = canRun && Input.GetKey(runningKey);

        // Get targetMovingSpeed.
        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get targetVelocity from input.
        float horiz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        Vector3 localMove = new Vector3(horiz * targetMovingSpeed, rb.linearVelocity.y, vert * targetMovingSpeed);

        // Convert local to world using only Y rotation (ignore pitch).
        Quaternion yawOnly = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
        Vector3 worldVelocity = yawOnly * new Vector3(localMove.x, 0f, localMove.z);
        worldVelocity.y = rb.linearVelocity.y;

        // Apply movement.
        rb.linearVelocity = worldVelocity;
    }
}