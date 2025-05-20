using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverObject : MonoBehaviour, IInteractable
{
    public string GetInteractPrompt() => "E키를 눌러 당기기";
    public void OnInteract()
    {
        Debug.Log("레버를 당겼습니다!");
        // 필요시 애니메이션 추가
        Destroy(gameObject);
    }
}
