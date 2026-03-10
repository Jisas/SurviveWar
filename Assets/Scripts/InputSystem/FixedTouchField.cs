using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using MyBox;

public class FixedTouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [System.Serializable]
    public class Event : UnityEvent<Vector2> { }

    [Header("Config")]
    public float sensitivity;
    public float maxDivider;
    public float minDivider;
    [SerializeField] private bool invertXOutputValue;
    [SerializeField] private bool invertYOutputValue;

    [Header("Output")]
    public Event touchFieldOutputEvent;

    private float internalSensitivity;
    private readonly float threshold = 0.1f;
    private readonly float scaler = 1000;

    private void Start()
    {
        internalSensitivity = sensitivity / scaler;
    }

    public void OutputVectorValue(Vector2 outputValue)
    {
        touchFieldOutputEvent.Invoke(outputValue);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OutputVectorValue(Vector2.zero);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 deltaPos = eventData.delta;
        Vector2 outputPos = ApplyInversionFilter(deltaPos);

        if ((outputPos.magnitude * internalSensitivity) < threshold || (outputPos.magnitude * internalSensitivity) < -threshold)
        {
            OutputVectorValue(outputPos * (sensitivity / maxDivider));        
        }
        else
        {
            OutputVectorValue(outputPos * (sensitivity / minDivider));
        }
    }

    Vector2 ApplyInversionFilter(Vector2 position)
    {
        if (invertXOutputValue)
        {
            position.x = InvertValue(position.x);
        }

        if (invertYOutputValue)
        {
            position.y = InvertValue(position.y);
        }

        return position;
    }

    float InvertValue(float value)
    {
        return -value;
    }
}
