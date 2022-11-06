![CI](../../workflows/CI/badge.svg) ![Cov](../gh-pages/docs/badge_linecoverage.svg)

## 헥사고날 아키텍처
* https://herbertograca.com/2017/11/16/explicit-architecture-01-ddd-hexagonal-onion-clean-cqrs-how-i-put-it-all-together/

![image](https://user-images.githubusercontent.com/6711748/200150653-f329ace5-d0ce-407b-b813-6e643ae8a1dd.png)


### 1. Ports
* 포트는 어플리케이션 코어의 경계에 존재한다.
* 포트는 외부세계의 언어와 도메인 언어를 모두 알고 있다.
* 동일한 도메인에서 재활용 가능하다.

### 2. Adapter
* 외부세계의 관심사만을 처리한다.
* 포트는 어뎁터의 언어를 알고 있다.
* 다른 비즈니스에서 재활용 가능하다.

## 분산 추적
![image](https://user-images.githubusercontent.com/6711748/200165901-834eda84-a7a1-4be6-b0c3-661158e466ed.png)
* 분산 시스템
