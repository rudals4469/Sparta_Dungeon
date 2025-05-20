using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxObject : MonoBehaviour, IInteractable
{
    public string GetInteractPrompt() => "E키를 눌러 열기";
    public void OnInteract()
    {
        Debug.Log("상자가 열렸습니다!");
        // 필요시 애니메이션 추가
        Destroy(gameObject);
    }
}
