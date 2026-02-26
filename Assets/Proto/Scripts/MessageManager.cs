using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Diagnostics;

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance;

    [SerializeField] private CanvasGroup keyAppearedImage;

    [SerializeField] private CanvasGroup leversLeftGroup;
    [SerializeField] private TextMeshProUGUI leversLeftText;

    [SerializeField] private CanvasGroup exitOpenedGroup;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void ShowKeyAppearedMessage()
    {
        Sequence s = DOTween.Sequence();
        s.AppendCallback(() => keyAppearedImage.DOFade(1, 0.5f));
        s.AppendInterval(3f);
        s.AppendCallback(() => keyAppearedImage.DOFade(0, 0.5f));
    }

    public void UpdateLeversLeftText(int leversLeft)
    {
        if (leversLeftText != null)
        {
            UnityEngine.Debug.Log("Hello?");
            leversLeftText.text = $"Levers Left: {leversLeft}";
        }
    }

    public void ShowLeversLeftText()
    {
        if (leversLeftText != null)
        {
            leversLeftGroup.DOFade(1, 0.5f);
            leversLeftGroup.DOFade(0, 0.5f).SetDelay(3f);
        }
    }

    public void ShowExitOpenedMessage()
    {
        exitOpenedGroup.DOFade(1, 0.5f);
        exitOpenedGroup.DOFade(0, 0.5f).SetDelay(3f);
    }
}
