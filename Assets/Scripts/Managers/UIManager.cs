using UnityEngine;
using TMPro;

namespace SurviveWar
{
    public class UIManager : MonoBehaviour
    {
        [Header("Player Output")]
        [SerializeField] private EntityInputs _input;
        [SerializeField] private ThirdPersonController _controller;

        [Header("Animations Output")]
        [SerializeField] private AnimationEvents finishedTextAnimEvents;
        [SerializeField] private AnimationEvents minigameDataAnimEvents;

        [Header("General UI References")]
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI ammoText;
        [SerializeField] private GameObject aimCrosshairUI;
        [SerializeField] private GameObject normalCrosshairUI;
        [SerializeField] private GameObject canvasMobile;
        public GameObject aimButton;
        public GameObject shootAndLookButton;
        public GameObject shootButton;
        public GameObject interactButton;

        [Header("Minigame UI References")]
        public GameObject canvasPopup;
        public TextMeshProUGUI firstElementText;
        public TextMeshProUGUI secondElementText;
        public TextMeshProUGUI thirdElementText;
        public TextMeshProUGUI fourthElementText;
        public TextMeshProUGUI fifthElementText;

        [Header("Altar UI References")]
        public TextMeshProUGUI dummysAmmountText;
        public TextMeshProUGUI targetsAmmountText;
        public TextMeshProUGUI headShootText;
        public TextMeshProUGUI bodyshootText;
        public TextMeshProUGUI totalPointsText;


        private WeaponsManager weaponsManager;
        private ShootMinigameManager minigameManager;

        private void Start()
        {
            weaponsManager = FindObjectOfType<WeaponsManager>();  
            minigameManager = FindObjectOfType<ShootMinigameManager>();

            SetAnimationListeners();
        }

        private void SetAnimationListeners()
        {
            finishedTextAnimEvents.animationEvent.AddListener(OnFinishedTextAnimationEvents);
            minigameDataAnimEvents.animationEvent.AddListener(OnMinigameDataAnimationEvents);
        }

        void Update()
        {
            if (weaponsManager != null)
            {
                if (weaponsManager.currentWeapon.weaponType != Enums.WeaponType.Knife)
                    ammoText.text = $"{weaponsManager.currentWeapon.currentAmmo}/{weaponsManager.currentWeapon.maxAmmo}";
                else
                    ammoText.text = "---";

                if (!weaponsManager.currentWeapon.inUse && !_controller.isOnPc)
                {
                    aimCrosshairUI.SetActive(false);
                    aimButton.SetActive(false);
                    shootButton.SetActive(false);
                    shootAndLookButton.SetActive(false);
                }
                else
                {
                    if (!_controller.isOnPc)
                    {
                        aimButton.SetActive(true);
                        shootButton.SetActive(true);
                        shootAndLookButton.SetActive(true);
                    }

                    if (_input.Aim)
                    {
                        normalCrosshairUI.SetActive(false);
                        aimCrosshairUI.SetActive(true);
                    }
                    else
                    {
                        normalCrosshairUI.SetActive(true);
                        aimCrosshairUI.SetActive(false);
                    }
                }
            }
        }

        public void SetLastMinigameInfo(string LGDummyAmount, string LGTargetAmount, string LGHeadShoots, string LGBodyShoots, string LGTotalPoints)
        {
            // Last game info
            dummysAmmountText.text = LGDummyAmount;
            targetsAmmountText.text = LGTargetAmount;
            headShootText.text = LGHeadShoots;
            bodyshootText.text = LGBodyShoots;
            totalPointsText.text = LGTotalPoints;
        }

        // Encapsulation
        public GameObject GetInteractButton()
        {
            return interactButton;
        }

        // Animation Events
        void OnFinishedTextAnimationEvents(string eventName)
        {
            switch (eventName)
            {
                case "open_popup":
                    OpenPopup();
                    break;
            }
        }

        void OpenPopup()
        {
            canvasPopup.SetActive(true);
            canvasMobile.SetActive(false);
        }

        void OnMinigameDataAnimationEvents(string eventName)
        {
            switch (eventName)
            {
                case "set_first_element_text":
                    SetElementText(firstElementText, minigameManager.targetsDestroyAmmount);
                    break;

                case "set_second_element_text":
                    SetElementText(secondElementText, minigameManager.dummysDestroyAmmount);
                    break;

                case "set_third_element_text":
                    SetElementText(thirdElementText, minigameManager.bodyShootAmmount);
                    break;

                case "set_fourth_element_text":
                    SetElementText(fourthElementText, minigameManager.headShootAmmount);
                    break;

                case "set_fifth_element_text":
                    SetElementText(fifthElementText, minigameManager.totalPointsAmmount);
                    break;
            }
        }

        void SetElementText(TextMeshProUGUI textMesh, int amount)
        {
            textMesh.text = amount.ToString();
        }
    }
}
