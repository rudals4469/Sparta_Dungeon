🏛 Sparta_Dungeon
스파르타 던전 탐험 만들기 - 개인 과제

📌 목차
<div align="left" style="border: 1px solid #ccc; border-radius: 8px; padding: 16px; background-color: #fdfdfd; box-shadow: 1px 1px 5px rgba(0,0,0,0.1)">

<h4>📑 주요 목차</h4>

<ol>
  <li><a href="#-게임-소개">🎮 게임 소개</a></li>
  <li><a href="#-주요-기능">🚀 주요 기능</a></li>
  <li><a href="#-트러블슈팅">🛠 트러블슈팅</a></li>
</ol>

</div>
   
## 🎮 게임 소개

게임명: 던전

장르: Unity 기반 3D 액션 시뮬레이터

개발 엔진: Unity 2022.3.17f1

개발 기간: 2025.05.16 ~ 2025.05.23

## 🚀 주요 기능

✅ 체력, 스테미나 UI

<details> 
  
![UI_-ezgif com-video-to-gif-converter](https://github.com/user-attachments/assets/ca656e29-c7b5-4deb-a84b-377d33846073)

```csharp
private void Update()
{
    hunger.Add(hunger.passiveValue * Time.deltaTime);
    stamina.Add(stamina.passiveValue * Time.deltaTime);

    if (hunger.curValue == 0f)
        health.Subtract(noHungerHealthDecay * Time.deltaTime);

    if (health.curValue == 0f)
        Die();
}

public void Heal(float amount) => health.Add(amount);
public void Eat(float amount) => hunger.Add(amount);
public void Die() => Debug.Log("플레이어 사망");
```
</details> 

🔍 동적 환경 조사

<details> 
  
![ezgif com-video-to-gif-converter](https://github.com/user-attachments/assets/9031a99e-1781-46a6-97b2-8f17bce30de2)

```csharp
private void Update()
{
    if (Time.time - lastCheckTime > checkRate)
    {
        lastCheckTime = Time.time;
        Ray ray = ... // 카메라 모드에 따른 레이 생성

        if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
        {
            curInteractGameObject = hit.collider.gameObject;
            curInteractable = hit.collider.GetComponent<IInteractable>();
            SetPromptText();
        }
        else
        {
            curInteractGameObject = null;
            UIManager.Instance.ClosePrompt();
        }
    }
}
```
</details> 
  
🎥 시점 변환

<details> 
  
![ezgif com-video-to-gif-converter (1)](https://github.com/user-attachments/assets/9b9874da-31aa-4277-8888-29e14dc8fc94)

```csharp
public void OnSwitchToFirstPerson(InputAction.CallbackContext context)
{
    if (context.performed) SetCamera(firstPersonCamera);
}

public void OnSwitchToThirdPerson(InputAction.CallbackContext context)
{
    if (context.performed) SetCamera(thirdPersonCamera);
}

private void SetCamera(Camera cam) { ... }
```
</details> 
  
🧗 벽타기

<details> 
  
![ezgif com-video-to-gif-converter (2)](https://github.com/user-attachments/assets/dcba5313-8a01-4710-9c83-1b2e644854bc)

```csharp
private void FixedUpdate()
{
    if (_isClimbing)
    {
        if (IsWallInFront())
            ClimbWall();
        else
            StopClimbing();
    }
}
```
</details> 

🍎 아이템 사용 시스템

<details> 
  
아이템 타입 및 소비 효과 스크립터블 오브젝트 정의

사용 시 회복, 스탯 상승, 이중 점프 부여 등

![Movie_015-ezgif com-video-to-gif-converter](https://github.com/user-attachments/assets/bef1eec8-f6f5-47d5-9577-6cb1af5fbae3)

```csharp

public void OnUseButton()
{
    foreach (var c in _selectedItem.consumables)
    {
        switch (c.type)
        {
            case ConsumableType.Health: _condition.Heal(c.value); break;
            ...
        }
    }
    RemovedSelectedItem();
}
```
</details> 

⛓ 플랫폼 (점프/이동형)

<details> 
  
점프 플랫폼

왕복형, 회전형 이동 플랫폼

탑승 감지 후 부모-자식 구조 설정

![ezgif com-video-to-gif-converter (3)](https://github.com/user-attachments/assets/e70db141-4248-4951-b5fb-13e440c24ea3)

```csharp

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.SetParent(collision.transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.SetParent(null);
        }
    }

```

```csharp

private void OnCollisionEnter(Collision other)
{
    if (other.gameObject.CompareTag("Player"))
        rb.AddForce(jumpDirection.normalized * jumpForce, ForceMode.Impulse);
}
```

</details> 

🔦 레이저

<details> 

플레이어 탐지 및 LineRenderer로 시각화

![ezgif com-video-to-gif-converter (4)](https://github.com/user-attachments/assets/ee9da5bd-6385-40a6-8565-d3dec9ff3719)

```csharp
Ray ray = new Ray(laserOrigin.position, laserDirection.normalized);
if (Physics.Raycast(ray, out hit, laserLength, playerLayerMask)) OnPlayerEnter();
else OnPlayerExit();
```

</details> 

🚀 플랫폼 발사기

<details> 

![ezgif com-video-to-gif-converter (5)](https://github.com/user-attachments/assets/5ec62d94-46b2-4d95-8488-872c668db51b)

```csharp
private void Update()
    {
        Ray ray = new Ray(laserOrigin.position, laserDirection.normalized);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, laserLength, playerLayerMask))
        {
            if (!_playerDetected)
            {
                _playerDetected = true;
                OnPlayerEnter();
            }
        }
        else
        {
            if (_playerDetected)
            {
                _playerDetected = false;
                OnPlayerExit();
            }
        }

        if (lineRenderer != null)
        {
            Vector3 endPos = laserOrigin.position + laserDirection.normalized * laserLength;
            if (hit.collider != null)
            {
                endPos = hit.point;
            }
            
            lineRenderer.SetPosition(0, laserOrigin.position);
            lineRenderer.SetPosition(1, endPos);
        }
    }
```

</details> 

👾 AI

<details> 
  
NavMeshAgent 기반 Wandering AI

경로 탐색 실패시 대기 후 재탐색

![AI-ezgif com-video-to-gif-converter](https://github.com/user-attachments/assets/8768bb81-ce89-4a7f-a1cc-bab5b35d9d71)

```csharp
    private void Update()
    {
        _playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);
        
        _animator.SetBool("Moving", _aiState != AIState.Idle);
        
        if (_lineRenderer != null)
        {
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, _wanderTarget);
            _lineRenderer.startWidth = 0.05f;
            _lineRenderer.endWidth = 0.05f;
            _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            _lineRenderer.startColor = Color.red;
            _lineRenderer.endColor = Color.red;
        }

        switch (_aiState)
        {
            case AIState.Idle:
            case AIState.Wandering:
                PassiveUpdate();
                break;
        }
    }

    private void SetState(AIState state)
    {
        _aiState = state;

        switch (_aiState)
        {
            case AIState.Idle: 
                _agent.speed = walkSpeed;
                _agent.isStopped = true;
                break;
            case AIState.Wandering :
                _agent.speed = walkSpeed;
                _agent.isStopped = false;
                break;
        }

        _animator.speed = _agent.speed / walkSpeed;
    }

    void PassiveUpdate()
    {
        if (_agent == null || !_agent.isOnNavMesh)
        {
            return;
        }
           
        if (_aiState == AIState.Wandering && _agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle);
            Invoke(nameof(WanderToNewLocation), Random.Range(minWanderWaitTime, maxWanderWaitTime));
        }
    }

    void WanderToNewLocation()
    {
        if (_aiState != AIState.Idle) return;

        SetState(AIState.Wandering);

        Vector3 target = GetWanderLocation();
        NavMeshPath path = new NavMeshPath();

        if (_agent.CalculatePath(target, path) && path.status == NavMeshPathStatus.PathComplete)
        {
            _agent.SetPath(path); // 최종적으로 경로 설정
            _wanderTarget = target;
        }
        else
        {
            Debug.Log("경로를 찾을 수 없음: 장애물 또는 제한된 지역");
            SetState(AIState.Idle);
        }
    }

    Vector3 GetWanderLocation()
    {
        NavMeshHit hit;
        
        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance,maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
        
        int i = 0;
        do
        {
            NavMesh.SamplePosition(
                transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)),
                out hit,
                maxWanderDistance,
                NavMesh.AllAreas);

            i++;
        }while (Vector3.Distance(transform.position, hit.position) < detectDistance && i < 30);

        _wanderTarget = hit.position;

        return hit.position;
    }
```

</details> 

## 🛠 트러블슈팅

시점 전환 시 상호작용 거리 재조정 문제
→ SetCamera() 내 interaction.SetMaxCheckDistance() 로 해결

이동형 플랫폼 위 점프 시 이탈 문제
→ OnCollisionEnter/Exit 내 SetParent() 로 플랫폼과 플레이어 연결

AI 경로 계산 실패
→ NavMeshPathStatus 로 검증 후 실패 시 Idle 대기 후 재탐색

벽타기 중 벽 없을 때 자동 낙하 처리 미비
→ IsWallInFront() 없을 시 StopClimbing() 호출 추가로 해결
