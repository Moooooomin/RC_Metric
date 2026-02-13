# 🚗 뒷바퀴만 움직이는 문제 - 빠른 해결

## 🎯 빠른 해결 (3가지 원인)

### 1️⃣ WheelCollider Y 위치가 0이 아님

**증상**: 앞바퀴가 공중에 떠서 지면에 안닿음

**해결**:
```
각 WheelCollider 선택 → Transform
Local Position Y: 0 (모두!)

FrontLeftWheel: (±0.13, 0, 0.17)
FrontRightWheel: (±0.13, 0, 0.17)
RearLeftWheel: (±0.13, 0, -0.17)
RearRightWheel: (±0.13, 0, -0.17)
```

---

### 2️⃣ Drive Type이 RWD (후륜구동)

**증상**: 뒷바퀴만 구동되는게 정상 (설계대로)

**해결**:
```
CAR 선택 → Movement 스크립트
Drive Type: AWD로 변경

RWD = 뒷바퀴만 (드리프트용)
FWD = 앞바퀴만
AWD = 4륜 (권장!)
```

---

### 3️⃣ 앞바퀴 WheelCollider 미할당

**증상**: 앞바퀴가 아예 작동 안함

**해결**:
```
CAR → Movement 스크립트
Front Left Wheel: Colliders/FrontLeftWheel 드래그
Front Right Wheel: Colliders/FrontRightWheel 드래그
```

---

## ⚡ 가장 빠른 해결 순서

### 1. 모든 WheelCollider Y = 0
```
Colliders/FrontLeftWheel → Local Y: 0
Colliders/FrontRightWheel → Local Y: 0
Colliders/RearLeftWheel → Local Y: 0
Colliders/RearRightWheel → Local Y: 0
```

### 2. Drive Type 변경
```
Movement → Drive Type: AWD
```

### 3. CAR 높이 조정
```
CAR → Transform Y: 1.5
(너무 낮으면 휠이 바닥에 안닿음)
```

### 4. Play 테스트
```
W 키 → 차 전체가 전진하는지 확인
```

---

## 🔍 진단 방법

### Console 확인:
```
Play 모드 실행

✅ 정상:
"✓ Front Left가 지면에 닿습니다"
"ℹ️ 구동 방식: AWD"

❌ 문제:
"⚠️ Front Left가 지면에 닿지 않습니다!"
→ CAR Y 위치 높이거나 WheelCollider Y=0
```

### Scene View 확인:
```
1. Scene View → Gizmos 켜기
2. WheelCollider 선택
3. 녹색 원 4개가 모두 바닥 근처에 있는지 확인

✅ 정상: 4개 모두 바닥에 닿음
❌ 문제: 앞바퀴가 공중에 떠있음
```

---

## 📊 표준 설정

### WheelCollider 위치:
```
⭐ 핵심: Y는 항상 0!

FrontLeft:  (-0.13, 0, +0.17)
FrontRight: (+0.13, 0, +0.17)
RearLeft:   (-0.13, 0, -0.17)
RearRight:  (+0.13, 0, -0.17)

X: 좌우 (차체 폭의 절반)
Y: 높이 (항상 0!)
Z: 앞뒤 (휠베이스의 절반)
```

### Movement 설정:
```
Drive Type: AWD (4륜, 권장)
Engine Type: Motor 또는 Engine
Vehicle Mass: 1.5
```

### CAR 위치:
```
Position Y: 1.0 ~ 1.5

너무 낮으면 → 휠이 바닥 안닿음
너무 높으면 → 차가 떨어짐
```

---

## 🎮 테스트 순서

1. **정지 상태**
   - 4개 휠 모두 바닥에 닿아있나?
   
2. **전진 (W 키)**
   - 차 전체가 앞으로 가나?
   - 4개 휠 모두 회전하나?
   
3. **조향 (A/D 키)**
   - 앞바퀴가 돌아가나?
   - 차가 회전하나?

---

## ✅ 체크리스트

- [ ] 모든 WheelCollider Local Y = 0
- [ ] Drive Type = AWD
- [ ] WheelCollider 4개 할당됨
- [ ] CAR Position Y >= 1.0
- [ ] Play 시 Console에 "지면에 닿지 않음" 경고 없음
- [ ] W 키로 전진됨
- [ ] 4개 휠 모두 회전함

---

## 📖 자세한 가이드

**WHEEL_POSITION_FIX.md** - 지금 열린 파일
- WheelCollider 위치 완벽 가이드
- 크기별 표준 설정값
- 자주 하는 실수 10가지

---

**핵심 요약:**
1. **WheelCollider Y = 0** (항상!)
2. **Drive Type = AWD** (4륜 구동)
3. **CAR Y >= 1.0** (충분히 높게)

이제 차 전체가 움직입니다! 🚗✨

