# 🚨 차가 통통 튀고 날아가는 문제 해결

## 📌 문제 원인

```
증상: 차가 땅에 닿을 때마다 튀고, 심하면 하늘로 날아감
원인: Suspension 설정이 너무 강하거나 충돌 문제
```

---

## ⚡ 빠른 해결 (5분)

### 🎯 1단계: Suspension Spring 값 감소 (가장 중요!)

```
각 WheelCollider 선택 (4개 모두):

Inspector → Wheel Collider → Suspension Spring
- Spring: 35000 → 15000 으로 변경 ⭐
- Damper: 4500 → 2000 으로 변경 ⭐
- Target Position: 0.5 (유지)

너무 강한 Spring → 차가 튕김
너무 약한 Spring → 차체가 바닥에 닿음
```

### 🎯 2단계: Body의 Mesh Collider와 WheelCollider 충돌 방지

```
문제: Body의 Mesh Collider가 WheelCollider와 충돌
해결: Physics 레이어 분리

1. Body 오브젝트 선택
2. Inspector → Layer → Add Layer...
3. User Layer 8: "CarBody" 추가
4. User Layer 9: "Wheel" 추가

5. Body → Layer: CarBody
6. 모든 WheelCollider → Layer: Wheel

7. Edit → Project Settings → Physics
8. Layer Collision Matrix에서:
   - CarBody와 Wheel 체크 해제 (충돌 안함)
```

### 🎯 3단계: Rigidbody Mass 확인

```
CAR 선택 → Rigidbody

Mass: 1.5 (RC카 기준)
- 너무 가벼우면 (0.5) → 튕김 심함
- 너무 무거우면 (5.0) → 무겁게 느껴짐

권장: 1.5 ~ 2.5
```

### 🎯 4단계: Force App Point Distance 설정

```
각 WheelCollider:

Force App Point Distance: 0.05 로 변경
(기본값 0은 불안정할 수 있음)
```

---

## 🔧 상세 설정값 (RC카 최적화)

### ✅ WheelCollider 설정 (4개 모두 동일)

```
=== Mass ===
Mass: 0.2

=== Radius ===
Radius: 0.1 (실제 휠 크기에 맞춤)
Wheel Damping Rate: 0.25

=== Suspension Distance ===
Suspension Distance: 0.1

=== Suspension Spring ===
Spring: 15000 ⭐ (35000에서 감소!)
Damper: 2000 ⭐ (4500에서 감소!)
Target Position: 0.5

=== Forward Friction ===
Extremum Slip: 0.4
Extremum Value: 1.0
Asymptote Slip: 0.8
Asymptote Value: 0.5
Stiffness: 1.0

=== Sideways Friction ===
Extremum Slip: 0.2
Extremum Value: 1.0
Asymptote Slip: 0.5
Asymptote Value: 0.75
Stiffness: 1.0

=== Force App Point Distance ===
Force App Point Distance: 0.05 ⭐
```

### ✅ Rigidbody 설정 (CAR)

```
Mass: 1.5 (RC카 기준)
Drag: 0.05
Angular Drag: 0.05
Use Gravity: ✅
Is Kinematic: ❌

Interpolation: Interpolate
Collision Detection: Continuous

Constraints:
- Freeze Position: 모두 해제
- Freeze Rotation: 모두 해제

Center of Mass: (0, -0.1, 0) ⭐ 낮은 무게중심
```

### ✅ Body Mesh Collider 설정

```
Body 선택 → Mesh Collider

Convex: ✅ 체크
Cooking Options: Everything
Mesh: Body 메시와 동일

⚠️ Material: None (PhysicMaterial 없이)
또는 Friction 0.5 정도의 Material
```

---

## 🎯 WheelCollider Inspector 설정 스크린샷 기준

### Spring 값이 너무 높은 경우:

```
❌ 문제:
Spring: 35000 이상
Damper: 4500 이상
→ 차가 딱딱한 스프링처럼 튕김

✅ 해결:
Spring: 10000 ~ 20000 (RC카)
Damper: 1500 ~ 3000
→ 부드러운 서스펜션
```

---

## 🔍 진단 방법

### Console 확인:

```
Play 모드 → Console

정상:
- 에러 없음
- "✓ 지면 발견"
- 차가 안정적으로 서있음

문제:
- "Wheel is penetrating" 경고
→ Suspension Distance 증가
→ Spring 값 감소
```

### Scene View 확인:

```
1. Play 모드 진입
2. Scene View에서 차 관찰
3. Pause 버튼으로 프레임 정지

정상:
- 차가 바닥에 안정적으로 붙어있음
- 서스펜션이 약간 압축됨
- 진동 없음

문제:
- 차가 위아래로 진동
- Suspension이 극단적으로 압축/팽창
→ Spring 값 조정 필요
```

---

## 🛠️ 단계별 해결 가이드

### Step 1: 모든 WheelCollider Spring 값 변경

```
1. Colliders/FrontLeftWheel 선택
2. Inspector → Wheel Collider
3. Suspension Spring 펼치기
4. Spring: 15000 입력
5. Damper: 2000 입력

6. FrontRightWheel, RearLeftWheel, RearRightWheel 반복
   (또는 복사 붙여넣기)
```

### Step 2: Physics Layer 설정 (충돌 방지)

```
=== Layer 생성 ===
1. Edit → Project Settings → Physics
2. Tags and Layers 탭
3. User Layer 8: "CarBody"
4. User Layer 9: "Wheel"

=== Layer 할당 ===
1. Body 선택 → Layer: CarBody
2. Colliders/FrontLeftWheel → Layer: Wheel
3. Colliders/FrontRightWheel → Layer: Wheel
4. Colliders/RearLeftWheel → Layer: Wheel
5. Colliders/RearRightWheel → Layer: Wheel

=== 충돌 매트릭스 설정 ===
1. Edit → Project Settings → Physics
2. Layer Collision Matrix (아래쪽)
3. CarBody와 Wheel의 교차점 체크 해제
   → 이제 Body와 WheelCollider가 충돌 안함!
```

### Step 3: Rigidbody Center of Mass 설정

```
CAR 선택 → Rigidbody

Center of Mass:
X: 0
Y: -0.1 ⭐ (낮은 무게중심)
Z: 0

→ 차가 안정적이고 잘 안뒤집힘
```

### Step 4: Play 테스트

```
1. Play 버튼
2. 차가 안정적으로 서있는지 확인
3. W 키로 전진 → 튀지 않는지 확인
4. Space 키로 정지 → 안정적으로 멈추는지 확인
```

---

## 🎮 테스트 시나리오

### 1. 정지 상태 안정성

```
Play → 관찰 (10초)

✅ 통과:
- 차가 바닥에 가만히 있음
- 진동 없음
- 서스펜션이 자연스럽게 압축됨

❌ 실패:
- 차가 위아래로 흔들림 → Spring 감소
- 차가 통통 튐 → Damper 증가
- 차체가 바닥에 닿음 → Spring 증가 또는 CAR Y 증가
```

### 2. 전진 테스트

```
W 키 → 전진

✅ 통과:
- 부드럽게 전진
- 바닥에 붙어서 이동
- 튀지 않음

❌ 실패:
- 통통 튐 → Spring/Damper 재조정
- 하늘로 날아감 → Body와 Wheel 충돌 → Layer 확인
```

### 3. 장애물 충돌

```
벽이나 장애물에 부딪힘

✅ 통과:
- 자연스럽게 튕겨남
- 과도하게 튀지 않음
- 다시 안정됨

❌ 실패:
- 하늘로 날아감 → Collision Detection 확인
- 계속 튐 → Angular Drag 증가
```

---

## 📊 Spring/Damper 값 가이드

### RC카 크기별 권장값

```
=== 소형 RC카 (1/18) ===
Spring: 10000 ~ 15000
Damper: 1500 ~ 2000
Mass: 1.0

=== 중형 RC카 (1/10) ===
Spring: 15000 ~ 20000 ⭐ 이거!
Damper: 2000 ~ 3000
Mass: 1.5

=== 대형 RC카 (1/5) ===
Spring: 25000 ~ 35000
Damper: 3500 ~ 5000
Mass: 2.5
```

### Spring 값 조정 기준

```
Spring 값이 너무 높으면:
- 딱딱함
- 차가 튕김
- 서스펜션이 거의 압축 안됨
→ 값을 50% 감소

Spring 값이 너무 낮으면:
- 무름
- 차체가 바닥에 닿음
- 서스펜션이 완전히 압축됨
→ 값을 50% 증가

적절한 값:
- 차가 바닥에 있을 때 서스펜션이 30~70% 압축
- 부드럽게 움직임
- 튀지 않음
```

### Damper 값 조정 기준

```
Damper = Spring의 약 10~15%

Spring: 15000 → Damper: 1500~2000
Spring: 20000 → Damper: 2000~3000

Damper가 너무 높으면:
- 서스펜션이 천천히 반응
- 딱딱한 느낌

Damper가 너무 낮으면:
- 서스펜션이 계속 진동
- 튀는 느낌
```

---

## 💡 추가 해결책

### 문제: 여전히 튐

**원인 1: Physics Timestep 문제**

```
Edit → Project Settings → Time

Fixed Timestep: 0.02 (기본값)
→ 0.01로 변경 (더 정밀)

Maximum Allowed Timestep: 0.1 (기본값)
→ 0.033으로 변경

⚠️ 성능에 영향 있음
```

**원인 2: Collision Detection 문제**

```
CAR → Rigidbody

Collision Detection: Continuous ✅
(Discrete는 고속에서 통과할 수 있음)
```

**원인 3: Body Collider가 지면과 충돌**

```
Body의 Mesh Collider가 바닥에 닿으면:
→ WheelCollider와 동시 충돌
→ 튕김 발생

해결:
1. CAR Y 위치 증가 (1.5 → 2.0)
2. Body Mesh Collider 크기 축소
3. 또는 Layer로 충돌 방지 (위에서 설명)
```

---

## 🔧 자동 설정 스크립트

Movement 스크립트가 이미 있으니, WheelColliderSetup에서 자동 설정:

```
CAR 선택 → Add Component → WheelColliderSetup

Inspector 설정:
- Suspension Spring: 15000
- Suspension Damper: 2000

Play 모드 → 자동 적용됨
```

또는 Tools 메뉴:

```
Tools → RC Car → Quick Setup
→ "Setup Everything" 클릭
→ 자동으로 적절한 값 설정됨
```

---

## ✅ 최종 체크리스트

### WheelCollider 설정 (4개 모두):
- [ ] Spring: 15000 (35000에서 감소)
- [ ] Damper: 2000 (4500에서 감소)
- [ ] Force App Point Distance: 0.05
- [ ] Layer: Wheel

### Rigidbody 설정 (CAR):
- [ ] Mass: 1.5
- [ ] Center of Mass: (0, -0.1, 0)
- [ ] Collision Detection: Continuous
- [ ] Interpolation: Interpolate

### Body 설정:
- [ ] Mesh Collider Convex: ✅
- [ ] Layer: CarBody
- [ ] CarBody와 Wheel 충돌 해제

### Physics Settings:
- [ ] Layer Collision Matrix 설정
- [ ] Fixed Timestep: 0.02 (또는 0.01)

### 테스트:
- [ ] 정지 상태에서 튀지 않음
- [ ] 전진 시 안정적
- [ ] 충돌 후 과도하게 튀지 않음

---

## 🚨 자주 하는 실수

### 실수 1: Spring이 너무 높음

```
❌ Spring: 35000 이상
→ 단단한 스프링, 튕김

✅ Spring: 15000 전후
→ 부드러운 서스펜션
```

### 실수 2: Body와 Wheel 충돌

```
❌ 모두 Default Layer
→ Body가 WheelCollider와 충돌
→ 예상치 못한 튕김

✅ Layer 분리 + 충돌 매트릭스 설정
→ 충돌 방지
```

### 실수 3: CAR이 너무 낮음

```
❌ CAR Y: 0.5
→ Body가 바닥에 닿음
→ 이중 충돌

✅ CAR Y: 1.5 이상
→ WheelCollider만 지면 접촉
```

### 실수 4: Center of Mass가 높음

```
❌ Center of Mass: (0, 0, 0) 또는 (0, 0.5, 0)
→ 무게중심 높음
→ 튀고 뒤집힘

✅ Center of Mass: (0, -0.1, 0) 또는 (0, -0.2, 0)
→ 낮은 무게중심
→ 안정적
```

---

## 🎯 최적 설정 요약

```
=== WheelCollider (각각) ===
Mass: 0.2
Radius: 0.1
Suspension Distance: 0.1
Spring: 15000 ⭐
Damper: 2000 ⭐
Force App Point: 0.05
Layer: Wheel

=== CAR Rigidbody ===
Mass: 1.5
Center of Mass: (0, -0.1, 0) ⭐
Collision Detection: Continuous
Interpolation: Interpolate

=== CAR Transform ===
Position Y: 1.5 ~ 2.0

=== Physics Layers ===
Body: CarBody Layer
WheelColliders: Wheel Layer
CarBody ↔ Wheel 충돌: 해제 ⭐
```

---

## 📖 관련 문서

- **REAR_WHEEL_ONLY_FIX.md** - 뒷바퀴만 움직이는 문제
- **WHEEL_POSITION_FIX.md** - WheelCollider 위치 설정
- **HIERARCHY_FIX_GUIDE.md** - 차체/휠 분리 문제

---

**핵심 요약:**

1. **Spring: 15000** (35000에서 감소!)
2. **Damper: 2000** (4500에서 감소!)
3. **Layer 분리** (Body ↔ Wheel 충돌 해제)
4. **Center of Mass: (0, -0.1, 0)** (낮게!)

이제 차가 안정적으로 달립니다! 🚗✨

