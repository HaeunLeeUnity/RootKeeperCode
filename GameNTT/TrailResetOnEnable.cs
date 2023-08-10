using UnityEngine;

// 오브젝트 풀에 있는 오브젝트가 다시 활성화 됐을 때 트레일이 길어지는 현상을 방지함.
public class TrailResetOnEnable : MonoBehaviour
{
    TrailRenderer _trailRenderer;

    void OnEnable()
    {
        if (_trailRenderer == null)
        {
            _trailRenderer = GetComponent<TrailRenderer>();
        }
    }

    private void OnDisable()
    {
        _trailRenderer.Clear();
    }
}
