using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class FontSetter : MonoBehaviour
{
    public string fontClass;

    private void OnEnable()
    {
        OptionsManager.FontUpdated += SetFont;
    }

    private void Start()
    {
        SetFont();
    }

    private void OnDisable()
    {
        OptionsManager.FontUpdated -= SetFont;
    }

    private void SetFont()
    {
        TMP_Text testComponent = GetComponent<TMP_Text>();

        if (testComponent && GameManager.Instance.OptionsManager != null)
        {
            testComponent.font = GameManager.Instance.OptionsManager.GetFontClass(fontClass);
        }
    }
}
