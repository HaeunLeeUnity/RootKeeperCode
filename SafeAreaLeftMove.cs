using UnityEngine;

// 노치 때문에 결과 화면이 잘리는 것을 방지하기 위해 safeArea 왼쪽 지점까지 UI를 옮기는 컴포넌트.
public class SafeAreaLeftMove : MonoBehaviour
{
    void Start()
    {
        var rect = GetComponent<RectTransform>();
        rect.position = new Vector2(rect.position.x + Screen.safeArea.xMin, rect.position.y);
    }

}
