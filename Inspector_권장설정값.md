# ⚙️ RC카 Inspector 권장 설정값 빠른 참조

> Unity Inspector에서 수동으로 설정할 때 참고하세요

---

## 🚗 Car (루트 오브젝트)

### Rigidbody 설정
```
Mass: 15
Drag: 0.5
Angular Drag: 3.0
Use Gravity: ✓
Is Kinematic: ✗
Interpolation: Interpolate
Collision Detection: Continuous

Constraints: 모두 체크 해제
```

---

## 🎮 Movement.cs 설정

### 차량 기본 설정
```
Vehicle Mass: 15
Center of Mass: (0, -0.2, 0)
```

### 휠 콜라이더 서스펜션 설정
```
Suspension Distance: 0.2
Spring Strength: 35000
Damper Strength: 4500
Target Position: 0.5
Wheel Radius: 0.08
Wheel Mass: 1.0
```

### 모터 설정
```
Motor Max Speed: 30
Motor Torque: 150
```

### 엔진 설정
```
Engine Max Speed: 50
Engine Torque: 100
```

### 조향 설정
```
Max Steering Angle: 35
Steering Speed: 3
```

### 브레이크 설정
```
Brake Force: 500
Deceleration Multiplier: 2
```

---

## 🛞 WheelCollider 설정 (각각 4개)

### Transform Position (로컬)
```
FL_Collider: (-0.15, 0, 0.25)
FR_Collider: (0.15, 0, 0.25)
RL_Collider: (-0.15, 0, -0.25)
RR_Collider: (0.15, 0, -0.25)
```
⚠️ **Y축은 반드시 0!**

### WheelCollider 컴포넌트
```
Mass: 1
Radius: 0.08
Wheel Damping Rate: 0.25
Suspension Distance: 0.2
Force App Point Distance: 0
```

### Suspension Spring
```
Spring: 35000
Damper: 4500
Target Position: 0.5
```

### Forward Friction
```
Extremum Slip: 0.4
Extremum Value: 1
Asymptote Slip: 0.8
Asymptote Value: 0.5
Stiffness: 1
```

### Sideways Friction
```
Extremum Slip: 0.2
Extremum Value: 1
Asymptote Slip: 0.5
Asymptote Value: 0.75
Stiffness: 1
```

---

## 🏞️ Ground 오브젝트

### Transform
```
Position: (0, 0, 0)
Rotation: (0, 0, 0)
Scale: (10, 1, 10)
```

### Mesh Collider
```
Convex: ✗ (체크 해제)
Is Trigger: ✗ (체크 해제)
Material: NonBouncyGround (Physics Material)
```

---

## 🧊 Physics Material (NonBouncyGround)

### 설정
```
Dynamic Friction: 0.6
Static Friction: 0.8
Bounciness: 0 ⭐ (매우 중요!)
Friction Combine: Maximum
Bounce Combine: Minimum ⭐ (매우 중요!)
```

---

## 📷 Camera Follow 설정

### 권장값
```
Offset: (0, 2, -5)
Follow Speed: 5
Rotation Speed: 3
Look At Offset: (0, 0.5, 0)
```

---

## 🎨 Body MeshCollider

### 설정
```
Convex: ✓ (체크 필수!)
Is Trigger: ✗
```

⚠️ **Rigidbody는 절대 추가하지 마세요!**

---

## ⚡ 빠른 문제 해결

### 차가 통통 튀는 경우:
1. Damper Strength → **4500**
2. Ground Bounciness → **0**
3. Bounce Combine → **Minimum**

### 차가 움직이지 않는 경우:
1. WheelCollider Y Position → **0**
2. Car Y Position → **1.5 이상**
3. Ground에 Collider 확인

### 차와 바퀴가 따로 움직이는 경우:
1. Body의 Rigidbody → **삭제**
2. Body MeshCollider Convex → **✓**

---

## 📝 설정 순서

1. **Car 위치**: Y = 1.5 이상
2. **Rigidbody 추가** (Car에만!)
3. **Movement.cs 추가**
4. **WheelCollider 4개 생성** (Y=0)
5. **Movement.cs Inspector에서 WheelCollider 할당**
6. **Ground 생성 + MeshCollider**
7. **Physics Material 생성 및 적용**
8. **Play 버튼으로 테스트**

---

**이 값들은 모두 Inspector에서 수동으로 설정하세요!**

