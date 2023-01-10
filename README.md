# RE: Ui

RE: Ui is a simple user interface library for MonoGame.

## Design Goals
- Simple
- Easy for MonoGame developers to grasp ("MonoGame-like")
- Easy to interface with any texture atlas implementation
- Extensible

Updates to RE: Ui may break your code until a stable version is established.   

## Quickstart

1. Load in your textures and source rectanges, you can do this however you like. I use TexturePacker and a small library (which you can find in the [Sample Project](/Sample/Game.Desktop/Game.cs)).
```csharp 
TexturePackerLoader.SpriteSheetLoader loader = new SpriteSheetLoader(Content);
var spriteSheet = loader.Load("texture");
var texture = spriteSheet.Texture;
```
2. Create a Rectangle to define the bounds for your UI. For most purposes, this is just the size of your MonoGame Window.
```csharp
var screenBounds = new Rectangle(0, 0, 800, 600); //The size of your GUI. This is usually just the window size.
```
3. Load your source rectangles into a dictionary of type `Dictionary <string, (Rectangle, int[]?)>` where:
- `string` is a label for the texture. See the [Sample Project](/Sample/Game.Desktop/Game.cs) for all defaults.
 - `Rectange` is the source rectangle for that texture.
 - `int[]`is an optional array of nine patch coordinates, in the order `Left`, `Top`, `Right` and `Bottom`. `null` defaults to `20,20,20,20`.
```csharp
var sourceRects = new Dictionary <string, (Rectangle, int[])>
{
    ["whiteTexture"] = (spriteSheet.Sprite("whitetexture").SourceRectangle, null),
    ["background"] = (spriteSheet.Sprite("background").SourceRectangle, null),
    ["windowBackground"] = (spriteSheet.Sprite("floatingbackground").SourceRectangle, new[] {20,30,20,20})
};
```
4. Place your SpriteFonts into a dictionary.
```csharp
var spriteFonts = new Dictionary <string, SpriteFont> 
{
    ["arial"] = Content.Load<SpriteFont>("Arial 11")
};
```
5. Create a Canvas. 
 - `this` is your MonoGame `Game` object. TODO: Remove. It shouldn't need the entire Game object.
 - `arial` is the name of your default font, as defined in the last step.
```csharp
//Off we go!
Canvas = new Canvas(this, screenBounds, texture, sourceRects, spriteFonts, "arial");
```
## Layout

```csharp
//Off we go!
canvas = new Canvas(this, screenBounds, texture, sourceRects, spriteFonts, "arial");

//Window.
var window = new Window(canvas)
{
    TitleBarHeight = 20,
    Bounds = new Rectangle(200, 100, 250, 350),
    Color = Color.White,
    PaddingExplicit = new[] {12, 16, 12, 15}
};

//Give it a layout.
var verticalLayout = new VerticalLayout(window);

//Add controls to our layout. Constructors define the parent control. TODO: Replace with a parent method. Why did I do this?

var blank = new Image(verticalLayout)
{
    Weight = 3,
    Texture = "corgi"
};
var inputText = new InputText(verticalLayout)
{
    Weight = 1,
};
var button = new Button(verticalLayout)
{
    Weight = 1,
    Text = "Go!",
};

//Invalidate the UI, and we're done.
canvas.Invalidate();

```

![Image of the above layout result](/layout.png)



## TODO: Extending and custom controls