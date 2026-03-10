using UnityEngine;

public class GameManager : MonoBehaviour
{
    //[Header("Resolution")]
    [SerializeField] private int width;
    [SerializeField] private int height;

    void Start()
    {
        //Screen.SetResolution(width, height, true);
        //Application.targetFrameRate = Screen.currentResolution.refreshRate;
        QualitySettings.vSyncCount = 0;
    }
}
