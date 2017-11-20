
using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public Transform target;

    public float targetHeight = 2f;
    public float distance = 10.0f;

    public float maxDistance = 20;
    public float minDistance = .6f;

    public float xSpeed = 250.0f;
    public float ySpeed = 120.0f;

    public int yMinLimit = -80;
    public int yMaxLimit = 80;

    public int zoomRate = 40;

    public float rotationDampening = 3.0f;
    public float zoomDampening = 5.0f;

    public float RotationAboutY = 0;
    public float RotationAboutX = 26;
    public float currentDistance;
    public float desiredDistance;
    public float correctedDistance;

    void Start()
    {
        //Vector3 angles = transform.eulerAngles;
        //RotationAboutY = 0;
        //RotationAboutX = 26;

        currentDistance = distance;
        desiredDistance = distance;
        correctedDistance = distance;

        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;
    }

    /**
     * Camera logic on LateUpdate to only update after all character movement logic has been handled.
     */
    void LateUpdate()
    {
        // Don't do anything if target is not defined
        if (!target)
            return;

        // If either mouse buttons are down, let the mouse govern camera position
        if (Input.GetMouseButton(0) ^ Input.GetMouseButton(1))
        {
            RotationAboutY += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            RotationAboutX -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
        }

        //otherwise, ease behind the target if any of the directional keys are pressed
        else if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0 ||(Input.GetMouseButton(0) && Input.GetMouseButton(1)))
        {
            float targetRotationAngle = target.eulerAngles.y;
            float currentRotationAngle = transform.eulerAngles.y;
            RotationAboutY = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, rotationDampening * Time.deltaTime);
            //RotationAboutX = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, rotationDampening * Time.deltaTime);

            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                RotationAboutX -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            }

            if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0 && (!Input.GetMouseButton(0) && Input.GetMouseButton(1)))
            {
                target.Rotate(0, 0 - RotationAboutY, 0);
            }
        }

        RotationAboutX = ClampAngle(RotationAboutX, yMinLimit, yMaxLimit);

        // set camera rotation
        Quaternion rotation = Quaternion.Euler(RotationAboutX, RotationAboutY, 0);

        // calculate the desired distance
        desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
        correctedDistance = desiredDistance;

        // calculate desired camera position
        Vector3 position = target.position - (rotation * Vector3.forward * desiredDistance + new Vector3(0, -targetHeight, 0));

        // check for collision using the true target's desired registration point as set by user using height
        RaycastHit collisionHit;
        Vector3 trueTargetPosition = new Vector3(target.position.x, target.position.y + targetHeight, target.position.z);

        // if there was a collision, correct the camera position and calculate the corrected distance
        bool isCorrected = false;
        if (Physics.Linecast(trueTargetPosition, position, out collisionHit))
        {
            position = collisionHit.point;
            correctedDistance = Vector3.Distance(trueTargetPosition, position);
            isCorrected = true;
        }

        // For smoothing, lerp distance only if either distance wasn't corrected, or correctedDistance is more than currentDistance
        currentDistance = !isCorrected || correctedDistance > currentDistance ? Mathf.Lerp(currentDistance, correctedDistance, Time.deltaTime * zoomDampening) : correctedDistance;

        // recalculate position based on the new currentDistance
        position = target.position - (rotation * Vector3.forward * currentDistance + new Vector3(0, -targetHeight, 0));

        transform.rotation = rotation;
        transform.position = position;
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
