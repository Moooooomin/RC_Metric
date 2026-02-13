# 🚗 RC카 완전 셋업 가이드

## 📌 현재 문제점
- ✅ 휠이 바닥 아래로 떨어짐
- ✅ 차가 제대로 움직이지 않음
- ✅ WheelCollider가 제대로 작동하지 않음

---

## 🎯 해결 방법 (순서대로 진행)

### ✅ 1단계: 지형(바닥) 설정 ⭐⭐⭐ (가장 중요!)

#### 방법:
1. **Hierarchy**에서 바닥 오브젝트 찾기
   - `Plane`, `Ground`, `Floor` 등의 이름으로 검색
   - 없으면 `GameObject → 3D Object → Plane` 으로 새로 생성

2. **바닥 오브젝트 선택 후:**
   ```
   Inspector → Add Component → GroundSetup (검색)
   ```

3. **GroundSetup 설정:**
   - `Auto Add Collider`: ✅ 체크
   - Play 모드 실행 → Console에 "MeshCollider 추가됨" 메시지 확인

#### 수동으로 하려면:
```
바닥 오브젝트 선택 → Add Component → Mesh Collider
(Convex는 체크 해제 상태로 유지)
```

---

### ✅ 2단계: 차량(CAR) 오브젝트 설정

#### 하이어라키 구조 확인:
```
CAR (최상위)
├─ Body (차체 메시)
├─ Collider (차체 충돌)
├─ Colliders (폴더)
│  ├─ FrontLeftWheel (WheelCollider)
│  ├─ FrontRightWheel (WheelCollider)
│  ├─ RearLeftWheel (WheelCollider)
│  └─ RearRightWheel (WheelCollider)
├─ Wheels (폴더)
│  └─ Meshes (폴더)
│     ├─ FrontLeftWheel (시각적 메시)
│     ├─ FrontRightWheel (시각적 메시)
│     ├─ RearLeftWheel (시각적 메시)
│     └─ RearRightWheel (시각적 메시)
└─ Effects
```

#### CAR 오브젝트 설정:
1. **CAR 오브젝트 선택**
2. **Position 설정:**
   ```
   Transform:
   X: 0
   Y: 1.0 (⭐ 중요! 바닥에서 최소 1m 위)
   Z: 0
   ```

3. **Rigidbody 확인/추가:**
   ```
   - Mass: 1.5
   - Drag: 0.05
   - Angular Drag: 0.05
   - Use Gravity: ✅
   - Is Kinematic: ❌ (체크 해제!)
   ```

4. **Movement 스크립트 추가:**
   ```
   Add Component → Movement
   ```

5. **WheelColliderSetup 추가 (진단/자동설정용):**
   ```
   Add Component → WheelColliderSetup
   
   설정:
   - Auto Setup On Start: ✅
   - Show Debug Info: ✅
   - Wheel Radius: 0.1
   - Suspension Distance: 0.1
   ```

---

### ✅ 3단계: WheelCollider 연결

#### Movement 스크립트 Inspector에서:

**Wheel Collider 할당:**
- `Front Left Wheel` → Colliders/FrontLeftWheel 드래그
- `Front Right Wheel` → Colliders/FrontRightWheel 드래그
- `Rear Left Wheel` → Colliders/RearLeftWheel 드래그
- `Rear Right Wheel` → Colliders/RearRightWheel 드래그

**Wheel Mesh 할당 (선택사항):**
- `Front Left Mesh` → Wheels/Meshes/FrontLeftWheel 드래그
- `Front Right Mesh` → Wheels/Meshes/FrontRightWheel 드래그
- `Rear Left Mesh` → Wheels/Meshes/RearLeftWheel 드래그
- `Rear Right Mesh` → Wheels/Meshes/RearRightWheel 드래그

---

### ✅ 4단계: WheelCollider 위치 조정

각 WheelCollider (Colliders 폴더 아래)의 **Local Position**을 확인:

#### FrontLeftWheel:
```
Position:
X: -0.13 (왼쪽)
Y: 0 (차체 중심 기준)
Z: 0.17 (앞쪽)
```

#### FrontRightWheel:
```
Position:
X: 0.13 (오른쪽)
Y: 0
Z: 0.17 (앞쪽)
```

#### RearLeftWheel:
```
Position:
X: -0.13 (왼쪽)
Y: 0
Z: -0.17 (뒤쪽)
```

#### RearRightWheel:
```
Position:
X: 0.13 (오른쪽)
Y: 0
Z: -0.17 (뒤쪽)
```

⚠️ **중요**: Y값은 0이어야 합니다! (차체 중심과 같은 높이)

---

### ✅ 5단계: Play 모드로 테스트

1. **Play 버튼 클릭**

2. **Console 확인:**
   ```
   ✓ FrontLeftWheel 설정 완료
   ✓ FrontRightWheel 설정 완료
   ✓ RearLeftWheel 설정 완료
   ✓ RearRightWheel 설정 완료
   
   --- 지면 Collider 체크 ---
   ✓ 지면 발견: Plane
   ```

3. **Scene View에서 확인:**
   - 차가 바닥에 올라가 있는지
   - 휠이 차체 아래 정상 위치에 있는지
   - Game View Gizmos 켜서 WheelCollider 위치 확인

4. **조작 테스트:**
   ```
   W/S: 전진/후진
   A/D: 좌회전/우회전
   Space: 브레이크
   ```

---

## 🔍 문제 해결 (Still Having Issues?)

### 문제: "지면을 찾을 수 없습니다" 에러

**해결:**
1. Scene에 Plane이 있는지 확인
2. Plane에 Collider가 있는지 확인
3. Plane의 Layer가 "Default"인지 확인

### 문제: 휠이 여전히 공중에 떠있음

**해결:**
1. WheelCollider의 Local Position Y값 = 0으로 설정
2. Suspension Distance = 0.1로 설정
3. CAR 오브젝트 Y 위치를 2.0으로 올려서 테스트

### 문제: 차가 기울어지거나 뒤집힘

**해결:**
1. Rigidbody → Center of Mass 확인
   ```
   X: 0
   Y: -0.1 (낮게)
   Z: 0
   ```
2. Vehicle Mass: 1.5 ~ 2.0 사이로 조정

### 문제: 차가 너무 빠름/느림

**해결:**
```
Movement 스크립트에서:
- Motor Max Speed: 30 (느린 RC카)
- Engine Max Speed: 50 (빠른 RC카)
- Motor Torque/Engine Torque 조정
```

---

## 🎮 최종 체크리스트

- [ ] 바닥에 Collider 있음
- [ ] CAR의 Y 위치 >= 1.0
- [ ] Rigidbody 추가됨 (Is Kinematic ❌)
- [ ] Movement 스크립트 추가됨
- [ ] WheelCollider 4개 모두 할당됨
- [ ] WheelCollider Y Position = 0
- [ ] WheelColliderSetup 스크립트 추가됨
- [ ] Play 모드에서 Console 에러 없음
- [ ] WASD로 움직임 확인

---

## 💡 팁

### 빠른 디버깅:
```
1. CAR 선택
2. WheelColliderSetup의 "Show Debug Info" 체크
3. Play 모드 → Console 메시지 확인
```

### Scene View에서 WheelCollider 보기:
```
Scene View → Gizmos 버튼 클릭
WheelCollider가 녹색 원으로 표시됨
```

### 물리 설정 초기화:
```
Edit → Project Settings → Physics
Reset 버튼 클릭
```

---

## 🚀 다음 단계

설정이 완료되면:
1. 스키드 마크 추가 (TrailRenderer)
2. 표면 재질별 물리 설정
3. 카메라 추적 스크립트
4. UI (속도계 등)

---

**문제가 계속되면:**
- Console 메시지 스크린샷
- Inspector 창 스크린샷
- Scene View 스크린샷
을 함께 공유해주세요!

