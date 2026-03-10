using System.Collections.Generic;
using UnityEngine;
using System;

namespace SurviveWar
{
    public class ProjectileController : MonoBehaviour
    {
        [Header("References")]
        public TrailRenderer trail;

        [Header("Values")]
        public float speed = 50f;
        public LayerMask GroundLayers;
        public float radius;
        public float offset;

        [Header("Damage Requiered Values")]
        [SerializeField] private int headLayerID;
        [SerializeField] private int bodyLayerID;
        [SerializeField] private int targetLayerID;

        [Header("Text Damage")]
        [SerializeField] private Vector3 textOffset;

        [Header("Hits VFX")]
        public List<HitVFX> vfxList = new();

        private ObjectPooler objectPooler;
        private Rigidbody rb;
        private WeaponsManager weaponsManager;
        private EntityInputs _input;
        private HealthManager healthManager;
        private Vector3 startPos;
        private bool Grounded = true;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            objectPooler = FindObjectOfType<ObjectPooler>();
            weaponsManager = FindObjectOfType<WeaponsManager>();
            _input = FindObjectOfType<EntityInputs>();
        }

        private void Update()
        {
            if (_input.Shoot)
            {
                startPos = GameObject.FindGameObjectWithTag("Target/ProjectileTarget").transform.position;
                var distance = Vector3.Distance(transform.position, startPos);

                if (distance >= weaponsManager.currentWeapon.range)
                {
                    rb.velocity = Vector3.zero;
                    trail.emitting = false;
                    this.gameObject.SetActive(false);
                }
            }
        }

        void FixedUpdate()
        {        
            if (GroundedCheck())
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                rb.velocity = transform.forward * speed;
                trail.emitting = true;
            }
        }

        private bool GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 capsuleStart = new (transform.position.x, transform.position.y - offset, transform.position.z);

            Grounded = Physics.CheckSphere(capsuleStart, radius, GroundLayers, QueryTriggerInteraction.Ignore);
            return Grounded;
        }

        private void OnCollisionEnter(Collision coll)
        {
            Vector3 pos;
            var contact = coll.contacts[0];
            var obj = coll.gameObject;
            var rot = Quaternion.FromToRotation(Vector3.up, contact.normal);

            foreach (var item in vfxList)
            {
                if (obj.layer == 21)
                {
                    pos = new(contact.point.x - 0.9f, contact.point.y, contact.point.z - 0.7f);
                }
                else
                {
                    pos = contact.point;
                }

                if (obj.layer == item.layerID)
                {               
                    objectPooler.SpawnParticlesFromPool(item.vfxName, pos, rot, true);
                }
            }

            try
            {
                healthManager = obj.transform.parent.GetComponent<HealthManager>();
            }
            catch (Exception)
            {
                healthManager = null;
                throw;
            }

            if (healthManager != null)
            {
                if (obj.layer == headLayerID)
                {
                    healthManager.TakeHeadShoot(false);
                    //healthManager.TakeHeadShoot(pos, textOffset);
                }
                else if (obj.layer == bodyLayerID)
                {
                    healthManager.TakeBodyDamage(weaponsManager.currentWeapon.damage, false);
                    //healthManager.TakeBodyDamage(weaponsManager.currentWeapon.damage, pos, textOffset);
                }
                else if (obj.layer == targetLayerID)
                {
                    healthManager.TargetTakeDamage(weaponsManager.currentWeapon.damage, true);
                    //healthManager.TakeBodyDamage(weaponsManager.currentWeapon.damage, pos, textOffset);
                }
            }
        }

    #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new (0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new (1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - offset, transform.position.z),
                radius);
        }
    #endif
    }

    [Serializable]
    public class HitVFX
    {
        [Tooltip("The name must be the same as the one placed in the Object Pool.")]
        public string vfxName;
        [Tooltip("The layer must also be selected in the \"Ground Layers\" entry.")]
        public int layerID;
    }
}
