using UnityEngine;
using TMPro;

namespace SurviveWar
{
    public class PointsController : MonoBehaviour
    {
        public TextMeshProUGUI pointsText;
        private int points = 0;

        private void Update()
        {
            pointsText.text = $"Points:{points.ToString()}";
        }

        public void AddPoint(int ammount)
        {
            points += ammount;
            ShootMinigameManager.Instance.totalPointsAmmount = points;
        }

        public void ResetPoints() => points = 0;
    }
}