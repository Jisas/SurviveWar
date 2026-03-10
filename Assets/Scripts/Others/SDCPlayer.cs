using System.Collections.Generic;
using UnityEngine;
using System;

namespace SurviveWar
{
    public class SDCPlayer : MonoBehaviour
    {
        public Crosshair crosshair;
        public Camera _camera;

        [Header("General")]
        public List<int> targetLayersID;

        [Header("Crosshair Settings")]
        public CrosshairConfig targetedCrosshair;
        public CrosshairConfig defaultCrosshair;

        [Header("Debug")]
        public Vector2 currentCrosshairSize;
        public GameObject currentTargetedObject;

        private WeaponsManager weaponsManager;
        private GameObject player;
        private float distance;

        private void Start()
        {
            crosshair.mainCamera = _camera;

            weaponsManager = FindObjectOfType<WeaponsManager>();
            player = GameObject.FindGameObjectWithTag("Player");
        }

        public void Update()
        {
            var target = crosshair.GetTarget();

            if (target != null)
            {
                distance = Vector3.Distance(player.transform.position, target.transform.position);

                for (int i = 0; i < targetLayersID.Count; i++)
                {
                    if (distance <= weaponsManager.currentWeapon.range && target.layer == targetLayersID[i])
                    {
                        currentTargetedObject = target;


                        crosshair.SetSize(targetedCrosshair.crosshairSize, targetedCrosshair.smoothSpeed);
                        crosshair.SetColor(targetedCrosshair.crosshairColor, targetedCrosshair.smoothSpeed);
                    }
                    else if (distance > weaponsManager.currentWeapon.range || target.layer != targetLayersID[i])
                    {
                        currentTargetedObject = null;
                        crosshair.SetSize(defaultCrosshair.crosshairSize, defaultCrosshair.smoothSpeed);
                        crosshair.SetColor(defaultCrosshair.crosshairColor, defaultCrosshair.smoothSpeed);
                    }
                }
            
            }
      
            currentCrosshairSize = crosshair.GetSize();
        }
    }

    [Serializable]
    public class CrosshairConfig
    {
        public Vector2 crosshairSize = new(100, 100);
        public Color crosshairColor;
        public float smoothSpeed = 0.1f;
    }
}

