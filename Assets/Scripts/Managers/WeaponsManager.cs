using System.Collections.Generic;
using System.Collections;
using static Enums;
using UnityEngine;
using MyBox;

namespace SurviveWar
{
    public class WeaponsManager : MonoBehaviour
    {
        [SerializeField] private DB_Weapons weaponDB;
        [SerializeField] private ThirdPersonController tpsController;
        [SerializeField] private Animator rigController;
        [Space(10)]

        public EquipedWeapon currentWeapon;
        [Space(10)]

        [SerializeField] private List<EquipedWeapon> equipedWeapons;

        private void Awake()
        {
            SetEquipedWeaponsList();
        }

        private void Start()
        {
            currentWeapon.currentAmmo = currentWeapon.maxAmmo;
        }

        public void SetEquipedWeaponsList()
        {
            equipedWeapons = new List<EquipedWeapon>();

            for (int i = 0; i < weaponDB.weapons.Count; i++)
            {
                var weapon = weaponDB.FindWeapoInDatabase(i);

                if (weapon.equiped == true)
                {
                    var newWeapon = new EquipedWeapon
                    {
                        id = weapon.id,
                        name = weapon.name,
                        weaponPrefab = weapon.weaponPrefab,
                        weaponType = weapon.weaponType,
                        currentAmmo = weapon.stats.maxAmmo,
                        maxAmmo = weapon.stats.maxAmmo,
                        fireCadency = weapon.stats.fireCadency,
                        range = weapon.stats.range,
                        damage = weapon.damage,
                        spreadX = weapon.stats.spreadX,
                        spreadY = weapon.stats.spreadY,
                        recoilY = weapon.stats.recoilY,
                        duration = weapon.stats.duration,
                        sfxAudioClip = weapon.soundSFX
                    };

                    equipedWeapons.Add(newWeapon);                
                }
            }
        }

        public int GetCurrentWeaponId()
        {
            return currentWeapon.id;
        }

        public void SetCurrentWeapon(int id)
        {
            var weapon = FindWeaponInEquipment(id);
            currentWeapon = weapon;

            for (int i = 0; i < weaponDB.weapons.Count; i++)
            {
                if (i == currentWeapon.id)
                {
                    FindWeaponInEquipment(i).inUse = true;
                }
                else
                {
                    FindWeaponInEquipment(i).inUse = false;
                }
            }
        }

        public void SetKnifeAsCurrentWeapon()
        {
            if (!currentWeapon.inUse)
            {
                var weapon = FindWeaponInEquipment(WeaponType.Knife);
                SetCurrentWeapon(weapon.id);

                StartCoroutine(ActiveWeapon(WeaponType.Knife));
            }
            else
            {
                StartCoroutine(SwitchWeapon(WeaponType.Knife));
            }
        }

        public void SetGunAsCurrentWeapon()
        {
            if (!currentWeapon.inUse)
            {
                var weapon = FindWeaponInEquipment(WeaponType.Gun);
                SetCurrentWeapon(weapon.id);

                StartCoroutine(ActiveWeapon(WeaponType.Gun));
            }
            else
            {
                StartCoroutine(SwitchWeapon(WeaponType.Gun));
            }       
        }

        public void SetRifleAsCurrentWeapon()
        {
            if (!currentWeapon.inUse)
            {
                var weapon = FindWeaponInEquipment(WeaponType.Rifle);
                SetCurrentWeapon(weapon.id);

                StartCoroutine(ActiveWeapon(WeaponType.Rifle));
            }
            else
            {
                StartCoroutine(SwitchWeapon(WeaponType.Rifle));
            }
        }

        private IEnumerator SwitchWeapon(WeaponType type)
        {
            yield return StartCoroutine(HolsterWeapon());
            yield return new WaitForSeconds(0.1f);
            yield return StartCoroutine(ActiveWeapon(type));
        }

        private IEnumerator HolsterWeapon()
        {
            rigController.SetBool("HolsterWeapon", true);
            rigController.Play($"Equip_{currentWeapon.weaponType}");

            do
            {
                yield return new WaitForEndOfFrame();

            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }

        private IEnumerator ActiveWeapon(WeaponType type)
        {
            var weapon = FindWeaponInEquipment(type);
            SetCurrentWeapon(weapon.id);

            rigController.SetBool("HolsterWeapon", false);
            rigController.Play($"Equip_{currentWeapon.weaponType}");

            do
            {
                yield return new WaitForEndOfFrame();

            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }

        public void DecreaseCurretAmmo()
        {
            currentWeapon.currentAmmo--;

            if (currentWeapon.currentAmmo < 0)
                currentWeapon.currentAmmo = 0;
        }

        public void IncreaseCurrentAmmo()
        {
            currentWeapon.currentAmmo += currentWeapon.maxAmmo - currentWeapon.currentAmmo;

            if (currentWeapon.currentAmmo > currentWeapon.maxAmmo)
                currentWeapon.currentAmmo = currentWeapon.maxAmmo;
        }

        public EquipedWeapon FindWeaponInEquipment(int id)
        {
            foreach (EquipedWeapon weapon in equipedWeapons)
            {
                if (weapon.id == id)
                {
                    return weapon;
                }
            }
            return null;
        }

        public EquipedWeapon FindWeaponInEquipment(string name)
        {
            foreach (EquipedWeapon weapon in equipedWeapons)
            {
                if (weapon.name == name)
                {
                    return weapon;
                }
            }
            return null;
        }

        public EquipedWeapon FindWeaponInEquipment(WeaponType type)
        {
            foreach (EquipedWeapon weapon in equipedWeapons)
            {
                if (weapon.weaponType == type)
                {                
                    return weapon;
                }
            }
            return null;
        }

        // ------------- Context Menus --------------
        [ContextMenu("SetWeapon/Rifle")]
        void SetRifle()
        {
            SetRifleAsCurrentWeapon();
        }

        [ContextMenu("SetWeapon/Gun")]
        void SetGun()
        {
            SetGunAsCurrentWeapon();
        }

        [ContextMenu("SetWeapon/Knife")]
        void SetKnife()
        {
            SetKnifeAsCurrentWeapon();
        }
    }

    [System.Serializable]
    public class EquipedWeapon
    {
        public string name;
        public int id;
        public bool inUse = false;
        [Space(10)]

        public GameObject weaponPrefab;
        public WeaponType weaponType;
        public int maxAmmo;
        public int currentAmmo;
        [Range(0.1f, 10.0f)] public float fireCadency = 0.1f;
        [Range(1f, 300f)] public float range = 100f;
        [Range(1f, 100f)] public float damage = 10f;

        [Header("Spread")]
        [MinMaxRange(-0.5f, 0.5f)] public MinMaxFloat spreadX;
        [MinMaxRange(-0.5f, 0.5f)] public MinMaxFloat spreadY;

        [Header("Recoil")]
        public float recoilY;
        public float duration;

        [Space(10)]
        public AudioClip sfxAudioClip;
    }
}
