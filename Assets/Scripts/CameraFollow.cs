using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    [Range(1, 10)]
    public float smoothFactor;
    public Vector3 minValues, maxValues;

    private void FixedUpdate()
    {
        Follow();
    }

    void Follow()
    {
        //Camera follows the player
        Vector3 targetPosition = target.position + offset;

        //Verify if the targetPosition is out of bound or not
        //Limit it to the min and max values
        Vector3 boundsPosition = new Vector3
            (Mathf.Clamp(targetPosition.x, minValues.x, maxValues.x),
            Mathf.Clamp(targetPosition.y, minValues.y, maxValues.y),
            Mathf.Clamp(targetPosition.z, minValues.z, maxValues.z));

        Vector3 smoothPosition = Vector3.Lerp(transform.position, boundsPosition, smoothFactor * Time.fixedDeltaTime);
        transform.position = smoothPosition;
        
    }
}
