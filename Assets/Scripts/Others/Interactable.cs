using UnityEngine.Events;
using UnityEngine;
using UnityEditor;

namespace SurviveWar
{
    public class Interactable : MonoBehaviour
    {
        [Header("Editor Visualization")]
        [SerializeField] private float range;
        [SerializeField] private Vector3 offset;

        [Header("Events")]
        [SerializeField] private UnityEvent interactEvents;

        private UIManager uiManager;
        private EntityInputs _input;
        private GameObject player;
        private float distance;

        void Start()
        {
            uiManager = FindObjectOfType<UIManager>();
            player = GameObject.FindWithTag("Player");
            _input = player.GetComponent<EntityInputs>();
        }

        void Update()
        {
            distance = Vector3.Distance(player.transform.position, transform.position);

            if (distance < range)
            {
                uiManager.GetInteractButton().SetActive(true);

                if (_input.Interact)
                {
                    interactEvents.Invoke();
                    _input.Interact = false;
                }
                else
                    CancelInvoke(nameof(interactEvents));
            }
            else
            {
                uiManager.GetInteractButton().SetActive(false);
            }
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Handles.color = Color.blue;
            Handles.DrawWireDisc(transform.position + offset, Vector3.up, range);
        }
#endif
    }
}