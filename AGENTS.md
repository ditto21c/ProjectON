# 저장소 가이드라인

## 프로젝트 구조 및 모듈 구성

이 저장소는 Unity 6000.3.9f1 프로젝트입니다. 소스 코드와 직접 관리하는 에셋은 `Assets/` 아래에 둡니다. `Library/`, `Temp/`, `Logs/`, `obj/` 같은 Unity 생성 폴더는 직접 수정하거나 커밋하지 마세요. 핵심 게임플레이 코드는 `ProjectON` 네임스페이스의 `Assets/Scripts/ColonySimGame.cs`에 있습니다. 씬은 `Assets/Scenes/`, URP 2D 설정은 `Assets/Settings/`, 입력 액션은 `Assets/InputSystem_Actions.inputactions`, 런타임 검증 이미지는 `Assets/Screenshots/`에 있습니다. 에셋을 이동하거나 이름을 바꿀 때는 대응되는 `.meta` 파일도 함께 유지하세요.

## 빌드, 테스트, 개발 명령

- 로컬 실행: Unity Hub에서 프로젝트 루트를 열고 에디터 `6000.3.9f1`을 사용합니다.
- Edit Mode 테스트:
  `Unity.exe -batchmode -projectPath . -runTests -testPlatform EditMode -testResults TestResults/EditMode.xml -quit`
- Play Mode 테스트:
  `Unity.exe -batchmode -projectPath . -runTests -testPlatform PlayMode -testResults TestResults/PlayMode.xml -quit`
- Windows 빌드: 정적 빌드 메서드를 추가한 뒤 실행합니다.
  `Unity.exe -batchmode -projectPath . -executeMethod BuildScript.BuildWindows -quit`

`Unity.exe`는 로컬 설치 경로에 맞게 조정하세요. 예: `C:\Program Files\Unity\Hub\Editor\6000.3.9f1\Editor\Unity.exe`

## 코딩 스타일 및 이름 규칙

C# 코드는 `ColonySimGame.cs`와 동일하게 4칸 들여쓰기와 줄바꿈 중괄호 스타일을 사용합니다. 타입, 메서드, enum 멤버, 상수는 `PascalCase`를 사용하고, 지역 변수, 매개변수, private 필드는 `camelCase`를 사용합니다. 게임 밸런스 값은 가능하면 `private const` 또는 `readonly`로 선언하세요. 큰 gameplay 파일을 수정할 때는 관련 없는 리팩터링을 피하고 변경 범위를 작게 유지하세요.

## 테스트 지침

Unity Test Framework 패키지는 설치되어 있지만 현재 프로젝트 테스트 파일은 없습니다. Edit Mode 테스트는 `Assets/Tests/EditMode/`, Play Mode 테스트는 `Assets/Tests/PlayMode/` 아래에 추가하고 파일명은 `*Tests.cs` 형식을 따르세요. 저장/불러오기, 시뮬레이션 규칙, UI 명령 모드, 수정한 시스템의 회귀 사례를 우선 검증합니다. `Assets/Screenshots/` 이미지는 실제 런타임 동작을 설명할 때만 추가하거나 갱신하세요.

## 커밋 및 Pull Request 지침

현재 체크아웃에는 `.git` 디렉터리와 커밋 이력이 없어 기존 커밋 규칙을 확인할 수 없습니다. 커밋 메시지는 `Add hydrogen filter validation`, `Fix save objective state`처럼 짧고 명령형으로 작성하세요. Pull Request에는 게임플레이 영향, 실행한 테스트, 관련 이슈, UI나 씬 변경을 보여주는 스크린샷 또는 짧은 영상을 포함합니다.

## 에이전트 작업 지침

`Library/PackageCache`와 기타 Unity 캐시 파일은 수정하지 마세요. 의존성 업그레이드가 목적이 아니라면 `Packages/manifest.json`의 패키지 버전을 유지합니다. 씬, 프리팹, 에셋을 수정할 때는 Unity가 직렬화 파일을 갱신하게 하고 대응되는 `.meta` 파일을 함께 보존하세요.
