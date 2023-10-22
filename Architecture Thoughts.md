# Current Architecture problems:
- A lot of tests have to build up half the universe because models contain logic, data and reference to other models. 
- Editor support would be nice for:
  - inspecting data in runtime (debug)
  - changing data in runtime (visually test presenters)
  - invoking commands
  - Monitoring messages (partially there)
- Models can grow very big because a single model contains a lot of features

Problems with the below solution:
- encapsulation of data vs access to it from logic vs setting data for tests
- how to instantiate the features, what to pass to them (e.g. readonlycollection vs collection)

# Solution

Note: this is just a thought for now.



## Data (Model):
- properties only
- absolutely no logic
- changeable from editor in design time
- value can be examined and changed in the editor during runtime. Persistent / not?
- separate class, so testing is easier (we don't have to build up the whole universe)
- IoC for access? Central monolithic singleton (XRTK does this)?
- How to ensure encapsulation (e.g. that only the appropriate logic can change the values)?
    - Logic needs full access
    - Tests need full access
    - external systems usually only need read-only (and notification) access
    - ReactiveProperty vs ReadOnlyreactiveProperty
    - **Solution1**: all public, rely on discipline or perhaps static analysis? 
    - **Solution2**: data is exposed as IReadOnlyReactiveProperty or IReadOnlyReactiveCollection, but no data interfaces are used, and feature classes only when model grows too large.
    - **Solution3**: data is defined in interfaces. Feature classes implement interface, receive external data via injection (interfaces) and add logic. Tests use mocking / simple interface implementation to set data.
    

### Solution 1 example:
```csharp
[Serializable]
public class CubeModel
{
    [SerializeField] private IntReactiveProperty _collisions = new IntReactiveProperty(0);
    public IntReactiveProperty Collisions => _collisions;

    [SerializeField] private BoolReactiveProperty _markedForRemoval = new BoolReactiveProperty(false);
    public BoolReactiveProperty MarkedForRemoval => _markedForRemoval;
}
```
Here, properties MarkedForRemoval can even be set from the outside, which may or may not be desirable. Certainly breaks encapsulation...

### Solution 2 example:
```csharp
[Serializable]
public class CubeModel
{
    [SerializeField] private IntReactiveProperty _collisions = new IntReactiveProperty(0);

    public IReadOnlyReactiveProperty<int> Collisions => _collisions;

    [SerializeField] private BoolReactiveProperty _markedForRemoval = new BoolReactiveProperty(false);
    public IReadOnlyReactiveProperty<bool> MarkedForRemoval => _markedForRemoval;

    public CubeModel()
    {
    }

    public void Collide()
    {
        _collisions.Value++;
    }

    public void MarkForRemoval()
    {
        _markedForRemoval.Value = true;
    }
}
```

A good balance for encapsulation, but how to do tests for classes that take CubeModel as parameter? E.g. how to set `Collisions` value to test something like:
```csharp
if (cube.Collisions > x)
  //Do stuff
```

### Solution 3 example:
```csharp
public interface ICubeData
{
    IReadOnlyReactiveProperty<int> Collisions { get; }
    IReadOnlyReactiveProperty<bool> MarkedForRemoval { get; }
}

public class CubeModel : ICubeData
{
    [SerializeField] private IntReactiveProperty _collisions = new IntReactiveProperty(0);

    public IReadOnlyReactiveProperty<int> Collisions => _collisions;

    [SerializeField] private BoolReactiveProperty _markedForRemoval = new BoolReactiveProperty(false);
    public IReadOnlyReactiveProperty<bool> MarkedForRemoval => _markedForRemoval;
    ...
    
public class TestCubeData : ICubeData
{
    public IntReactiveProperty _collisions = new IntReactiveProperty();
    public BoolReactiveProperty _markedForRemoval = new BoolReactiveProperty();

    public IReadOnlyReactiveProperty<int> Collisions => _collisions;
    public IReadOnlyReactiveProperty<bool> MarkedForRemoval => _markedForRemoval;
}    
```    
This seriously feels like over-architecting.



## Logic: 
- gets, sets and subscribes to data properties
- no public fields / properties
- testable, even outside Unity
- One logic class per presenter
- **Who drives the instantiation of the logic?**
- **If the data is encapsulated (IReadOnlyReactiveProperty), how does the logic change it?**
- **How do we access private, feature and model specific data? Dictionaries?**
- problem: can get very big if organized by presenter
    - **Partial classes** for models? Just cosmetic, but helps
	- **Feature class / Service:**
		- every feature has its own class (single responsibility)
		- helps with overgrowing logic
		- accesses other features via messages and data only, except when it contains them
		- Lifecycle?
		- Idea: static only? Maybe extension methods for instances?
          - No, because it needs to be initiated.
		- This could mean more logic class per presenter, or feature classes containing feature classes
        - Service configuration via ScriptableObjects?

## Command: 
- CanExecute, Execute
- Sees the data
- Has access to the logic for helper functions
- Can be invoked from the editor
- ReactiveCommand issues:
  1. Can be subscribed to and acted upon by presenter 
  2. No async / await support
  3. CanExecute source must be provided at construction time (big problem)
  4. CommandBase solves 1 and 2, but has issues with logic encapsulation

**Get rid of ReactiveCommands altogether?**
- Use naming convention, e.g. "BoolReactiveProperty FooCanExecute" and "Foo()" or "FooAsync()"
- PresenterBase helper methods, e.g. Button.BindTo(BoolReactiveProperty canExecute, Action onExecute)
- How to invoke commands from Editor? ContextMenu in the Presenter?

##AppState - needs a good demo
  

## Presenter:
- subscribes to data changes
- calls logic via commands
- calls logic via methods?

## Message:
- system wide event that logic can subscribe to

## Can data, logic and presenter all be MonoBehaviour / ScriptableObjects?
- No, because Monobehaviour and ScriptableObject cannot be instantiated in Test time, outside Unity.
- Editor & Runtime injection with usual GameObject pointers, GetComponent, etc
- Test time injection with Init(data) or constructor injection