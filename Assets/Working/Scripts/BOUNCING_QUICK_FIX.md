# 🎯 차가 튀는 문제 - 초간단 해결

## ⚡ 3분 해결법

### 1️⃣ 모든 WheelCollider Spring 값 변경 (가장 중요!)

```
각 WheelCollider 4개 선택:
- Colliders/FrontLeftWheel
- Colliders/FrontRightWheel
- Colliders/RearLeftWheel
- Colliders/RearRightWheel

Inspector → Wheel Collider → Suspension Spring 펼치기
- Spring: 15000 입력 (35000에서 감소!)
- Damper: 2000 입력 (4500에서 감소!)

⭐ 이것만 해도 90% 해결!
```

---

### 2️⃣ Rigidbody Center of Mass 설정

```
CAR 선택 → Rigidbody

Center of Mass:
X: 0
Y: -0.1
Z: 0

⭐ Y를 -0.1로 (낮은 무게중심 → 안정적)
```

---

### 3️⃣ CAR 높이 조정

```
CAR 선택 → Transform

Position Y: 1.5 ~ 2.0

(Body가 바닥에 닿으면 튀니까 충분히 높게)
```

---

## 🔍 확인 방법

```
Play 모드 실행

✅ 성공:
- 차가 바닥에 안정적으로 서있음
- 진동 없음
- W 키로 부드럽게 전진

❌ 여전히 튐:
- Spring을 더 낮춤 (10000으로)
- Damper를 높임 (3000으로)
```

---

## 📊 설정값 요약

### WheelCollider (4개 모두):
```
Spring: 15000 ⭐
Damper: 2000 ⭐
Force App Point: 0.05
```

### CAR Rigidbody:
```
Mass: 1.5
Center of Mass: (0, -0.1, 0) ⭐
```

### CAR Transform:
```
Position Y: 1.5 ~ 2.0
```

---

## 💡 추가 해결책 (위에서 안되면)

### Layer 충돌 방지:

```
1. Edit → Project Settings → Tags and Layers
2. User Layer 8: "CarBody"
3. User Layer 9: "Wheel"

4. Body → Layer: CarBody
5. 모든 WheelCollider → Layer: Wheel

6. Edit → Project Settings → Physics
7. Layer Collision Matrix
8. CarBody와 Wheel 체크 해제

→ Body와 WheelCollider가 충돌 안함!
```

---

## ✅ 체크리스트

- [ ] Spring: 15000 (4개 모두)
- [ ] Damper: 2000 (4개 모두)
- [ ] Center of Mass Y: -0.1
- [ ] CAR Position Y >= 1.5
- [ ] Play 시 튀지 않음
- [ ] 부드럽게 전진

---

**핵심:**
1. **Spring: 15000** (낮춤!)
2. **Center of Mass: (0, -0.1, 0)** (낮춤!)
3. **CAR Y: 1.5 이상** (높임!)

이제 안정적으로 달립니다! 🚗✨

**자세한 설명:** CAR_BOUNCING_FIX.md

