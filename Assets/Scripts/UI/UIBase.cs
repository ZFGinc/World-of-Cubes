using UnityEngine;

namespace ZFGinc.Assets.WorldOfCubes
{
    public abstract class UIBase : MonoBehaviour
    {
        [SerializeField] protected BlockSettings _blockSettings;

        public abstract void SetInfo(BlockInfo info);

        public abstract void SetInfo(BlockInfo info, Block block);

        private void Start()
        {
            Disable();
        }

        public void Enable()
        {
            if (this != null)
                gameObject.SetActive(true);
        }

        public void Disable()
        {
            if (this != null)
                gameObject.SetActive(false);
        }
    }
}