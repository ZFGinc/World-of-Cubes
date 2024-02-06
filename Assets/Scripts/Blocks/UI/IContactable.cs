using UnityEngine;

namespace ZFGinc.WorldOfCubes
{
    public interface IContactable
    {
        public AudioSource AudioSource { get; set; }
        public void Contact(bool state);
    }
}