# UnityFinger

Screen events manager for Unity


## Usage

The sample Unity project is here for your reference!

- https://github.com/tagia0212/UnityFinger.Sample


You should know the follwoing knowledges for using UnityFinger

`FingerObserverSupervisor` supervises the observer for observing the finger events.
For observing it, the each observer needs the reference for `IScreenInput`.
I implemented the some classes implementing `IScreenInput`.

- `EditorInput`: Input for UnityEditor
- `MobileInput`: Input for deploing onto mobile

`FingerObserverSupervisor` requires the any `IScreenInput` instance for its intialization.

```
using UnityFinger;


var input = new EditorInput();
var supervisor = new FingerObserverSupervisor(input);
```

`FingerEventManager` manages the events for UnityFinger.
And also, it manages observers for registering them to `FingerObserverSupervisor`.
This means that `FingerObserverSupervisor` do nothing if there are no events for UnityFinger.
The each observer will be registered when setting `FingerEventManager` an event corresponding to it.
So `FingerManager` does create the instances of observers, or requires the observer configs.

The configuration in UnityFinger is put into `IFingerObserverConfig`.
This is an interface for all observers referring.
But considering all all configuration is so hard work, so I implemented `DefaultFingerObserverConfig`.
You can implement the class inheriting this and override some configs if you need.


```
var config = new DefaultFingerObserverConfig();
var manager = new FingerManager(supervisor, config);
```

Let's register some events via `FingerManager`!

```
manager.AddTapListener(pos => { /* ... */ });
```

As the last step for usage, you must call update functions.


```
void Update()
{
    input.Update();
    supervisor.Update();
}
```


### Requirements

- UnityEngine
- UnityEngine.UI

## Tests

I use `NUnitFramework` as test tool.
see `UnityFinger/Tests` directory.