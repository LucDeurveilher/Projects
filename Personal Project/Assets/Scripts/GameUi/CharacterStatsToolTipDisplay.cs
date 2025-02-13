using TMPro;
using UnityEngine;

public class CharacterStatsToolTipDisplay : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI cardTypesText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI dodgeChanceText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI critChanceText;
    public TextMeshProUGUI critDamageText;
    public TextMeshProUGUI damageTypeText;
    public TextMeshProUGUI effectBoostText;
    public TextMeshProUGUI attackPaternText;
    public TextMeshProUGUI priorityTargetText;

    private RectTransform rectTransform;
    public CanvasGroup canvasGroup;
    [SerializeField] private float lerpFactor = 0.1f;
    [SerializeField] private float xOffset = 200f;
    private Canvas canvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (canvasGroup.alpha != 0)
        {
            //RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,Input.mousePosition, canvas.worldCamera, out Vector2 pos);
            //rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition,new Vector2(pos.x+ xOffset, pos.y),lerpFactor);
        }
    }

    public void SetStatsText(CharacterStats stats)
    {
        nameText.text = $"{stats.characterName} Stats";
        cardTypesText.text = string.Join(", ", stats.cardElementType);
        healthText.text = stats.health.ToString();
        dodgeChanceText.text = $"{stats.dodgeChance} %";
        damageText.text = $"{stats.damageMin} - {stats.damageMax}";
        critChanceText.text = $"{stats.critChance} %";
        critDamageText.text = $"{stats.critDamage} %";
        damageTypeText.text = string.Join(", ", stats.damageType);
        effectBoostText.text = $"{stats.effectBoost} %";
        attackPaternText.text = stats.attackPattern.ToString();
        priorityTargetText.text = stats.priorityTarget.ToString();
    }
}
