# 🚗 RC카 Unity 셋업 완벽 가이드

> **Unity WheelCollider 기반 RC카 물리 시뮬레이션 프로젝트**  
> 마지막 업데이트: 2026-02-20

---

## 📋 목차
1. [프로젝트 개요](#프로젝트-개요)
2. [핵심 기능](#핵심-기능)
3. [Unity 셋업 가이드](#unity-셋업-가이드)
4. [권장 설정값](#권장-설정값)
5. [트러블슈팅](#트러블슈팅)

---

## 🎯 프로젝트 개요

### 구현 목표
- ✅ RC카 특유의 가벼운 움직임 (가끔 뒤집어지기도 함)
- ✅ 모터/엔진 타입 구분
  - **모터**: 초반 강한 토크, 빠른 가속, 낮은 최고속도
  - **엔진**: 리니어한 가속, 높은 최고속도
- ✅ 구동계 차이 (전륜/후륜/4륜)
- ✅ 코너링 시 스키드 마크 생성
- ✅ 표면 재질에 따른 물리 변화 (빙판, 기름 등)

### 주요 스크립트
- `Movement.cs` - 차량 물리 및 제어 (메인)
- `CameraFollow.cs` - 카메라 추적
- `SurfaceDetector.cs` - 표면 재질 감지
- `SkidMarkSetup.cs` - 스키드 마크 자동 생성

---

## ⚙️ 핵심 기능

### 1. 모터 vs 엔진 시스템
```
모터 (RC카 기본):
- MaxSpeed: 30 km/h
- Torque: 150
- 특징: 초반 토크 강함 → 빠른 가속

엔진 (고급 RC카):
- MaxSpeed: 50 km/h  
- Torque: 100
- 특징: 리니어한 가속 → 높은 최고속도
```

### 2. 구동 시스템
- **FWD** (전륜구동): 앞바퀴만 구동
- **RWD** (후륜구동): 뒷바퀴만 구동, 드리프트 쉬움
- **AWD** (4륜구동): 모든 바퀴 구동, 가장 안정적

### 3. 표면 재질 시스템
- **Normal**: 기본 노면
- **Ice**: 빙판 (마찰력 0.3)
- **Oil**: 기름 (마찰력 0.5)
- **Gravel**: 자갈 (마찰력 0.8)

---

## 🔧 Unity 셋업 가이드

### Step 1: 하이어라키 구조

올바른 구조:
```
Car (GameObject)
├── Rigidbody ✅ (여기에만!)
├── Movement.cs ✅
├── CameraFollow.cs
├── Body (GameObject)
│   ├── Mesh Filter
│   ├── Mesh Renderer
│   └── Mesh Collider (Convex ✓)
├── WheelColliders (GameObject) - 정리용 폴더
│   ├── FL_Collider (WheelCollider)
│   ├── FR_Collider (WheelCollider)
│   ├── RL_Collider (WheelCollider)
│   └── RR_Collider (WheelCollider)
└── WheelMeshes (GameObject) - 정리용 폴더
    ├── FL_Mesh
    ├── FR_Mesh
    ├── RL_Mesh
    └── RR_Mesh
```

⚠️ **중요**: 
- Rigidbody는 **Car (루트)**에만 있어야 합니다
- Body에 Rigidbody가 있으면 **반드시 삭제**하세요
- WheelCollider는 Car의 **직계 자식** 또는 **정리용 폴더 안**에 배치

---

### Step 2: WheelCollider 배치

#### WheelCollider 생성 방법:
1. Car 오브젝트 선택
2. 우클릭 → Create Empty (또는 `Ctrl+Shift+N`)
3. 이름: `FL_Collider` (FrontLeft)
4. Add Component → WheelCollider
5. **로컬 Position 설정**:

```
FL_Collider: X: -0.15, Y: 0, Z: 0.25
FR_Collider: X: +0.15, Y: 0, Z: 0.25
RL_Collider: X: -0.15, Y: 0, Z: -0.25
RR_Collider: X: +0.15, Y: 0, Z: -0.25
```

⚠️ **Y축은 반드시 0으로!** (Body 로컬 기준)

---

### Step 3: Ground (바닥) 설정

#### 3-1. Ground 오브젝트 생성
- Hierarchy 우클릭 → 3D Object → Plane
- 이름: `Ground`
- Position: (0, 0, 0)
- Scale: (10, 1, 10)

#### 3-2. Physics Material 생성
1. Project 창 → 우클릭 → Create → **Physics Material**
2. 이름: `NonBouncyGround`
3. 설정:
   ```
   Static Friction: 0.8
   Dynamic Friction: 0.6
   Bounciness: 0 ⭐ (매우 중요!)
   Friction Combine: Maximum
   Bounce Combine: Minimum ⭐ (매우 중요!)
   ```

#### 3-3. Ground에 적용
- Ground 선택 → Mesh Collider
- Material 슬롯에 `NonBouncyGround` 드래그

---

### Step 4: Movement.cs 설정

#### 4-1. Inspector 설정

**Car/Body에서:**
1. `Movement.cs` 컴포넌트 확인
2. WheelCollider 슬롯에 할당:
   - Front Left Wheel → `FL_Collider`
   - Front Right Wheel → `FR_Collider`
   - Rear Left Wheel → `RL_Collider`
   - Rear Right Wheel → `RR_Collider`

3. (선택사항) Wheel Mesh 할당

#### 4-2. 권장 초기값

아래 값들을 Inspector에서 **수동으로 설정**하세요:

```yaml
=== 차량 기본 설정 ===
Vehicle Mass: 15
Center of Mass: (0, -0.2, 0)

=== 휠 콜라이더 서스펜션 설정 ===
Suspension Distance: 0.2
Spring Strength: 35000
Damper Strength: 4500
Target Position: 0.5
Wheel Radius: 0.08
Wheel Mass: 1.0

=== 모터 설정 ===
Motor Max Speed: 30
Motor Torque: 150

=== 엔진 설정 ===
Engine Max Speed: 50
Engine Torque: 100

=== 조향 설정 ===
Max Steering Angle: 35
Steering Speed: 3

=== 브레이크 설정 ===
Brake Force: 500
Deceleration Multiplier: 2
```

---

### Step 5: Rigidbody 설정

Car (루트 오브젝트)의 Rigidbody 설정:

```yaml
Mass: 15
Drag: 0.5
Angular Drag: 3.0
Use Gravity: ✓
Is Kinematic: ✗
Interpolation: Interpolate
Collision Detection: Continuous

Constraints:
- Freeze Rotation: 모두 체크 해제
```

**Center of Mass는 Movement.cs에서 자동 설정됩니다!**

---

### Step 6: 카메라 설정

#### Main Camera 설정:
1. Main Camera 선택
2. Add Component → `CameraFollow`
3. Target에 Car 할당
4. 권장 설정:
   ```
   Offset: (0, 2, -5)
   Follow Speed: 5
   Rotation Speed: 3
   Look At Offset: (0, 0.5, 0)
   ```

---

## 📊 권장 설정값 상세

### 차량 안정화 핵심 원리

#### 1. 적절한 무게 (Mass)
- **15kg 권장** (Unity WheelCollider에 최적화)
- 너무 가벼우면 → 통통 튐
- 너무 무거우면 → 답답한 움직임

#### 2. 낮은 무게중심 (Center of Mass)
- **Y: -0.2 권장**
- 효과: 뒤집힘 방지, 코너링 안정성

#### 3. 높은 감쇠 (Damping)
- **Drag: 0.5, Angular Drag: 3.0**
- 효과: 과도한 움직임 억제

#### 4. 서스펜션 밸런스
```
Spring: 35000 (15kg 차체 지탱)
Damper: 4500 (튀는 것 완전 방지)
Distance: 0.2m (충분한 충격 흡수)
```

#### 5. 강한 그립
```
Forward Stiffness: 3.0
Sideways Stiffness: 3.0
→ 바닥에 강하게 붙어서 안정성 확보
```

---

## 🎮 조작 방법

- **W / ↑**: 전진
- **S / ↓**: 후진
- **A / ←**: 좌회전
- **D / →**: 우회전
- **Space**: 브레이크

---

## 🐛 트러블슈팅

### ❌ 문제 1: 차가 통통 튀어서 날아감

**원인:**
- Suspension Damper가 너무 낮음
- Ground에 Bounciness가 있음
- Y축 속도 제한이 없음

**해결책:**
1. Movement.cs 확인:
   - Damper Strength: **4500** (높게!)
   - Spring Strength: **35000**
2. Ground Physics Material:
   - Bounciness: **0** (절대!)
   - Bounce Combine: **Minimum**
3. Vehicle Mass: **15** 이상

---

### ❌ 문제 2: 바퀴가 바닥 아래로 떨어짐

**원인:**
- WheelCollider Y 위치가 잘못됨
- Suspension Distance가 너무 작음

**해결책:**
1. 각 WheelCollider의 **로컬 Y Position을 0**으로 설정
2. Suspension Distance: **0.2** 이상
3. Car 전체를 Y: **1.5** 이상 위치로 이동

---

### ❌ 문제 3: 차가 움직이지 않음

**체크리스트:**
- [ ] WheelCollider가 Movement.cs에 할당되었는가?
- [ ] Ground에 Collider가 있는가?
- [ ] Rigidbody가 Car (루트)에 있는가?
- [ ] Body에 불필요한 Rigidbody가 **없는가**?
- [ ] WheelCollider가 지면에 닿아있는가?

**진단 방법:**
1. Play 모드 진입
2. Console 창에서 "지면 접촉: True" 확인
3. Scene 뷰에서 WheelCollider 기즈모 확인 (초록색 선)

---

### ❌ 문제 4: 차와 바퀴가 따로 움직임

**원인:**
- Body에 Rigidbody가 있음 (치명적!)
- WheelCollider가 Car의 자식이 아님

**해결책:**
1. Body의 Rigidbody **즉시 삭제**
2. WheelCollider를 Car의 **직계 자식**으로 이동
3. Body의 Mesh Collider: **Convex ✓**

---

### ❌ 문제 5: 뒷바퀴만 움직임

**원인:**
- Drive Type이 RWD로 설정됨
- 앞바퀴 WheelCollider 할당 안됨

**해결책:**
1. Movement.cs Inspector:
   - Drive Type: **AWD** 또는 **FWD**
2. Front Left/Right Wheel 슬롯 확인

---

### ❌ 문제 6: 차가 너무 빠름/느림

**조정 방법:**

**더 빠르게:**
```
Motor Max Speed: 30 → 40
Motor Torque: 150 → 200
Vehicle Mass: 15 → 12
```

**더 느리게:**
```
Motor Max Speed: 30 → 20
Motor Torque: 150 → 100
Vehicle Mass: 15 → 20
Drag: 0.5 → 1.0
```

---

## 🔍 디버그 팁

### Console에서 확인할 정보:
```
속도: XX km/h
엔진 타입: Motor / Engine
구동 방식: FWD / RWD / AWD
지면 접촉: True / False
```

### Scene 뷰 기즈모:
- **초록색 선**: WheelCollider 서스펜션
- **빨간색 구체**: Center of Mass
- **파란색 화살표**: 휠 방향

---

## 📈 성능 최적화

### 권장 설정:
- **Fixed Timestep**: 0.02 (50Hz)
  - Edit → Project Settings → Time
  - Fixed Timestep: 0.02

- **Physics Solver**:
  - Edit → Project Settings → Physics
  - Default Solver Iterations: 6
  - Default Solver Velocity Iterations: 1

---

## 🚀 고급 기능

### 표면 재질 적용 예제:

```csharp
// SurfaceDetector.cs를 Ground에 추가하면 자동 감지
// 수동 적용:
Movement movement = car.GetComponent<Movement>();
movement.ApplySurfacePhysics(Movement.SurfaceType.Ice);
```

### 스키드 마크 활성화:

1. WheelMesh에 Trail Renderer 추가
2. Movement.cs Inspector:
   - Use Skid Marks: ✓
   - Skid Threshold: 0.4
   - 각 Skid 슬롯에 Trail Renderer 할당

---

## 📝 체크리스트

### 셋업 완료 확인:
- [ ] Car 하이어라키 구조 올바름
- [ ] WheelCollider 4개 생성 및 Y=0 설정
- [ ] Ground에 Collider + Physics Material
- [ ] Movement.cs에 WheelCollider 할당
- [ ] Rigidbody는 Car에만 있음
- [ ] Body Mesh Collider Convex ✓
- [ ] Car 높이 Y ≥ 1.5
- [ ] Camera Follow 설정

### 테스트:
- [ ] Play 버튼 클릭
- [ ] Console에 "지면 접촉: True" 표시
- [ ] WASD로 움직임 확인
- [ ] 통통 튀지 않음
- [ ] 부드러운 회전
- [ ] 스키드 마크 생성 (옵션)

---

## 🆘 추가 도움

### 문제가 계속되면:

1. **Scene 저장 후 Unity 재시작**
2. **Console 창 에러 메시지 확인**
3. **WheelCollider 기즈모 시각적 확인**
4. **Play 모드에서 Inspector 값 실시간 확인**

### 설정 초기화:
Movement.cs의 모든 값을 [권장 초기값](#4-2-권장-초기값)으로 되돌린 후 테스트

---

## ✅ 최종 정리

### 가장 중요한 3가지:

1. **Rigidbody는 Car (루트)에만!**
2. **WheelCollider Y Position = 0!**
3. **Ground Bounciness = 0!**

이 3가지만 지켜도 90%의 문제가 해결됩니다.

---

**프로젝트 제작: 2026**  
**Unity Version: 2022.3 LTS 이상 권장**

