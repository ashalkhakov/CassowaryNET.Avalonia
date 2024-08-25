# CassowaryNET

A pure .NET implementation of the Cassowary incremental linear constraint solver.
See this [link](http://constraints.cs.washington.edu/cassowary/).

Incremental solving results in signficantly better performance than fully 
re-solving all constraints every time, making UI layout an ideal application
for this solver.

This library makes use of operator overloading to allow for simple expression
of complex constraints.


## Example

```csharp
var x = new Variable(20d);
var y = new Variable(10d);

var solver = new CassowarySolver();
    
solver.AddConstraint(x <= 5d);
solver.AddConstraint(x == y);

solver.Solve();
    
// x.Value == 5d
// y.Value == 5d
```


---


# LayoutNET

An [Avalonia](https://github.com/AvaloniaUI/Avalonia) control (`AutoLayoutPanel`) that allows you to add child controls and define 
their layout in terms of linear relationships and constraints with other controls.

Constraints can be created between `Left`, `HCenter`, `Right`, `Top`, `VCenter`, `Bottom`, 
`Width` and `Height` properties of controls.


## Example

```xml
<layout:AutoLayoutPanel
    x:Name="MainPanel">
        
    <!-- Horizontally and vertically centered in the panel. Width is between 100 and 300. -->
    <Button 
        x:Name="Button1"
        Content="Button1"
        layoutNet:AutoLayoutPanel.Constraints="
            [VCenter] eq [MainPanel.VCenter];
            [HCenter] eq [MainPanel.HCenter];
            [Width] ge [100];
            [Width] le [300];"/>

    <!-- Horizontally centred with Button1. Top is 10 below the bottom of Button1. -->
    <Button 
        x:Name="Button2"
        Content="Button2"
        layoutNet:AutoLayoutPanel.Constraints="
            [Top] eq [Button1.Bottom + 10];
            [HCenter] eq [Button1.HCenter];"/>

</layout:AutoLayoutPanel>
```
