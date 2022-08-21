![CI](../../workflows/CI/badge.svg) ![Cov](../gh-pages/docs/badge_linecoverage.svg)

* https://stackoverflow.com/questions/14024912/ddd-persistence-model-and-domain-model
* https://enterprisecraftsmanship.com/posts/having-the-domain-model-separate-from-the-persistence-model/

도메인 모델은 빠르게 변화하며 성장한다. <br/>
지속성 모델을 별도로 작성하지 않으면 도메인 모델을 인프라와 분리해서 성장 시킬 수 없다. <br/>
지속성 모델은 도메인 모델을 인프라와 연결하는 목적으로만 사용된다.


Virtual Actor는 Aggregate Root를 담당하며 도메인 모델과 지속성 모델의 추상화를 돕는다. <br/>
Virtual Actor의 Life Cycle은 Orleans의 Grain과 일치하며 내부적으로 도메인 모델 상태를 가진다.  <br/>
Deactivated로 전환될 때 도메인 모델은 지속성 모델로 변환하여 인프라를 통해 저장된다.
