# Currently a WIP
--------------------


# RE: Ui

RE: Ui is a GUI library for Monogame.

##### Design Goals
- Simple.
- Provide a UI framework rather than a messy full-featured library. 
- Keep it "Monogame-like".
- Easy to interface with any standard texture atlas implementation.
- Reduce side effects where possible.
- Follow the single purpose principle where possible.
- The less interdependence between components the better.

## Set Up
RE: Ui is designed to interface with any single-texture, texture atlas system, so keep your UI textures in a single texture.

You'll need to provide at least one default SpriteFont, a single Texture2D, source rectangles for that texture, as well as nice patch coordinates for those textures. Don't worry if I've lost you, this will be shown in code below.

##### If you are experienced in Monogame it might be faster for you to just check out the [Sample Project](/Sample/Game.Desktop/Game.cs)

##### The following code would all go in your Load method.

First load in your textures and source rectanges, you can do this however you like, I use TexturePacker and a small library (which is included in the Sample project).
```csharp 
TexturePackerLoader.SpriteSheetLoader loader = new SpriteSheetLoader(Content);
var spriteSheet = loader.Load("texture");
var texture = spriteSheet.Texture;
```
We need a Rectangle to define our UI bounds. This is usually just the size of the window.
```csharp
var screenBounds = new Rectangle(0, 0, 800, 600); //The size of your GUI. This is usually just the window size.
```
Now comes the most complicated part. You need to load your source rectangles into a dictionary of type `Dictionary <string,
     (Rectangle, int[])>` where:
 - The `string` is the name of a ui element. You can see all the defaults in the [Sample Project](/Sample/Game.Desktop/Game.cs). Yours should match them.
 - The `Rectange` is the source rectangle for that texture, so we know what to draw from your texture atlas.
 - The `int[]`is an array of four integers defining the nine patch coordinates for this sprite. 
 If you don't know what a nine patch(or nine slice) sprite is, you should Google it. 
 You can leave this null and it'll choose a default of `20,20,20,20`. These are in order from `Left`, `Top`, `Right` and `Bottom`.
```csharp
var windowSpliceCoordinates = new[] {20,30,20,20}; //30 on the top to leave room for the window's top bar.

var sourceRects = new Dictionary <string, (Rectangle, int[])>
{
    ["whiteTexture"] = (spriteSheet.Sprite("whitetexture").SourceRectangle, null),
    ["background"] = (spriteSheet.Sprite("background").SourceRectangle, null),
    ["windowBackground"] = (spriteSheet.Sprite("floatingbackground").SourceRectangle, windowSpliceCoordinates)
};
```
Nearly done with the setup. You need to put at least one SpriteFont into a dictionary. One of these will be your default font. Name it whatever you like, but you'll need it in the next step.
```csharp
var spriteFonts = new Dictionary <string, SpriteFont> 
{
    ["arial"] = Content.Load<SpriteFont>("Arial 11")
};
```
Lastly, put it all together and create a Canvas. `this` here is a `Game` object. Also notice the last parameter `arial` should be whatever you named your default font in the last step.
```csharp
//Off we go!
Canvas = new Canvas(this, screenBounds, texture, sourceRects, spriteFonts, "arial");
```
##### Now we are ready to start laying out our UI.

Of course, feel free to use the textures and implementation included in the sample project, just bear in mind it uses [TexturePacker](https://www.codeandweb.com/texturepacker) which isn't a free software, so if you want to add more textures without buying TexturePacker(It's great software), you'll need to find another way to define your source rectangles, or manually edit the texture data file.

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

//Add controls to our layout.

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

//Invalidate the whole UI and we're done.
canvas.Invalidate();

```

![Image of the above layout result](/layout.png)



TODO: Extending and custom controls