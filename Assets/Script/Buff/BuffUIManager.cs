using System.Collections.Generic;
using UnityEngine;

public class BuffUIManager : MonoBehaviour
{
    public static BuffUIManager Instance;

    public GameObject buffIconPrefab;
    public Transform buffPanel;

    private Dictionary<BuffType, BuffUI> _activeBuffs = new();

    private void Awake()
    {
        Instance = this;
    }

    public void ShowBuff(Sprite icon, float duration, BuffType type)
    {
        if (_activeBuffs.TryGetValue(type, out var existingBuff))
        {
            existingBuff.Refresh(duration); // 기존 버프 시간 갱신
        }
        else
        {
            GameObject obj = Instantiate(buffIconPrefab, buffPanel);
            BuffUI ui = obj.GetComponent<BuffUI>();
            ui.Init(icon, duration, type);
            _activeBuffs.Add(type, ui);
        }
    }
}