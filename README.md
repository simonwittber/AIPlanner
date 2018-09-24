# uHTNP #

## Type ActionState

 An action state is returned by action functions, and determines the whether the actions is marked as a success, fail or call again. 



---
#### Field ActionState.Success

 The action succeeded. 



---
#### Field ActionState.InProgress

 The action is in progress, and needs to be called again next frame. 



---
#### Field ActionState.Error

 The action failed. 



---
## Type DSL.Action

 An Action is analogous to a method call. It is passed the current world state and must return an ActionState which specifies if the action is finished, failed, or needs to be called again. 



---
#### Field DSL.Action.name

 The unique name of the action. 



---
#### Field DSL.Action.actionDelegate

 The action delegate which performs the actual work. 



---
## Type DSL.CompoundTask

 A Compound task is a list of Methods. At execute time, the appropriate method is chosen based on world state and used to create a plan. 



---
#### Method DSL.CompoundTask.DefineMethod(System.String)

 Defines a new method on this task. A method is a set of preconditions and a list of sub tasks. 



---
#### Method DSL.CompoundTask.Conditions(System.String[])

 Adds named preconditions to the last Method created by the DefineMethod call. 



---
#### Method DSL.CompoundTask.Tasks(System.String[])

 Adds named tasks to the last Method created by the DefineMethod call. 



---
## Type DSL.Domain

 The Domain is a collection of tasks, conditions and actions that are used to create a plan based on world state. 



---
#### Method DSL.Domain.New

 Create a new domain and set it to be the active domain. The domain contains all tasks and sensors. 

**Returns**: The new.



---
#### Method DSL.Domain.DefineCompoundTask(System.String)

 Defines a named compound task and adds it to the active domain. A compound task contains methods which contain conditions and other primitive and compound tasks. 

**Returns**: The compound task.

|Name | Description |
|-----|------|
|name: |Name.|


---
#### Method DSL.Domain.DefinePrimitiveTask(System.String)

 Defines a named primitive task and adds it to the active domain. A primitive task checks conditions, runs an action, and applies an effect to the world state. 

**Returns**: The primitive task.

|Name | Description |
|-----|------|
|name: |Name.|


---
#### Method DSL.Domain.DefineSensor(System.String)

 Defines a named sensor. A sensor is checked when needed and updates the world state. 

**Returns**: The sensor.

|Name | Description |
|-----|------|
|name: |Name.|


---
#### Method DSL.Domain.SetRootTask(System.String)

 Sets the root task of the planner. This is the compound task which is executed first. 

|Name | Description |
|-----|------|
|name: |Name.|


---
#### Method DSL.Domain.UpdateWorldState(uHTNP.WorldState)

 Updates the state of the world by checking all sensors. 

|Name | Description |
|-----|------|
|currentState: |Current state.|


---
#### Method DSL.Domain.Dispose

 Closes the domain, sets active domain to null. 



---
## Type DSL.Effect

 An Effect is a state name and a value. It is used by PrimitiveTasks to store the changes (effects) that are applied to world state when the task is executed. 



---
#### Field DSL.Effect.name

 The name of the state. 



---
#### Field DSL.Effect.value

 The value of the state. 



---
## Type DSL.Method

 A Method is a collection of tasks that are grouped by a common list of preconditions. The parent container the list of Methods is a CompoundTask. 



---
#### Field DSL.Method.name

 The unique name of the method. 



---
## Type DSL.Precondition

 A Precondition maps a a state that must be satisfied, as well as an optional func which must return true for the condition to succeed. 



---
#### Field DSL.Precondition.name

 The name of the state. 



---
#### Field DSL.Precondition.value

 The required value of that state. 



---
#### Field DSL.Precondition.proceduralPrecondition

 The optional procedural precondition that can return true to satisfy the precondition. 



---
## Type DSL.PrimitiveTask

 A primitive task is an action that is executed if preconditions are satisfied. It then applies it's effects to the world state. 



---
#### Method DSL.PrimitiveTask.Conditions(System.String[])

 Adds named conditions to the task. Conditions are checked before executing this task. 



---
#### Method DSL.PrimitiveTask.Actions(System.String)

 Sets the named action that this primitive task will run. 



---
#### Method DSL.PrimitiveTask.Set(System.String)

 When the task succeeds, this state variable will be set to true. 



---
#### Method DSL.PrimitiveTask.Unset(System.String)

 When the task succeeds, this state variable will be set to false. 



---
## Type DSL.Sensor

 A Sensor instance contains a delegate which is passed a WorldState instance. The delegate can inspect the world and modify the world state if required. 



---
#### Field DSL.Sensor.name

 The unique name of the sensor. 



---
#### Field DSL.Sensor.sensorDelegate

 The sensor delegate which performs logic tests and sets world state to suit. 



---
## Type DSL.Task

 The base class for Compound and Primitive Tasks. 



---
#### Field DSL.Task.name

 The unique name of the task. 



---
## Type Planner

 Planner is a static class used to create a plan (List of tasks) in a domain. 



---
#### Method Planner.CreatePlan(uHTNP.WorldState,uHTNP.DSL.Domain)

 Creates the plan based on current world state. A plan is a list of Primitive Tasks. 

**Returns**: The plan.

|Name | Description |
|-----|------|
|currentState: |Current state.|
|domain: |Domain.|


---
## Type PlanState

 Represents the current state of plan execution. 



---
#### Field PlanState.Failed

 The plan failed, usually due to a change in state effecting required preconditions for a task. 



---
#### Field PlanState.InProgress

 The plan is stil in progress, and Execute needs to be called again in the next frame. 



---
#### Field PlanState.Completed

 The plan completed successfuly. 



---
## Type PlanRunner

 The Plan Runner takes a plan (List of Primitive Tasks) and executes the plan, checking preconditions and applying effects of actions as needed. 



---
#### Method PlanRunner.#ctor(uHTNP.DSL.Domain,System.Collections.Generic.List{uHTNP.DSL.PrimitiveTask})

 Initializes a new instance of the [[|T:uHTNP.PlanRunner]] class for a given domain and list of tasks. 

|Name | Description |
|-----|------|
|domain: |Domain.|
|plan: |Plan.|


---
#### Method PlanRunner.Execute(uHTNP.WorldState)

 Execute the plan, returning current plan state. If the PlanState is InProgress, Execute will need to be called again during the next update tick. 



---
## Type WorldState

 The world state is a simple map of name to boolean values. All values default to false. 



---
#### Method WorldState.Get(System.String)

 Get the state specified by name. All states default to false. 



---
#### Method WorldState.Set(System.String,System.Boolean)

 Set the state specified by name to a value. 



---


