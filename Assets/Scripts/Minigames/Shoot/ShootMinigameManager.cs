using System.Collections;
using UnityEngine.Events;
using UnityEngine;
using MyBox;

namespace SurviveWar
{
    public class ShootMinigameManager : MonoBehaviour
    {
        public static ShootMinigameManager Instance;

        [Header("References")]
        [SerializeField] private Timer timerController;
        [SerializeField] private UIManager uIManager;

        [Header("Stop Event")]
        public UnityEvent OnStop;

        [Header("Debug")]
        [ReadOnly] public int targetsDestroyAmmount;
        [ReadOnly] public int dummysDestroyAmmount;
        [ReadOnly] public int bodyShootAmmount;
        [ReadOnly] public int headShootAmmount;
        [ReadOnly] public int totalPointsAmmount;

        private float currentTime;
        private readonly float stopTime = 0.0f;
        private string jsonString;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(Instance);
        }

        public void StartAwaiting()
        {
            currentTime = timerController.GetCurrentTime();
            StartCoroutine(AwaitMethod(currentTime));
        }
        private IEnumerator AwaitMethod(float time)
        {
            while (time > stopTime)
            {
                time = timerController.GetCurrentTime();
                yield return null;
            }

            OnStop.Invoke();
            StopCoroutine(nameof(AwaitMethod));
        }
        private struct DataWrapper
        {
            public int targetsDestroyAmmount;
            public int dummysDestroyAmmount;
            public int bodyShootAmmount;
            public int headShootAmmount;
            public int totalPointsAmmount;
        }
        public void SaveMinigameData()
        {
            DataWrapper dataWrapper = new()
            {
                targetsDestroyAmmount = this.targetsDestroyAmmount,
                dummysDestroyAmmount = this.dummysDestroyAmmount,
                bodyShootAmmount = this.bodyShootAmmount,
                headShootAmmount= this.headShootAmmount,
                totalPointsAmmount = this.totalPointsAmmount,
            };
        
            jsonString = JsonUtility.ToJson(dataWrapper);
            PlayerPrefs.SetString("MinigameData", jsonString);
        }
        public void LoadMinigameData()
        {
            if (PlayerPrefs.HasKey("MinigameData"))
                LoadSavedMinigameData();

            else LoadEmptyMinigameData();
        }
        private void LoadEmptyMinigameData()
        {
            this.targetsDestroyAmmount = 0;
            this.dummysDestroyAmmount = 0;
            this.bodyShootAmmount = 0;
            this.headShootAmmount = 0;
            this.totalPointsAmmount = 0;
        }
        private void LoadSavedMinigameData()
        {
            jsonString = PlayerPrefs.GetString("MinigameData");
            DataWrapper dataWrapper = JsonUtility.FromJson<DataWrapper>(jsonString);

            this.targetsDestroyAmmount = dataWrapper.targetsDestroyAmmount;
            this.dummysDestroyAmmount = dataWrapper.dummysDestroyAmmount;
            this.bodyShootAmmount = dataWrapper.bodyShootAmmount;
            this.headShootAmmount = dataWrapper.headShootAmmount;
            this.totalPointsAmmount = dataWrapper.totalPointsAmmount;

            SavedDataToUIText();
        }
        private void SavedDataToUIText()
        {
            uIManager.SetLastMinigameInfo
            (
                this.dummysDestroyAmmount.ToString(),
                this.targetsDestroyAmmount.ToString(),
                this.headShootAmmount.ToString(),
                this.bodyShootAmmount.ToString(),
                this.totalPointsAmmount.ToString()
            );
        }
    }
}
