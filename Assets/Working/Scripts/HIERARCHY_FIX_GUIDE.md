# 🚨 차체와 휠이 분리되어 날아가는 문제 해결

## 📌 현재 문제 상황

```
증상: Play 모드 실행 시 차체(Body)와 휠(Wheels)이 따로 날아감
원인: Rigidbody가 잘못된 위치(Body)에 있음
```

---

## ❌ 잘못된 구조 (현재 상태)

```
CAR
├─ Body
│  ├─ Rigidbody ❌ 여기 있으면 안됨!
│  └─ Mesh Collider ❌ 여기 있으면 안됨!
├─ Collider
├─ Wheels
│  └─ Meshes
│     ├─ FrontLeftWheel (시각적 메시)
│     ├─ FrontRightWheel
│     ├─ RearLeftWheel
│     └─ RearRightWheel
└─ Colliders (또는 여기에 WheelCollider들)
   ├─ FrontLeftWheel (WheelCollider) ❌ Body의 자식이 아님!
   ├─ FrontRightWheel (WheelCollider)
   ├─ RearLeftWheel (WheelCollider)
   └─ RearRightWheel (WheelCollider)
```

**문제**: 
- Body에 Rigidbody가 있으면 Body만 물리 영향 받음
- WheelCollider가 Body와 연결 안됨
- 결과: 차체와 휠이 분리됨

---

## ✅ 올바른 구조

```
CAR
├─ Rigidbody ✅ 최상위에 있어야 함!
├─ Body (시각적 메시만)
│  └─ Mesh Collider ✅ 차체 충돌용
├─ Colliders
│  ├─ FrontLeftWheel (WheelCollider)
│  ├─ FrontRightWheel (WheelCollider)
│  ├─ RearLeftWheel (WheelCollider)
│  └─ RearRightWheel (WheelCollider)
├─ Wheels
│  └─ Meshes
│     ├─ FrontLeftWheel (시각적)
│     ├─ FrontRightWheel (시각적)
│     ├─ RearLeftWheel (시각적)
│     └─ RearRightWheel (시각적)
└─ Effects
```

---

## 🔧 해결 방법 (3단계)

### Step 1: Body에서 Rigidbody 제거하고 CAR로 이동

1. **Body 오브젝트 선택**
2. **Inspector에서 Rigidbody 컴포넌트 찾기**
3. **우클릭 → Remove Component** (또는 톱니바퀴 → Remove)
4. **CAR 오브젝트 선택**
5. **Add Component → Rigidbody**
6. **설정:**
   ```
   Mass: 1.5
   Drag: 0.05
   Angular Drag: 0.05
   Use Gravity: ✅
   Is Kinematic: ❌ (체크 해제!)
   Interpolation: Interpolate
   Collision Detection: Continuous
   ```

### Step 2: Body의 Mesh Collider 확인

**Body의 Mesh Collider는 그대로 유지!**
- Body는 차체 충돌 판정용
- Mesh Collider는 시각적 메시와 동일하게
- Convex: ✅ 체크 (움직이는 오브젝트이므로)

### Step 3: Movement 스크립트를 CAR에 추가

1. **CAR 오브젝트 선택**
2. **Add Component → Movement**
3. **Inspector에서 WheelCollider 할당:**
   ```
   Front Left Wheel:  Colliders/FrontLeftWheel
   Front Right Wheel: Colliders/FrontRightWheel
   Rear Left Wheel:   Colliders/RearLeftWheel
   Rear Right Wheel:  Colliders/RearRightWheel
   ```

4. **Wheel Mesh 할당 (선택사항):**
   ```
   Front Left Mesh:  Wheels/Meshes/FrontLeftWheel
   Front Right Mesh: Wheels/Meshes/FrontRightWheel
   Rear Left Mesh:   Wheels/Meshes/RearLeftWheel
   Rear Right Mesh:  Wheels/Meshes/RearRightWheel
   ```

---

## ⚙️ WheelCollider 위치 설정

각 WheelCollider의 **Local Position** 확인:

### 중요! Parent는 CAR이어야 함

```
CAR
└─ Colliders
   └─ FrontLeftWheel (WheelCollider)
      Local Position:
      X: -0.13 (차체 왼쪽)
      Y: 0 (차체 중심 높이)
      Z: 0.17 (앞쪽)
```

### 권장 위치값 (RC카 기준)

```
FrontLeftWheel:
  X: -0.13
  Y: 0
  Z: 0.17

FrontRightWheel:
  X: 0.13
  Y: 0
  Z: 0.17

RearLeftWheel:
  X: -0.13
  Y: 0
  Z: -0.17

RearRightWheel:
  X: 0.13
  Y: 0
  Z: -0.17
```

⚠️ **Y = 0이 중요!** (차체 중심과 같은 높이)

---

## 🎯 CAR 오브젝트 위치 설정

```
Transform Position:
X: 0
Y: 1.5 (최소 1.0 이상!)
Z: 0

Rotation:
X: 0
Y: 0
Z: 0

Scale:
X: 1
Y: 1
Z: 1
```

---

## ✅ 최종 체크리스트

### CAR 오브젝트
- [ ] Rigidbody 있음 (Is Kinematic ❌)
- [ ] Movement 스크립트 있음
- [ ] WheelCollider 4개 할당됨
- [ ] Transform Y >= 1.0

### Body 오브젝트
- [ ] Rigidbody 없음 ❌
- [ ] Mesh Collider만 있음 (Convex ✅)
- [ ] Parent는 CAR

### WheelCollider들
- [ ] Parent는 CAR (또는 CAR/Colliders)
- [ ] Local Position Y = 0
- [ ] 4개 모두 Movement에 할당됨

### Wheel Meshes (시각적)
- [ ] Parent는 CAR/Wheels/Meshes
- [ ] Collider 없음
- [ ] Movement에 할당 (선택사항)

---

## 🔍 진단 방법

### Play 모드 전 확인:

1. **CAR 선택 → Inspector:**
   ```
   ✅ Rigidbody 있음
   ✅ Movement 스크립트 있음
   ✅ WheelCollider 4개 할당됨
   ```

2. **Body 선택 → Inspector:**
   ```
   ✅ Mesh Collider만 있음
   ❌ Rigidbody 없어야 함!
   ```

3. **Scene View:**
   ```
   - Gizmos 켜기
   - WheelCollider가 녹색 원으로 보임
   - 차체 아래 정확한 위치에 있는지 확인
   ```

### Play 모드 테스트:

```
정상:
- 차가 바닥에 안정적으로 서있음
- 휠이 차체와 함께 움직임
- WASD로 정상 조작 가능
- Console에 에러 없음

문제:
- 차체와 휠이 분리됨 → Rigidbody 위치 확인!
- 차가 떨어짐 → 지면 Collider 확인
- 차가 안움직임 → Is Kinematic 확인
```

---

## 🚨 자주 하는 실수

### 실수 1: 여러 오브젝트에 Rigidbody
```
❌ CAR, Body 둘 다 Rigidbody 있음
✅ CAR에만 Rigidbody 하나

해결: Body의 Rigidbody 제거
```

### 실수 2: WheelCollider Parent가 잘못됨
```
❌ WheelCollider가 Body의 자식
✅ WheelCollider가 CAR의 직접 자식

해결: WheelCollider를 CAR 아래로 드래그
```

### 실수 3: Body의 Mesh Collider가 Convex 아님
```
❌ Convex 체크 해제 (정적 오브젝트용)
✅ Convex 체크 (움직이는 오브젝트용)

해결: Body의 Mesh Collider → Convex ✅
```

### 실수 4: WheelCollider Y 위치가 0이 아님
```
❌ Local Y = 0.5 또는 -0.2
✅ Local Y = 0 (차체 중심)

해결: 각 WheelCollider의 Local Position Y = 0
```

---

## 🛠️ 빠른 수정 스크립트

에디터 도구로 자동 수정:

```
Tools → RC Car → Quick Setup
→ CAR Object 선택
→ "Setup Everything" 클릭
```

이 도구가 자동으로:
1. Body의 Rigidbody 제거
2. CAR에 Rigidbody 추가
3. WheelCollider 자동 연결
4. 위치 자동 조정

---

## 📊 올바른 컴포넌트 분배

### CAR (최상위)
```
컴포넌트:
✅ Transform
✅ Rigidbody (여기만!)
✅ Movement
✅ WheelColliderSetup (선택)
```

### Body (시각적 메시)
```
컴포넌트:
✅ Transform
✅ Mesh Filter
✅ Mesh Renderer
✅ Mesh Collider (Convex)
❌ Rigidbody (없어야 함!)
```

### WheelCollider (물리 휠)
```
컴포넌트:
✅ Transform
✅ Wheel Collider
```

### Wheel Mesh (시각적 휠)
```
컴포넌트:
✅ Transform
✅ Mesh Filter
✅ Mesh Renderer
❌ Collider (없어야 함!)
```

---

## 💡 추가 팁

### Rigidbody 설정 최적화:

```
CAR의 Rigidbody:
- Mass: 1.5 (RC카는 가벼움)
- Center of Mass: (0, -0.1, 0) (낮은 무게중심)
- Constraints:
  - Freeze Position: 모두 해제
  - Freeze Rotation: 모두 해제 (자유롭게 회전)
```

### Body Mesh Collider 설정:

```
Body의 Mesh Collider:
- Convex: ✅ (반드시 체크!)
- Cooking Options: Everything
- Mesh: Body의 메시와 동일
```

### WheelCollider 설정:

```
각 WheelCollider:
- Radius: 0.1 (RC카)
- Suspension Distance: 0.1
- Spring: 35000
- Damper: 4500
- Mass: 0.2
```

---

## 🧪 테스트 시나리오

### 1. 정지 상태 테스트
```
Play → 차가 바닥에 안정적으로 서있음
✅ 통과: 휠과 차체가 분리되지 않음
❌ 실패: 분리됨 → Rigidbody 위치 확인
```

### 2. 전진 테스트
```
W 키 → 차가 전진
✅ 통과: 차체와 휠이 함께 움직임
❌ 실패: 차체만 움직임 → WheelCollider 연결 확인
```

### 3. 충돌 테스트
```
벽에 부딪힘 → 차 전체가 튕겨나감
✅ 통과: 차체와 휠이 함께 튕겨남
❌ 실패: 분리됨 → Rigidbody가 여러 개 있는지 확인
```

---

## 🆘 여전히 문제가 있다면?

### 1. Hierarchy 스크린샷 확인
```
- CAR 펼친 상태
- 모든 자식 오브젝트 보이게
```

### 2. Inspector 확인
```
CAR 선택:
- Rigidbody 설정 스크린샷
- Movement 설정 스크린샷

Body 선택:
- 컴포넌트 목록 스크린샷
```

### 3. Console 메시지
```
Play 모드 → Console 창
- 모든 에러/경고 복사
```

---

## 📖 관련 문서
- `QUICK_FIX_GUIDE.md` - 휠 떨어지는 문제
- `RC_CAR_SETUP_GUIDE.md` - 전체 셋업
- `WHEEL_TROUBLESHOOTING.md` - 휠 문제 상세

---

**핵심 요약:**
1. **Rigidbody는 CAR에만** (Body에 있으면 안됨!)
2. **WheelCollider는 CAR의 자식**
3. **Body는 시각적 + Mesh Collider만**
4. **CAR Y 위치 >= 1.0**

이렇게 하면 차체와 휠이 하나로 움직입니다! 🚗✨

