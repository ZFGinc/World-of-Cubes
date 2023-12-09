using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ZFGinc.Assets.WorldOfCubes.Bootstrap
{
    [RequireComponent(typeof(AudioSource))]
    public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
    {
        [Header("Звук при наведении")]
        [SerializeField] private bool _onHoverSound;
        [SerializeField] private AudioClip _hoverSound;
        [Space]
        [Header("Звук при нажатии")]
        [SerializeField] private bool _onPressSound;
        [SerializeField] private AudioClip _pressSound;

        private AudioSource _audioSource;
        private AudioSource _buttonClicks;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>(); 
            _buttonClicks = GameObject.FindWithTag("ButtonClicks").GetComponent<AudioSource>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_onHoverSound) return;
            _audioSource.PlayOneShot(_hoverSound);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_onPressSound) return;
            _buttonClicks.PlayOneShot(_pressSound);
        }
    }
}