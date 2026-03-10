using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [HideInInspector] public Camera mainCamera;
    [SerializeField] Vector2 minSize = new (87, 87);
    [SerializeField] Vector2 maxSize = new (300, 300);

    private float range = Mathf.Infinity;
    private GameObject targetedObject;

    private void OnEnable()
    {
        mainCamera = Camera.main;
    }

    /// <summary>
    /// Returns the current crosshair size.
    /// </summary>
    /// <returns>The current crosshair size as a Vector2.</returns>
    public Vector2 GetSize()
    {
        return this.GetComponent<RectTransform>().sizeDelta;
    }

    /// <summary>
    /// Sets the current crosshair size.
    /// </summary>
    /// <param name="size">The new crosshair size as a Vector2.</param>
    /// <param name="time">The time it takes for the size to change, in seconds. Default is 0.1 seconds.</param>
    public void SetSize(Vector2 size, float time = 0.1f)
    {
        Vector2 initialSize = GetComponent<RectTransform>().sizeDelta;

        if (size.x > maxSize.x || size.y > maxSize.y)
        {
            size = new Vector2(maxSize.x, maxSize.y);
        }
        else if (size.x < minSize.x || size.y < minSize.y)
        {
            size = new Vector2(minSize.x, minSize.y);
        }

        Vector2 smoothedSize = Vector2.Lerp(initialSize, size, time);
        GetComponent<RectTransform>().sizeDelta = smoothedSize;
    }

    /// <summary>
    /// Multiplies the current crosshair size.
    /// </summary>
    /// <param name="multiplier">The factor by which to multiply the current crosshair size.</param>
    /// <param name="time">The time it takes for the size to change, in seconds. Default is 0.1 seconds.</param>
    public void MultiplySize(float multiplier, float time = 0.1f)
    {
        Vector2 initialSize = GetComponent<RectTransform>().sizeDelta;
        Vector2 newSize = new Vector2(initialSize.x * multiplier, initialSize.y * multiplier);
        if (newSize.x > maxSize.x || newSize.y > maxSize.y)
        {
            newSize = new Vector2(maxSize.x, maxSize.y);
        }
        else if (newSize.x < minSize.x || newSize.y < minSize.y)
        {
            newSize = new Vector2(minSize.x, minSize.y);
        }
        Vector2 smoothedSize = Vector2.Lerp(initialSize, newSize, time);
        GetComponent<RectTransform>().sizeDelta = smoothedSize;
    }

    /// <summary>
    /// Sets the current crosshair color.
    /// </summary>
    /// <param name="color">The new crosshair color as a Color.</param>
    /// <param name="time">The time it takes for the color to change, in seconds. Default is 0.1 seconds.</param>
    public void SetColor(Color color, float time = 0.1f)
    {
        Image[] myImages = GetComponentsInChildren<Image>();
        foreach (Image image in myImages)
        {
            Color initialColor = image.color;
            Color newColor = color;
            Color smoothedColor = Color.Lerp(initialColor, newColor, time);
            image.color = smoothedColor;
        }
    }

    /// <summary>
    /// Returns the GameObject that is currently targeted.
    /// </summary>
    /// <returns>The GameObject that is currently targeted.</returns>
    public GameObject GetTarget()
    {
        // Shoot a Raycast from the center of the screen.
        Vector3 rayDirection = new (Screen.width / 2, Screen.height / 2, 0);
        Ray ray = mainCamera.ScreenPointToRay(rayDirection);

        // If the rayCast hits within the range, return the target.
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            targetedObject = hit.transform.gameObject;
        }
        else
        {
            targetedObject = null;
        }

        return targetedObject;
    }

    /// <summary>
    /// Returns the point where the raycast hit.
    /// </summary>
    /// <returns>The point where the raycast hit.</returns>
    public Vector3 GetHitPoint()
    {
        // Shoot a Raycast from the center of the screen.
        RaycastHit hit;
        Vector3 rayDirection = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = mainCamera.ScreenPointToRay(rayDirection);
        // If the rayCast hits within the range, return the target.
        if (Physics.Raycast(ray, out hit, range))
        {
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    /// <summary>
    /// Sets the range of the raycast.
    /// </summary>
    /// <param name="range">The range of the raycast.</param>
    public void SetRange(int range)
    {
        this.range = range;
    }

    /// <summary>
    /// Gets the range of the raycast.
    /// </summary>
    /// <returns>The range of the raycast.</returns>
    public float GetRange()
    {
        return range;
    }
}
