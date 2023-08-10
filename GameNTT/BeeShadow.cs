using UnityEngine;

namespace GameNTT
{
    // 벌 전용 그림자. 타겟이 등록되면 타겟의 위치를 따라 이동한다.
    public class BeeShadow : MonoBehaviour
    {
        public GameObject Target;

        [SerializeField] float offsetY = 0;

        void Update()
        {
            if (Target != null)
            {
                transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y + offsetY, Target.transform.position.z);
            }
        }
    }
}
