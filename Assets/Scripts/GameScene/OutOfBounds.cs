using UnityEngine;

namespace ZFGinc.WorldOfCubes
{
    [RequireComponent(typeof(BoxCollider))]
    public class OutOfBounds : MonoBehaviour
    {
        private GameButtons _buttons;

        public void Initialization(GameButtons buttons)
        {
            _buttons = buttons;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag != "Player") return;

            _buttons.Restart();
        }
    }
}

