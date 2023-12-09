using UnityEngine;

namespace ZFGinc.Assets.WorldOfCubes
{
    public interface IContactable
    {
        public AudioSource AudioSource { get; set; }
        public void Contact(bool state);
    }
}