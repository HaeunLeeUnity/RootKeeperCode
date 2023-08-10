using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;

// 게임 도움말을 구현.
public class Tips : MonoBehaviour
{
    [SerializeField] Text titleText;
    [SerializeField] Text contentsText;
    [SerializeField] GameObject[] tipsImages;

    [SerializeField] Button nextButton;
    [SerializeField] Button prevButton;


    int tipNumber = 0;

    // 활성화 될 때 페이지를 처음 장으로 변경.
    private void OnEnable()
    {
        tipNumber = 0;
        TipInitalize();
    }

    // 페이지에 맞는 설명과 이미지를 표시. 설명은 번역됨.
    void TipInitalize()
    {

        var LocalizedTipTitle = new LocalizedString("New Table", $"TipTitle{tipNumber + 1}_Key");
        var LocalizedTip = new LocalizedString("New Table", $"Tip{tipNumber + 1}_Key");

        titleText.text = LocalizedTipTitle.GetLocalizedString();
        contentsText.text = LocalizedTip.GetLocalizedString();

        prevButton.interactable = true;
        nextButton.interactable = true;


        if (tipNumber == 0)
        {
            prevButton.interactable = false;
        }
        if (tipNumber == 5)
        {
            nextButton.interactable = false;
        }
        foreach (var tipImage in tipsImages)
        {
            tipImage.SetActive(false);
        }

        tipsImages[tipNumber].SetActive(true);
    }

    public void NextPage()
    {
        tipNumber++;
        TipInitalize();
    }

    public void prevPage()
    {
        tipNumber--;
        TipInitalize();
    }



}
