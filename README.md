# PreAdScreen

## How to add in project
PackageManager > + > Add package from git URL..
```
https://github.com/TheErikSar/PreAdScreenPackage.git
```

## Examples

```csharp
// Инициализация в начале игры
PreAdvertScreenRoot.Initialize(Resources.Load<ClickItem>("Prefabs/Particle Click Item"));

// =========================================== Настройка =================================================

// Настройка параметро, если стандартные не устраивают
PreAdvertScreenRoot.Duration = TimeSpan.FromSeconds(5);
PreAdvertScreenRoot.SpawnCooldown = TimeSpan.FromSeconds(0.5);
PreAdvertScreenRoot.Speed = 300;

// Обновляем внешний вид (при необходимости)
var screenData = new ScreenData("Счетчик: {tag}", "Реклама через\n{tag}", "{tag}");
PreAdvertScreenRoot.Setup(backgroundSprite: Resources.Load<Sprite>("Sprites/adclicker_background"), 
    activeTimer: true, 
    activeCounter: true,
    screenData: screenData,
    mode: CanvasMode.CAMERA_SPACE);

// или дефолтное значение

PreAdvertScreenRoot.Setup();

// ============================================= Запуск ==================================================

// Запуск
PreAdvertScreenRoot.Run(
    onOpen: () => Debug.Log("Start!"),
    onSpawnClickItem: item => item.SetSize(percentage: 30),
    onClose: () => Debug.Log("End!") // Тут запускаем рекламу например
);
```