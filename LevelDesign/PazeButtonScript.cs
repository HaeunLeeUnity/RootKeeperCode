using UnityEngine;

namespace LevelDesign
{
    // 레벨 디자이너 페이지를 변경.
    public class PageButtonScript : MonoBehaviour
    {
        public int pageOffset = 0;

        public void MoveToPage()
        {
            LevelDesigner.instance.MovePage(pageOffset);
        }
    }
}