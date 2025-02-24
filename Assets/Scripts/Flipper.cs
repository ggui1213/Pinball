using UnityEngine;

public class Flipper : MonoBehaviour
{
    public float rotationAngle = 25f;
    public bool rotationDirection = true;
    public KeyCode activationKey = KeyCode.Space;
    public float angleThreshold = 0.1f;
    public float rotationSpeed = 200f;

    private HingeJoint2D hinge;
    private float initialAngle;

    void Awake()
    {
        hinge = GetComponent<HingeJoint2D>();
        
        initialAngle = transform.eulerAngles.z;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.rotation = initialAngle;
        }
        
        JointAngleLimits2D limits = hinge.limits;
        
        if (rotationDirection)
        {
            limits.min = 0f;
            limits.max = rotationAngle;
        }
        else
        {
            limits.min = -rotationAngle;
            limits.max = 0f;
        }
        hinge.limits = limits;
        hinge.useLimits = true;
    }

    void FixedUpdate()
    {
        float targetRelativeAngle = Input.GetKey(activationKey) ? (rotationDirection ? rotationAngle : -rotationAngle) : 0f;
        float currentRelativeAngle = hinge.jointAngle;
        float angleDifference = targetRelativeAngle - currentRelativeAngle;
        
        if (Mathf.Abs(angleDifference) < angleThreshold)
        {
            JointMotor2D motor = hinge.motor;
            motor.motorSpeed = 0f;
            hinge.motor = motor;
            return;
        }
        
        JointMotor2D newMotor = hinge.motor;
        newMotor.motorSpeed = Mathf.Sign(angleDifference) * rotationSpeed;
        hinge.motor = newMotor;
    }
}
