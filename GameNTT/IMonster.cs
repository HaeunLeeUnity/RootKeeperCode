using UnityEngine;
using UnityEngine.Pool;

namespace GameNTT
{
    public interface IMonster
    {
        // 몬스터의 인터페이스.
        bool IsKnockBack
        {
            set;
        }

        bool IsAlive
        {
            get;
            set;
        }

        int NowHP
        {
            get;
            set;
        }

        int RewordAmount
        {
            get;
        }


        GameObject GameObjectProperty
        {
            get;
        }

        void Reset();

        void Initalize(int monsterType);

        IObjectPool<IMonster> Pool { get; set; }
    }

}