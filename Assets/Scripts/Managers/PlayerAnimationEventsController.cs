using UnityEngine;

namespace SurviveWar
{
    public class PlayerAnimationEventsController : MonoBehaviour
    {
        [Header("Controller")]
        //[SerializeField] private ThirdPersonController _tpsController;
        [SerializeField] private CharacterController _controller;

        [Header("Audio")] //--
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;
        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;


        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
    }
}