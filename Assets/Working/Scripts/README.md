# 🚗 RC Car 물리 시스템 - 빠른 시작

## 🎯 휠이 떨어지는 문제? → 1분 해결!

### 가장 빠른 방법:
```
Unity 메뉴 → Tools → RC Car → Quick Setup
→ CAR 오브젝트 선택
→ "Setup Everything" 클릭
→ Play!
```

---

## 📚 가이드 문서들

### 🚨 **CAR_BOUNCING_FIX.md** ⭐ 차가 튀고 날아가는 문제!
- Suspension Spring/Damper 설정
- Physics Layer 충돌 방지
- Center of Mass 최적화
- 안정적인 주행 설정

### 🚨 **HIERARCHY_FIX_GUIDE.md** - 차체와 휠이 분리되는 문제!
- Body에 Rigidbody가 있어서 분리되는 문제 해결
- 올바른 Hierarchy 구조
- 자동 수정 방법

### 🚨 **REAR_WHEEL_ONLY_FIX.md** - 뒷바퀴만 움직이는 문제!
- WheelCollider Y=0 설정
- Drive Type 변경 (AWD)
- 빠른 해결 방법

### 1. **QUICK_FIX_GUIDE.md** - 휠이 떨어지는 문제!
- 휠이 바닥 아래로 떨어지는 문제 해결
- 1분 빠른 해결 방법
- 맵 설정 방법

### 2. **RC_CAR_SETUP_GUIDE.md**
- 전체 RC카 셋업 가이드
- 단계별 상세 설명
- 체크리스트

### 3. **WHEEL_TROUBLESHOOTING.md**
- 휠 관련 모든 문제 해결
- 고급 설정
- 디버깅 방법

---

## 🛠️ 스크립트 파일들

### 핵심 스크립트
- `Movement.cs` - RC카 물리 엔진 (차량에 부착)
- `WheelColliderSetup.cs` - 휠 자동 설정 (차량에 부착)
- `GroundSetup.cs` - 지형 자동 설정 (바닥에 부착)

### 에디터 도구
- `Editor/RCCarQuickSetup.cs` - 한번에 모든 설정 (Tools 메뉴)

---

## ⚡ 빠른 체크리스트

현재 문제가 있다면:

- [ ] 바닥에 Collider 있음?
- [ ] CAR의 Y 위치 >= 1.0?
- [ ] Rigidbody의 Is Kinematic 꺼짐?
- [ ] WheelCollider 4개 할당됨?

위 4가지만 확인하면 90% 해결!

---

## 🎮 조작법

```
W/S     : 전진/후진
A/D     : 좌우 회전
Space   : 브레이크
```

---

## 📖 더 자세히 알고 싶다면

각 가이드 파일을 참고하세요:
1. 문제 해결 → `QUICK_FIX_GUIDE.md`
2. 전체 설정 → `RC_CAR_SETUP_GUIDE.md`
3. 고급 설정 → `WHEEL_TROUBLESHOOTING.md`

---

**문제가 해결되었나요?** Play 모드로 즐기세요! 🎉
**여전히 문제?** Console 메시지를 확인하세요!



