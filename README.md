# 🚗 RC Car Physics Simulation

Unity WheelCollider 기반 RC카 물리 시뮬레이션 프로젝트

---

## 📂 프로젝트 구조

```
RC_Metric/
├── Assets/
│   └── Working/
│       └── Scripts/
│           ├── Movement.cs          # 메인 차량 제어 스크립트
│           ├── CameraFollow.cs      # 카메라 추적
│           ├── SurfaceDetector.cs   # 표면 감지
│           ├── SkidMarkSetup.cs     # 스키드 마크
│           ├── WheelColliderSetup.cs # 휠 진단 도구
│           └── GroundSetup.cs       # 지면 헬퍼
│
├── RC카_셋업_가이드.md              # 📘 전체 가이드 (메인)
├── Inspector_권장설정값.md          # ⚙️ 빠른 설정 참조
└── README.md                        # 이 파일

```

---

## 🎯 핵심 기능

- ✅ **RC카 특유의 물리**: 가벼운 움직임, 쉽게 뒤집힘
- ✅ **모터 vs 엔진**: 서로 다른 가속/최고속도 특성
- ✅ **구동계 시스템**: FWD, RWD, AWD
- ✅ **표면 재질**: 빙판, 기름, 자갈 등
- ✅ **스키드 마크**: 코너링 시 자동 생성

---

## 🚀 빠른 시작

### 1. 문서 읽기 순서

1. **RC카_셋업_가이드.md** - 전체 가이드 (처음 읽기)
2. **Inspector_권장설정값.md** - 설정할 때 빠른 참조

### 2. 기본 셋업 (5분)

1. Car 오브젝트 Y 위치 → **1.5 이상**
2. Car에 **Rigidbody** 추가 (Mass=15, Drag=0.5, AngularDrag=3)
3. Car에 **Movement.cs** 추가
4. WheelCollider 4개 생성 (로컬 **Y=0**)
5. Ground 생성 + MeshCollider + Physics Material (**Bounciness=0**)
6. Movement.cs Inspector에서 WheelCollider 할당
7. **Play!**

자세한 내용은 `RC카_셋업_가이드.md` 참조

---

## ⚠️ 중요 사항

### 반드시 지켜야 할 3가지:

1. **Rigidbody는 Car (루트)에만!**
   - Body에 Rigidbody 있으면 즉시 삭제
   
2. **WheelCollider Y Position = 0!**
   - 로컬 좌표 기준
   
3. **Ground Bounciness = 0!**
   - Physics Material 필수

---

## 🛠️ 자동 셋팅 제거됨!

**이 프로젝트는 더 이상 자동 셋팅을 하지 않습니다.**

### 이유:
- 자동 셋팅이 문제를 더 복잡하게 만듦
- 사용자가 직접 설정값을 이해하는 것이 중요
- Inspector에서 실시간 조정 가능

### 대신:
- **권장 초기값**이 코드에 이미 설정되어 있음
- **Inspector_권장설정값.md**에서 빠른 참조 가능
- 문제 발생 시 값 확인 후 수정

---

## 🎮 조작

- **W / ↑**: 전진
- **S / ↓**: 후진
- **A / ←**: 좌회전
- **D / →**: 우회전
- **Space**: 브레이크

---

## 📊 권장 스펙

- **Unity**: 2022.3 LTS 이상
- **Physics**: Default (3D)
- **Fixed Timestep**: 0.02 (50Hz)

---

## 🐛 트러블슈팅

### 차가 통통 튀어요!
→ `RC카_셋업_가이드.md` - 트러블슈팅 섹션 참고

### 차가 움직이지 않아요!
→ `Inspector_권장설정값.md` - 빠른 문제 해결 참고

### 설정값이 헷갈려요!
→ `Inspector_권장설정값.md` - 모든 권장값 정리되어 있음

---

## 📚 문서 목록

| 문서 | 용도 | 읽기 순서 |
|-----|------|---------|
| **RC카_셋업_가이드.md** | 전체 가이드, 원리 설명 | 1️⃣ 처음 |
| **Inspector_권장설정값.md** | 빠른 참조, 설정값 목록 | 2️⃣ 설정 시 |
| **README.md** | 프로젝트 개요 | - 지금 읽는 중 |

---

## ✅ 체크리스트

설정 완료 확인:
- [ ] Car에 Rigidbody만 있음 (Body에 없음)
- [ ] WheelCollider 4개 Y=0
- [ ] Movement.cs에 WheelCollider 할당
- [ ] Ground에 Collider + Physics Material
- [ ] Ground Bounciness = 0
- [ ] Car Y 위치 ≥ 1.5
- [ ] Play 모드에서 "지면 접촉: True" 확인

---

## 🆘 도움말

1. 먼저 `RC카_셋업_가이드.md` 읽기
2. 트러블슈팅 섹션 확인
3. Inspector 값을 `Inspector_권장설정값.md`와 비교
4. Console 창 에러 메시지 확인

---

**Made with ❤️ in Unity**  
**Last Updated: 2026-02-20**

