using UnityEngine.Events;
using System.Collections;
using Cinemachine;
using UnityEngine;

namespace SurviveWar
{
    [RequireComponent(typeof(AudioSource))]
    public class WeaponController : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private GameObject spawnTarget;

        [Header("Weapon Components")]
        [SerializeField] private GameObject magazine;

        [Header("Event Components Needed")]
        [SerializeField] private Transform hand;

        [Header("Events")]
        public UnityEvent onStartShoot;
        public UnityEvent onEndShoot;

        // References
        private ObjectPooler objectPooler;
        private CinemachineImpulseSource cameraShake;
        private ThirdPersonController tpsController;
        private WeaponsManager weaponsManager;
        private EntityInputs _input;
        private AudioSource _audioSource;
        private Animator rigController;
        private AnimationEvents animationEvent;
        private GameObject rigManager;

        // Private parameters
        private bool isWait = false;
        private float time = 0;
        private GameObject magazineHand;


        private void Start()
        {
            rigManager = GameObject.FindGameObjectWithTag("RigLayers");
            objectPooler = FindObjectOfType<ObjectPooler>();
            animationEvent = rigManager.GetComponent<AnimationEvents>();
            rigController = rigManager.GetComponent<Animator>();
            tpsController = FindObjectOfType<ThirdPersonController>();
            weaponsManager = FindObjectOfType<WeaponsManager>();
            _input = FindObjectOfType<EntityInputs>();
            _audioSource = GetComponent<AudioSource>();
            cameraShake = GetComponent<CinemachineImpulseSource>();

            _audioSource.clip = weaponsManager.FindWeaponInEquipment(gameObject.name).sfxAudioClip;

            SetAnimationListeners();
        }

        private void SetAnimationListeners()
        {
            animationEvent.animationEvent.AddListener(OnAnimationEvents);
        }

        private void Update()
        {
            if (weaponsManager.currentWeapon.name == gameObject.name)
            {
                Shoot();

                // Recoil
                if (_input.Shoot && time > 0)
                {
                    _input.look.y -= weaponsManager.currentWeapon.recoilY / 1000 * Time.deltaTime / (weaponsManager.currentWeapon.duration / 10);
                    time -= Time.deltaTime;
                }

                Reload();
            }
        }

        private void Shoot()
        {
            if (_input.Shoot && weaponsManager.currentWeapon.currentAmmo > 0)
            {
                if (!isWait)
                {
                    isWait = true;

                    // Spread on shoot
                    var dir = (tpsController.dinamicTargetAim.transform.position - spawnTarget.transform.position).normalized;

                    dir += new Vector3(
                        Random.Range(weaponsManager.currentWeapon.spreadX.Min, weaponsManager.currentWeapon.spreadX.Max),
                        Random.Range(weaponsManager.currentWeapon.spreadY.Min, weaponsManager.currentWeapon.spreadY.Max),
                        0);

                    // Spawn bullet
                    objectPooler.SpawnFromPool("Projectile", spawnTarget.transform.position, Quaternion.LookRotation(dir));

                    // Recoil on shoot
                    Recoil(weaponsManager.currentWeapon.weaponType.ToString());

                    // Decrease ammo count
                    weaponsManager.DecreaseCurretAmmo();

                    onStartShoot.Invoke();

                    StartCoroutine(FireCadency(weaponsManager.currentWeapon.fireCadency));
                }
            }
            else
            {
                onEndShoot.Invoke();
            }
        }

        private IEnumerator FireCadency(float candencyLenght)
        {
            yield return new WaitForSeconds(candencyLenght);
            isWait = false;
        }

        private void Recoil(string WeaponTypeString)
        {
            time = weaponsManager.currentWeapon.duration;
            cameraShake.GenerateImpulse(Camera.main.transform.forward);

            rigController.Play($"Weapon_Recoil_{WeaponTypeString}", 1, 0.0f);
        }

        private void Reload()
        {
            if (_input.Reload)
            {
                if (weaponsManager.currentWeapon.currentAmmo < weaponsManager.currentWeapon.maxAmmo && weaponsManager.currentWeapon.weaponType != Enums.WeaponType.Knife)
                {
                    _input.Aim = false;
                    _input.Shoot = false;

                    rigController.SetTrigger("Reload");

                    //_input.reload = false;
                }
            }
            else
            {
                if (weaponsManager.currentWeapon.currentAmmo == 0 && weaponsManager.currentWeapon.weaponType != Enums.WeaponType.Knife)
                {
                    _input.Aim = false;
                    _input.Shoot = false;

                    rigController.SetTrigger("Reload");

                    //_input.reload = false;
                }
            }
        }

        // Animation Events
        void OnAnimationEvents(string eventName)
        {
            if (weaponsManager.currentWeapon.name == gameObject.name && !_input.Shoot)
            {
                switch (eventName)
                {
                    case "detach_magazine":
                        DetachMagazine();
                        break;

                    case "drop_magazine":
                        DropMagazine();
                        break;

                    case "refill_magazine":
                        RefillMagazine();
                        break;

                    case "attach_magazine":
                        AttachMagazine();
                        break;

                    case "update_magazine_ui":
                        UpdateMagazineUI();
                        break;
                }
            }
        }

        void DetachMagazine()
        {
            magazineHand = Instantiate(magazine, hand, true);
            magazine.SetActive(false);
        }

        void DropMagazine()
        {
            GameObject droppedMagazine = Instantiate(magazineHand, magazineHand.transform.position, magazineHand.transform.rotation);
            droppedMagazine.AddComponent<Rigidbody>();
            magazineHand.SetActive(false);
        }

        void RefillMagazine()
        {
            magazineHand.SetActive(true);
        }

        void AttachMagazine()
        {
            magazine.SetActive(true);
        }

        void UpdateMagazineUI()
        {
            _input.Reload = false;
            weaponsManager.IncreaseCurrentAmmo();
        }
    }
}