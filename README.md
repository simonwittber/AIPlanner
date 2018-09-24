 uHTNP provides an DSL for defining a Hierarchial Task Network, which can be used to create a plan, which can then be executed. ##### Example:  This is the DSL used to define a BTN. 

######  code

```
    using (var domain = Domain.New())
    {
         DefinePrimitiveTask("P1")
             .Conditions("P1C1", "P1C2")
             .Actions("P1A1")
             .Set("X")
             .Unset("Y");
         DefinePrimitiveTask("P2")
             .Actions("P1A2")
             .Set("X")
             .Unset("B");
         DefineCompoundTask("C1")
             .DefineMethod("CT1")
                 .Conditions("C1C1")
                 .Tasks("P1", "P2")
             .DefineMethod("CT2")
                 .Conditions("C1C2")
                 .Tasks("P1");
         SetRootTask("C1");
    }
```
