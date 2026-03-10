using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using static Enums;

[CreateAssetMenu(menuName = "Database / Weapons")]
public class DB_Weapons : ScriptableObject
{
    public List<Weapon> weapons = new();

    public Weapon FindWeapoInDatabase(int id)
    {
        foreach (Weapon weapon in weapons)
        {
            if (weapon.id == id)
            {
                return weapon;
            }
        }
        return null;
    }

    public Weapon FindWeapoInDatabase(string name)
    {
        foreach (Weapon weapon in weapons)
        {
            if (weapon.name == name)
            {
                return weapon;
            }
        }
        return null;
    }
}

[System.Serializable]
public class Weapon
{
    public string name;
    public int id;
    public bool equiped;
    [Space(10)]

    public GameObject weaponPrefab;
    public WeaponType weaponType;
    [Range(1f, 100f)] public float damage;

    [ConditionalField(
        nameof(weaponType), 
        false, 
        WeaponType.Gun, 
        WeaponType.Rifle, 
        WeaponType.Sniper)] public FireWeaponStats stats;

    public AudioClip soundSFX;

    [System.Serializable]
    public struct FireWeaponStats
    {
        public int maxAmmo;
        [Range(0.1f, 10.0f)] public float fireCadency;
        [Range(1f, 300f)] public float range;

        [Header("Spread")]
        [MinMaxRange(-0.5f, 0.5f)] public MinMaxFloat spreadX;
        [MinMaxRange(-0.5f, 0.5f)] public MinMaxFloat spreadY;

        [Header("Recoil")]
        public float recoilY;
        public float duration;
    }
}
