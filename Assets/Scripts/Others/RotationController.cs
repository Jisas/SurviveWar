using UnityEngine;
using MyBox;

public enum RotationType { Normal, Orbit };
public enum RotationDirection { Clockwise, Counterclockwise };
public enum RotationAxis { X, Y, Z };

public class RotationController : MonoBehaviour
{
    public float rotationSpeed;
    [Space(10)]

    public RotationType rotationType;
    [ConditionalField(nameof(rotationType), false, RotationType.Orbit)] public GameObject target;
    public RotationDirection rotationOrientation;
    public RotationAxis rotationAxis;

    float orientation;

    void Update()
    {
        SetRotation();     
    }

    void SetRotation()
    {
        SetRotationDirection();

        if (rotationType == RotationType.Normal)
        {
            if (rotationAxis == RotationAxis.X)
            {
                transform.Rotate(orientation, 0, 0);
            }
            else if (rotationAxis == RotationAxis.Y)
            {
                transform.Rotate(0, orientation, 0);
            }
            else if (rotationAxis == RotationAxis.Z)
            {
                transform.Rotate(0, 0, orientation);
            }
        }
        else if (rotationType == RotationType.Orbit)
        {
            if (rotationAxis == RotationAxis.X)
            {
                transform.RotateAround(target.transform.position, Vector3.right, orientation);
            }
            else if (rotationAxis == RotationAxis.Y)
            {
                transform.RotateAround(target.transform.position, Vector3.up, orientation);
            }
            else if (rotationAxis == RotationAxis.Z)
            {
                transform.RotateAround(target.transform.position, Vector3.forward, orientation);
            }
        }
    }

    void SetRotationDirection()
    {
        if (rotationOrientation == RotationDirection.Clockwise)
        {
            orientation = Time.deltaTime * rotationSpeed;
        }
        else if (rotationOrientation == RotationDirection.Counterclockwise)
        {
            orientation = -Time.deltaTime * rotationSpeed;
        }
    }
}
