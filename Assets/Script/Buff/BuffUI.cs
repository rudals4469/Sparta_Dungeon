using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BuffType
{
    SpeedBoost,
    AttackBoost,
    DefenseBoost,
    // 필요시 추가
}
public class BuffUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI timerText;

    private float _duration;
    private float _remainingTime;

    private BuffType _buffType;

    public BuffType BuffType => _buffType;

    public void Init(Sprite icon, float duration, BuffType type)
    {
        iconImage.sprite = icon;
        _duration = duration;
        _remainingTime = duration;
        _buffType = type;
    }

    public void Refresh(float duration)
    {
        _duration = duration;
        _remainingTime = duration;
    }

    private void Update()
    {
        if (_remainingTime > 0)
        {
            _remainingTime -= Time.deltaTime;
            timerText.text = Mathf.CeilToInt(_remainingTime).ToString();

            if (_remainingTime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
