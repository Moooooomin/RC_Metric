# 🎯 휠이 바닥 아래로 떨어지는 문제 - 완전 해결 가이드

## 📌 문제 원인 3가지

### 1️⃣ 지형에 Collider가 없음 (90%의 경우)
```
증상: 차가 바닥을 뚫고 떨어짐
원인: 지형은 보이지만 물리적 충돌이 없음
```

### 2️⃣ WheelCollider 위치가 잘못됨
```
증상: 휠이 차체와 분리되어 공중에 떠있음
원인: Local Position Y값이 0이 아님
```

### 3️⃣ 차량이 너무 낮은 위치에 있음
```
증상: Play하자마자 바닥 아래로 시작
원인: Transform Y < 0.5
```

---

## 🚀 가장 빠른 해결 방법 (1분 컷)

### 방법 1: 자동 설정 도구 사용 (권장!)

1. **Unity 메뉴**에서:
   ```
   Tools → RC Car → Quick Setup
   ```

2. **윈도우가 열리면**:
   - `CAR Object`: CAR 오브젝트 드래그
   - `Ground Object`: 바닥 오브젝트 드래그 (없으면 비워둠)
   - **"Setup Everything" 버튼 클릭**

3. **끝!** Play 모드로 테스트

---

### 방법 2: 수동 설정 (5분)

#### Step 1: 지형 설정
```
1. Hierarchy에서 바닥 오브젝트 선택 (Plane 등)
   없으면: GameObject → 3D Object → Plane

2. Inspector → Add Component → GroundSetup

3. 또는 수동으로: Add Component → Mesh Collider
```

#### Step 2: CAR 위치 조정
```
1. CAR 오브젝트 선택
2. Transform:
   Position Y: 1.5 (최소 1.0 이상)
```

#### Step 3: Rigidbody 확인
```
CAR 오브젝트:
- Component → Rigidbody (없으면 추가)
- Mass: 1.5
- Is Kinematic: ❌ 반드시 해제!
```

#### Step 4: WheelCollider 연결
```
CAR → Movement 스크립트:
- Front Left Wheel: Colliders/FrontLeftWheel
- Front Right Wheel: Colliders/FrontRightWheel
- Rear Left Wheel: Colliders/RearLeftWheel
- Rear Right Wheel: Colliders/RearRightWheel
```

#### Step 5: WheelCollider 위치 확인
```
각 WheelCollider의 Local Position:

FrontLeftWheel:  (-0.13, 0, 0.17)
FrontRightWheel: (0.13, 0, 0.17)
RearLeftWheel:   (-0.13, 0, -0.17)
RearRightWheel:  (0.13, 0, -0.17)

⚠️ Y값은 반드시 0!
```

---

## 🔍 진단 방법

### Console 메시지 확인
Play 모드 실행하면 자동으로 진단됩니다:

```
✅ 정상:
=== RC Car 진단 시작 ===
✓ 지면 발견: Plane (거리: 1.50m)
✅ 모든 설정이 정상입니다! WASD로 움직여보세요!
=== RC Car 진단 완료 ===

❌ 문제 있음:
⚠️ 차량이 너무 낮습니다! (Y: 0.20)
💡 해결: Transform Y 위치를 1.0 이상으로 설정하세요

❌ Front Left WheelCollider가 할당되지 않았습니다!

❌ 지면을 찾을 수 없습니다!
💡 해결:
  1. Plane/Terrain 생성
  2. 지면에 GroundSetup 컴포넌트 추가
  3. 또는 MeshCollider 수동 추가
```

### Scene View에서 확인
```
1. Scene View에서 Gizmos 버튼 켜기
2. WheelCollider가 녹색 원으로 표시됨
3. 휠이 차체 아래 정확히 위치하는지 확인
```

---

## 🎮 테스트 방법

### 1. Play 모드 진입
```
- Console에 에러 없는지 확인
- "모든 설정이 정상입니다!" 메시지 확인
```

### 2. Scene View 확인
```
- 차가 바닥 위에 있는지
- 휠이 정상 위치에 있는지
- 차가 떨어지지 않는지
```

### 3. 조작 테스트
```
W: 전진
S: 후진
A: 좌회전
D: 우회전
Space: 브레이크
```

### 4. 화면 좌측 상단 정보 확인
```
속도: 0.0 km/h
엔진 타입: Motor
구동 방식: RWD
지면 접촉: True  ← 반드시 True여야 함!
```

---

## ❌ 자주하는 실수

### 실수 1: 지형에 Renderer만 있고 Collider 없음
```
문제: 지형이 보이지만 차가 떨어짐
해결: GroundSetup 추가 또는 MeshCollider 수동 추가
```

### 실수 2: Is Kinematic 체크됨
```
문제: 차가 아예 움직이지 않음
해결: Rigidbody의 'Is Kinematic' 체크 해제
```

### 실수 3: WheelCollider가 차체 밖에 있음
```
문제: 휠이 공중에 떠있음
해결: 
- Parent를 CAR로 설정
- Local Position 확인 (Y=0)
```

### 실수 4: 차량 Y 위치가 너무 낮음
```
문제: Play하자마자 바닥 아래로
해결: Transform Y를 1.5로 설정
```

---

## 🛠️ 맵(지형) 추가 작업 필요 사항

### 기본 Plane 사용시
```
✓ MeshCollider 추가됨 (자동)
✓ 추가 작업 필요 없음
```

### 커스텀 지형 사용시
```
1. 지형 메시에 Collider 추가:
   - 단순 평면: Mesh Collider
   - 복잡한 지형: Mesh Collider (Convex 해제)
   - 유니티 Terrain: Terrain Collider

2. Layer 확인:
   - Layer: Default (권장)

3. Static 설정 (선택):
   - Static 체크 (최적화)
```

### 경사로/장애물 추가시
```
모든 오브젝트에 Collider 필수:
- Box Collider (큐브)
- Mesh Collider (복잡한 메시)
- Terrain Collider (지형)
```

### 미끄러운 표면 만들기
```
1. Assets → Create → Physics Material
2. 이름: IcyPhysicsMaterial
3. 설정:
   - Dynamic Friction: 0.1
   - Static Friction: 0.1
   - Bounciness: 0
4. 지형 Collider에 Material 드래그

또는:
Movement 스크립트의 ApplySurfacePhysics() 사용
```

---

## 📊 최종 체크리스트

- [ ] 바닥에 Collider 있음
- [ ] CAR의 Y 위치 >= 1.0
- [ ] Rigidbody 추가됨 (Is Kinematic ❌)
- [ ] Movement 스크립트 추가됨
- [ ] WheelCollider 4개 모두 할당됨
- [ ] WheelCollider의 Local Y = 0
- [ ] WheelColliderSetup 추가됨
- [ ] Play 시 Console 에러 없음
- [ ] 지면 접촉: True
- [ ] WASD로 움직임 확인

---

## 💡 추가 팁

### 빠른 진단
```
Tools → RC Car → Quick Setup
→ "🔍 현재 설정 진단" 버튼 클릭
```

### Scene View 최적화
```
- Gizmos 켜서 WheelCollider 보기
- Shading Mode: Wireframe으로 Collider 확인
```

### 문제 지속시
```
1. Unity 재시작
2. Reimport Scripts (Assets 우클릭 → Reimport)
3. 새 Scene에서 테스트
```

---

## 🆘 여전히 안되면?

### 로그 확인
```
Play 모드 → Console 창 확인
모든 빨간색/노란색 메시지 복사
```

### Inspector 확인
```
CAR 오브젝트:
- Screenshot of Inspector
- All components visible

Ground 오브젝트:
- Screenshot of Inspector
```

### Scene View 확인
```
- Scene View 스크린샷
- Gizmos 켜진 상태
```

---

## 📖 관련 문서
- `RC_CAR_SETUP_GUIDE.md` - 전체 셋업 가이드
- `WHEEL_TROUBLESHOOTING.md` - 휠 문제 상세 해결
- Movement.cs - 차량 물리 스크립트
- WheelColliderSetup.cs - 휠 자동 설정
- GroundSetup.cs - 지형 자동 설정

---

**🎉 성공했다면**: WASD로 즐겁게 달려보세요!
**❌ 여전히 문제**: 위 로그/스크린샷과 함께 질문하세요.

