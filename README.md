# MiniGL

A Minimalistic '*graphics*' library where you are able to draw datastructures on other datastructures.

## ChangeLog

- **Jan 2017**:
  - Main storage manager added: GManager
  - GConstructor supports several shapes now

## Basic Usage

The main instance is the *GManager*. It handles polygon and datastructure storage.
The base type which is drawn, is the *GObject*. If you want to draw custom objects inheritate from this one.
Every *GObject* is associated with a couple of triangles and/or lines.
Transformations are made by the *TMaker*. Every *GObject* has its own *TMaker* to make transformations.
A basic usage looks like this:
```C#
var gMan = new GManager(50); //50 is initial capacity of the internal storage
var tm = new TMaker(); //transformation maker initialized with identity matrix
var myGObj = new GObject(tm);
gMan.AddGObject(myGObj);
//now every triangle or line added will be associated with 'myGObj'
gMan.AddVertices(new Vec4(1,1,1,1), new Vec4(0,1,1,1), new Vec4(0,0,1,1)); //add a simple triangle
//or use GConstructor for more complex shapes:
GConstructor.MakeCube(true, gMan); //filled unit cube with center at (0,0,0)
```

Drawing is made by the *Rasterizer*. A *Rasterizer* draws on the specified datastructure, usually on a *ZBuffer*.
The *Rasterizer* draws hash codes into the datastructure, you can get the corresponding *GObject* through the *GManager*.
To draw the full buffer with the *GManager* you will also need a *ViewTransformator*, which is basicly a TMaker which allows
perspective or orthogonal projection calculations and is generally a global transformation applied to all vertices.
The process is quite simple:
```C#
var zBuf = new ZBuffer(10, 10, 0); //initalize a new ZBuffer of size 10x10 with background '0'
var ras = new Rasterizer(zBuf);
var viewT = new ViewTransformator();
viewT.ProjectionType = ProjectionType.Orthogonal;
viewT.DepthRange = new Vec2(1, 15);
viewT.ViewPort = new Rect(0, 0, 10, 10);
gMan.DrawStorage(ras, viewT);
```

