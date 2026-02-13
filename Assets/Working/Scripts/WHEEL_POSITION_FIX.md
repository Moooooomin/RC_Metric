# 🚨 뒷바퀴만 움직이고 차가 전진 안하는 문제 해결

## 📌 현재 문제 상황

```
증상: Play 모드에서 뒷바퀴만 회전하고 차가 전진하지 않음
원인: WheelCollider 위치가 실제 휠 메시 위치와 일치하지 않음
```

---

## ❌ 흔한 실수

### 실수: WheelCollider를 보이는 휠 위치에 맞춤

```
❌ 잘못된 생각:
"시각적 휠이 X=0.5, Y=-0.2, Z=0.3에 있으니
 WheelCollider도 같은 위치에 놓아야겠다"

문제:
- WheelCollider는 차체 중심(CAR) 기준으로 위치 지정
- 시각적 휠 메시는 별도로 애니메이션됨
- WheelCollider Y값이 0이 아니면 공중에 떠있거나 땅에 박힘
```

---

## ✅ 올바른 WheelCollider 위치 설정

### 핵심 규칙:

```
1. WheelCollider는 차체 중심(CAR 오브젝트) 기준
2. Y 좌표는 반드시 0 (차체 바닥 높이)
3. X, Z만 실제 휠 위치에 맞춤
4. 시각적 휠 메시는 Movement 스크립트가 자동으로 회전시킴
```

---

## 🔧 해결 방법

### Step 1: WheelCollider 위치 초기화

각 WheelCollider의 **Local Position**을 다음과 같이 설정:

#### 1. **FrontLeftWheel (Collider)**
```
Transform → Local Position:
X: -0.13 (차체 왼쪽, 휠 폭의 절반)
Y: 0 (⭐ 반드시 0!)
Z: 0.17 (차체 앞쪽, 휠베이스의 절반)
```

#### 2. **FrontRightWheel (Collider)**
```
Transform → Local Position:
X: 0.13 (차체 오른쪽)
Y: 0 (⭐ 반드시 0!)
Z: 0.17 (차체 앞쪽)
```

#### 3. **RearLeftWheel (Collider)**
```
Transform → Local Position:
X: -0.13 (차체 왼쪽)
Y: 0 (⭐ 반드시 0!)
Z: -0.17 (차체 뒤쪽)
```

#### 4. **RearRightWheel (Collider)**
```
Transform → Local Position:
X: 0.13 (차체 오른쪽)
Y: 0 (⭐ 반드시 0!)
Z: -0.17 (차체 뒤쪽)
```

---

### Step 2: WheelCollider 설정 확인

각 WheelCollider의 **Inspector** 설정:

```
Wheel Collider 컴포넌트:
- Mass: 0.2
- Radius: 0.1 (실제 휠 반지름)
- Wheel Damping Rate: 0.25
- Suspension Distance: 0.1 (서스펜션 길이)
- Force App Point Distance: 0
- Center: (0, 0, 0) ⭐ 모두 0!

Spring:
- Spring: 35000
- Damper: 4500
- Target Position: 0.5

Forward/Sideways Friction:
- Extremum Slip: 0.4
- Extremum Value: 1
- Asymptote Slip: 0.8
- Asymptote Value: 0.5
- Stiffness: 1.5
```

---

### Step 3: 실제 휠 크기 측정

**Scene View에서 확인:**

1. **Wheels/Meshes/FrontLeftWheel 선택**
2. **Scene View에서 휠 크기 확인**
3. **WheelCollider Radius 설정:**
   ```
   소형 RC카: 0.05 ~ 0.08
   중형 RC카: 0.08 ~ 0.12
   대형 RC카: 0.12 ~ 0.15
   ```

4. **휠 간격 측정:**
   - Scene View에서 앞뒤 휠 거리 확인
   - 좌우 휠 거리 확인
   - WheelCollider의 X, Z 값 조정

---

### Step 4: CAR 높이 조정

WheelCollider가 Y=0일 때 차가 공중에 뜨지 않으려면:

```
CAR Transform Position:
Y = WheelCollider Radius + Suspension Distance + 여유(0.2)

예시:
- Radius: 0.1
- Suspension: 0.1
- 여유: 0.2
→ Y = 0.4 ~ 0.5

⚠️ 너무 낮으면 차체가 바닥에 닿음
⚠️ 너무 높으면 WheelCollider가 지면에 안닿음

권장: Y = 1.0 ~ 1.5 (처음 테스트용)
```

---

## 🎯 WheelCollider 위치 계산법

### 현재 차량 크기 기준으로 계산:

#### 1. Scene View에서 측정
```
1. CAR 선택 → 차체 크기 확인
2. 실제 휠 메시 위치 확인 (Wheels/Meshes)
3. 차체 중심에서 휠까지 거리 측정
```

#### 2. X 좌표 (좌우)
```
휠 폭 = 왼쪽 휠 중심 ~ 오른쪽 휠 중심
X = ± (휠 폭 / 2)

예시:
휠이 좌우로 0.26m 떨어져있으면
→ X = ±0.13
```

#### 3. Z 좌표 (앞뒤)
```
휠베이스 = 앞바퀴 중심 ~ 뒷바퀴 중심
Z = ± (휠베이스 / 2)

예시:
앞뒤 휠이 0.34m 떨어져있으면
→ 앞: Z = +0.17
→ 뒤: Z = -0.17
```

#### 4. Y 좌표 (높이)
```
⭐ 항상 0!

이유:
- Y=0은 CAR 오브젝트의 중심 높이
- Suspension Distance가 아래로 늘어남
- 실제 지면 접촉은 자동 계산됨
```

---

## 🔍 진단 방법

### Scene View로 확인:

```
1. Scene View → Gizmos 켜기
2. WheelCollider 선택
3. 녹색 원(Gizmo)이 보임

✅ 정상:
- 4개의 원이 차체 아래에 균등하게 배치
- 원의 크기가 실제 휠과 비슷함
- 차체 바닥 근처에 위치

❌ 문제:
- 원이 공중에 떠있음 → Y값 확인
- 원이 차체 안에 있음 → Radius 확인
- 원이 한쪽으로 치우침 → X, Z값 확인
```

### Console 진단:

```
Play 모드 실행 → Console 확인

✅ 정상:
"✓ 지면 접촉: Plane"
"✓ FrontLeftWheel 설정 완료"
"✓ 모든 설정이 정상입니다!"

❌ 문제:
"⚠️ Front Left 높이가 이상합니다! (Y: 0.250)"
→ WheelCollider Local Y를 0으로 설정
```

---

## 🎮 테스트 시나리오

### 1. 정지 상태 테스트

```
Play → 관찰

✅ 정상:
- 차가 바닥에 안정적으로 서있음
- 4개 휠 모두 바닥에 닿아있음
- 차체가 흔들리지 않음

❌ 문제:
- 차가 기울어짐 → 휠 위치 불균등
- 차가 떨어짐 → 지면 Collider 없음
- 뒷바퀴만 바닥에 닿음 → 앞뒤 Z값 잘못됨
```

### 2. 전진 테스트

```
W 키 → 전진

✅ 정상:
- 차 전체가 앞으로 이동
- 4개 휠 모두 회전
- 안정적으로 직진

❌ 문제:
- 뒷바퀴만 회전 → 앞바퀴 WheelCollider 위치 확인
- 차가 제자리 회전 → 좌우 휠 위치 확인
- 차가 튕김 → Suspension 설정 확인
```

### 3. 회전 테스트

```
A 또는 D 키 → 회전

✅ 정상:
- 앞바퀴가 조향됨
- 차가 부드럽게 회전
- 뒷바퀴는 직진

❌ 문제:
- 앞바퀴가 안 움직임 → Movement 스크립트 확인
- 차가 옆으로 미끄러짐 → Sideways Friction 증가
```

---

## 🛠️ 자동 수정 도구

### 빠른 수정:

```
Tools → RC Car → Quick Setup
→ CAR Object 선택
→ "4. WheelCollider 자동 찾기" 클릭
```

또는 WheelColliderSetup 사용:

```
1. CAR 선택
2. Add Component → WheelColliderSetup
3. Inspector 설정:
   - Wheel Radius: 0.1
   - Suspension Distance: 0.1
4. Play 모드 → 자동 설정됨
```

---

## 📊 표준 설정값 (RC카 크기별)

### 소형 RC카 (1/18 스케일)
```
WheelCollider Position:
- Front: (±0.08, 0, 0.10)
- Rear: (±0.08, 0, -0.10)

WheelCollider 설정:
- Radius: 0.05
- Suspension: 0.05
- Spring: 25000

CAR Position Y: 0.3
```

### 중형 RC카 (1/10 스케일)
```
WheelCollider Position:
- Front: (±0.13, 0, 0.17)
- Rear: (±0.13, 0, -0.17)

WheelCollider 설정:
- Radius: 0.1
- Suspension: 0.1
- Spring: 35000

CAR Position Y: 0.5
```

### 대형 RC카 (1/5 스케일)
```
WheelCollider Position:
- Front: (±0.20, 0, 0.25)
- Rear: (±0.20, 0, -0.25)

WheelCollider 설정:
- Radius: 0.15
- Suspension: 0.15
- Spring: 50000

CAR Position Y: 0.7
```

---

## 💡 핵심 포인트

### WheelCollider vs 시각적 휠 메시

```
WheelCollider (물리):
- 위치: CAR 기준, Y=0
- 역할: 물리 계산, 지면 접촉
- 보이지 않음 (Gizmo로만 확인)

시각적 휠 메시:
- 위치: Wheels/Meshes 아래
- 역할: 플레이어가 보는 휠
- Movement 스크립트가 자동 회전/이동
```

### Movement 스크립트가 하는 일:

```csharp
void AnimateWheels()
{
    // WheelCollider 위치/회전 가져옴
    // → 시각적 휠 메시에 적용
    
    즉:
    WheelCollider가 올바른 위치에 있으면
    → 시각적 휠이 자동으로 맞춰짐!
}
```

---

## 🚨 자주 하는 실수들

### 실수 1: WheelCollider를 휠 메시 위치에 맞춤
```
❌ WheelCollider를 Wheels/Meshes 아래 위치로
✅ WheelCollider는 차체 중심 기준, Y=0
```

### 실수 2: Y값을 휠 높이로 설정
```
❌ Y = -0.2 (휠이 차체보다 낮으니까)
✅ Y = 0 (차체 중심 기준)
```

### 실수 3: Radius를 너무 작게/크게
```
❌ Radius = 0.01 (작음) → 차가 땅에 박힘
❌ Radius = 1.0 (큼) → 차가 공중에 뜸
✅ Radius = 실제 휠 반지름
```

### 실수 4: Suspension Distance가 0
```
❌ Suspension = 0 → 서스펜션 없음, 딱딱함
✅ Suspension = Radius와 비슷하게 (0.05~0.15)
```

---

## 🔧 단계별 수정 가이드

### 1단계: 모든 WheelCollider Y=0으로

```
Colliders/FrontLeftWheel → Transform
Local Position Y: 0

Colliders/FrontRightWheel → Transform
Local Position Y: 0

Colliders/RearLeftWheel → Transform
Local Position Y: 0

Colliders/RearRightWheel → Transform
Local Position Y: 0
```

### 2단계: X, Z값 대칭 확인

```
왼쪽 휠 X: -0.13
오른쪽 휠 X: +0.13

앞바퀴 Z: +0.17
뒷바퀴 Z: -0.17
```

### 3단계: Radius 확인

```
Scene View → WheelCollider 선택
Gizmo 원의 크기가 실제 휠과 비슷한지 확인

너무 크면: Radius 감소
너무 작으면: Radius 증가
```

### 4단계: CAR 높이 조정

```
CAR Position Y를 올려가며 테스트:
0.5 → 너무 낮으면
1.0 → 적당
1.5 → 테스트
2.0 → 안정적
```

### 5단계: Play 테스트

```
W 키 → 차 전체가 전진하는지 확인
A/D 키 → 조향되는지 확인
```

---

## 📖 관련 설정

### Movement 스크립트에서:

```
Inspector → Movement:

Engine Type: Motor 또는 Engine
Drive Type: RWD (후륜) 또는 AWD (4륜)

⭐ RWD인데 앞바퀴만 움직이면 드라이브 타입 확인!
```

### 구동 방식별 차이:

```
RWD (후륜구동):
- 뒷바퀴만 토크 받음
- 앞바퀴는 조향만
- 드리프트 쉬움

FWD (전륜구동):
- 앞바퀴만 토크 받음
- 뒷바퀴는 따라감
- 언더스티어

AWD (4륜구동):
- 모든 휠이 토크 받음
- 안정적
- 오프로드 적합
```

---

## ✅ 최종 체크리스트

- [ ] 모든 WheelCollider Local Y = 0
- [ ] WheelCollider 좌우 대칭 (X: ±0.13)
- [ ] WheelCollider 앞뒤 대칭 (Z: ±0.17)
- [ ] Radius = 실제 휠 크기
- [ ] Suspension Distance = 0.1
- [ ] CAR Position Y >= 1.0
- [ ] Scene View Gizmo로 위치 확인
- [ ] Play 모드에서 4개 휠 모두 바닥 접촉
- [ ] W 키로 전진 확인
- [ ] Console 에러 없음

---

## 🆘 여전히 뒷바퀴만 움직이면?

### 1. Drive Type 확인
```
Movement 스크립트:
Drive Type이 RWD인지 확인

→ RWD이면 뒷바퀴만 구동 (정상)
→ AWD로 변경하면 4륜 구동
```

### 2. 앞바퀴 WheelCollider 할당 확인
```
Movement 스크립트:
- Front Left Wheel: 할당됨?
- Front Right Wheel: 할당됨?

None이면 드래그하여 할당
```

### 3. 앞바퀴 위치 확인
```
Scene View → Gizmos
앞바퀴 WheelCollider가 바닥에 닿는지 확인

공중에 떠있으면:
- CAR Y 위치 증가
- 또는 앞바퀴 Z 위치 조정
```

---

**핵심 요약:**
1. **WheelCollider Local Y = 0** (항상!)
2. **X, Z만 실제 휠 위치로**
3. **Radius = 실제 휠 크기**
4. **CAR Y >= 1.0**
5. **Drive Type 확인 (AWD 권장)**

이제 차 전체가 움직일 겁니다! 🚗✨

