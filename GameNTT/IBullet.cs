using UnityEngine;
using UnityEngine.Pool;

namespace GameNTT
{
    // 플레이어가 발사하는 총알의 인터페이스.
    public interface IBullet
    {
        void Initalize(IObjectPool<IBullet> bulletPool);

        GameObject GameObjectProperty
        {
            get;
        }

    }
}
