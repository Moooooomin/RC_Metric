# 🔧 휠이 바닥 아래로 떨어지는 문제 해결 가이드

## 🎯 빠른 해결 방법 (3단계)

### 1단계: 지형에 Collider 추가 ⭐ 가장 중요!

#### 방법 A: 자동 설정 (권장)
```
1. 지형 오브젝트(Plane) 선택
2. Add Component → GroundSetup
3. Play 모드 실행 → 자동으로 Collider 추가됨
```

#### 방법 B: 수동 설정
```
1. 지형 오브젝트 선택
2. Add Component → Mesh Collider (Plane인 경우)
3. Convex 체크 해제
```

### 2단계: WheelCollider 자동 설정

```
1. RC_Car 오브젝트 선택
2. Add Component → WheelColliderSetup
3. Play 모드 실행 → Console에서 진단 결과 확인
```

### 3단계: 차량 위치 조정

```
1. RC_Car 선택
2. Transform Position Y를 1.0 이상으로 설정
   (권장: Y = 1.5 ~ 2.0)
```

---

## 🔍 문제 원인 진단

### Console 로그 확인하기

Play 모드로 실행하면 다음과 같은 진단 메시지가 나타납니다:

#### ✅ 정상인 경우
```
[WheelSetup] 모든 휠 설정 완료!
✓ 지면 접촉: Plane
  접촉 거리: 0.050m
```

#### ⚠️ 문제가 있는 경우
```
⚠️ 지면 접촉 안됨!
  아래 0.500m에 지면 발견: Plane
  💡 해결책: Suspension Distance를 0.400m로 증가시키세요
```

#### ❌ 심각한 문제
```
❌ 차량 아래 지면을 찾을 수 없습니다!
💡 해결책:
  1. Plane 또는 Terrain 생성
  2. 지형에 Collider 추가
```

---

## 📋 체크리스트

### 지형 설정 체크
- [ ] 지형(Plane/Terrain)이 존재하는가?
- [ ] 지형에 Collider가 있는가? (MeshCollider, TerrainCollider, BoxCollider)
- [ ] 지형의 Layer가 'Default'인가?
- [ ] 지형의 Scale이 정상인가?

### WheelCollider 설정 체크
- [ ] 4개의 WheelCollider가 모두 할당되었는가?
- [ ] Wheel Radius가 적절한가? (RC카: 0.05~0.15)
- [ ] Suspension Distance가 적절한가? (RC카: 0.05~0.15)
- [ ] 휠이 차체 아래에 위치하는가?

### 차량 설정 체크
- [ ] RC_Car의 Y 위치가 1.0 이상인가?
- [ ] Rigidbody가 추가되었는가?
- [ ] Rigidbody의 Mass가 설정되었는가?
- [ ] Constraints가 모두 해제되었는가?

---

## 🛠️ 상황별 해결책

### 문제 1: "지면을 찾을 수 없습니다"

**원인**: 지형에 Collider가 없음

**해결책**:
```
방법 1 (자동):
1. 지형 선택
2. Add Component → GroundSetup
3. Play

방법 2 (수동):
1. 지형 선택
2. Add Component → Mesh Collider
3. Mesh Collider의 Mesh에 Plane 메시 할당
```

### 문제 2: "지면 접촉 안됨"

**원인**: Suspension Distance가 너무 짧거나 차량이 너무 높음

**해결책**:
```
Console 로그에서 추천값 확인:
"💡 해결책: Suspension Distance를 0.400m로 증가시키세요"

1. WheelColliderSetup 선택
2. Suspension Distance 값을 로그의 추천값으로 변경
3. Inspector에서 우클릭 → "휠 설정 적용"
```

### 문제 3: 휠이 여전히 떨어짐

**원인**: 휠 위치가 잘못되었거나 차량이 너무 낮음

**해결책**:
```
1. RC_Car의 Y 위치를 2.0으로 올림
2. 각 휠의 Local Position 확인:
   - FrontLeft: (-0.3, 0, 0.4)
   - FrontRight: (0.3, 0, 0.4)
   - RearLeft: (-0.3, 0, -0.4)
   - RearRight: (0.3, 0, -0.4)
3. Y값은 0 또는 약간 음수 (차체 바로 아래)
```

### 문제 4: 휠이 너무 깊이 들어감

**원인**: Suspension이 너무 부드럽거나 Spring이 약함

**해결책**:
```
WheelColliderSetup에서:
- Suspension Spring: 35000 → 50000 증가
- Suspension Damper: 4500 → 6000 증가
- Suspension Distance: 0.1 → 0.08 감소
```

---

## 🎨 권장 설정값 (RC카)

### 소형 RC카 (1~2kg)
```
WheelCollider:
- Radius: 0.08
- Suspension Distance: 0.08
- Spring: 30000
- Damper: 4000
- Mass: 0.15

차량 위치:
- Y: 1.5
```

### 중형 RC카 (2~3kg)
```
WheelCollider:
- Radius: 0.12
- Suspension Distance: 0.12
- Spring: 40000
- Damper: 5000
- Mass: 0.25

차량 위치:
- Y: 2.0
```

### 대형 RC카 (3~5kg)
```
WheelCollider:
- Radius: 0.15
- Suspension Distance: 0.15
- Spring: 50000
- Damper: 6000
- Mass: 0.3

차량 위치:
- Y: 2.5
```

---

## 🎮 테스트 시나리오

### 1. 기본 테스트
```
1. Play 모드 실행
2. Console 확인
3. "✓ 지면 접촉" 메시지 확인
4. W키로 전진 테스트
```

### 2. Scene 뷰 확인
```
1. Play 모드에서 Scene 탭 선택
2. RC_Car 선택
3. Gizmos로 휠 위치 확인
4. 초록색 점 = 지면 접촉 (정상)
5. 빨강/파랑 원 = 휠 위치
```

### 3. Debug 정보 확인
```
Game 뷰 왼쪽 상단:
- "지면 접촉: True" → 정상
- "지면 접촉: False" → 문제 있음
```

---

## 🔧 Inspector 유틸리티

WheelColliderSetup 컴포넌트에서 우클릭하면:

### "휠 설정 적용"
- 현재 설정값으로 모든 휠 재설정
- 변경 사항 즉시 적용

### "휠 진단 실행"
- 현재 상태 진단
- Console에 상세 정보 출력
- 문제점 및 해결책 제시

---

## 📊 Scene 뷰 Gizmo 가이드

### 색상 의미
- **빨강**: Front Left Wheel
- **초록**: Front Right Wheel
- **파랑**: Rear Left Wheel
- **노랑**: Rear Right Wheel

### 선과 점
- **와이어 구**: 휠 위치 및 크기
- **아래 선**: 서스펜션 범위
- **초록 점**: 지면 접촉 지점 (있으면 정상!)

---

## 💡 추가 팁

### Tip 1: Physics Debug 활성화
```
Window → Analysis → Physics Debugger
Play 모드에서 모든 Collider 시각화
```

### Tip 2: Layer 충돌 매트릭스 확인
```
Edit → Project Settings → Physics
Layer Collision Matrix에서 Default ↔ Default 체크 확인
```

### Tip 3: 프레임레이트 확인
```
Fixed Timestep이 너무 크면 휠이 불안정:
Edit → Project Settings → Time
Fixed Timestep: 0.02 권장 (50 FPS)
```

---

## 🚨 자주 하는 실수

### ❌ 실수 1: 지형에 Renderer만 있고 Collider 없음
```
증상: 지형은 보이지만 차가 그냥 떨어짐
해결: GroundSetup 추가 또는 Collider 수동 추가
```

### ❌ 실수 2: 휠이 차체 밖에 위치
```
증상: 휠이 공중에 떠있음
해결: 휠의 Parent를 RC_Car로 설정
      Local Position 확인
```

### ❌ 실수 3: Rigidbody가 Kinematic
```
증상: 차가 아예 움직이지 않음
해결: Rigidbody의 Is Kinematic 체크 해제
```

### ❌ 실수 4: 차량이 너무 낮음 (Y < 0.5)
```
증상: Play하자마자 휠이 지면 아래
해결: Transform Y를 1.5 이상으로
```

---

## 📞 여전히 해결 안 되면?

### 1. Console 로그 캡처
```
Play 모드 실행 → Console 전체 복사
"=== 휠 콜라이더 진단 시작 ===" 부터
"=== 휠 콜라이더 진단 완료 ===" 까지
```

### 2. 설정값 확인
```
- Movement 컴포넌트 스크린샷
- WheelColliderSetup 컴포넌트 스크린샷
- Hierarchy 뷰 (RC_Car 펼친 상태)
```

### 3. 수동 디버그
```
Console에서 다음 정보 찾기:
- "차량 높이: X.XXm"
- "Radius: X.XX"
- "Suspension Distance: X.XX"
- "지면 발견: [이름]" 또는 "지면을 찾을 수 없습니다"
```

---

**이 가이드로 99%의 휠 문제를 해결할 수 있습니다! 🎉**

*문제가 계속되면 WheelColliderSetup의 "휠 진단 실행"을 사용하세요.*

