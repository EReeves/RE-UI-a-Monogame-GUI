# RE: Ui

RE: Ui is a GUI library or framework for Monogame.

##### Design Goals
- Simple
- Provide a UI framework rather than a messy full-featured library. 
- Easy to interface with any standard texture atlas implementation.
- Reduce side effects where possible
- Follow the single purpose principle where possible
- Don't prioritize code reuse if it will lead to negative affects on the previous design principles. Better to duplicate a little code than create complex co-dependencies.

## Set Up
RE: Ui is designed to interface with any single texture, texture atlas system, so keep your UI textures in a single texture.

You'll need to provide at least one default SpriteFont, a single Texture2D, source rectangles for that texture, as well as nice patch coordinates for those textures, this will all be explained further if your aren't following along.

##### The following code would all go in your Load method.

First load in your textuers and source rectanges, you can do this however you like, I use TexturePacker and a library inluded in the Sample project.
```csharp 
TexturePackerLoader.SpriteSheetLoader loader = new SpriteSheetLoader(Content);
var spriteSheet = loader.Load("texture");
var texture = spriteSheet.Texture;
```
We need a Rectangle to define our UI bounds, usually just the size of the window.
```csharp
var screenBounds = new Rectangle(0, 0, 800, 600); //The size of your GUI, usually just the window size.
```
Now the most complicated part, you need to load your source rectangles into a dictionary of type `Dictionary <string,
     (Rectangle, int[])>`
 - The `string` is the name of the texture, you can see the defaults in the Sample, yours should match.
 - The `Rectange` is the source rectangle for that texture so we know where to draw.
 - The `int[]`is an array of four integers defining the nine patch coordinates for this sprite. 
 If you don't know what a nine patch(or nine slice) sprite is, give it a Google. 
 You can leave this null and it'll choose a default of `20,20,20,20`. These go in order from `Left`, `Top`, `Right` and `Bottom`.
```csharp
var windowSpliceCoordinates = new[] {20,30,20,20}; //30 on the top to leave room for the window's top bar.

var sourceRects = new Dictionary <string, (Rectangle, int[])>
{
    ["whiteTexture"] = (spriteSheet.Sprite("whitetexture").SourceRectangle, null),
    ["background"] = (spriteSheet.Sprite("background").SourceRectangle, null),
    ["windowBackground"] = (spriteSheet.Sprite("floatingbackground").SourceRectangle, windowSpliceCoordinates)
};
```
Then at least one SpriteFont, this will be your default font. Name it whatever you like, but you'll need it in the next step.
```csharp
var spriteFonts = new Dictionary <string, SpriteFont> 
{
    ["arial"] = Content.Load<SpriteFont>("Arial 11")
};
```
Lastly, put it all together and create a Canvas, `this` is a `Game` object. Also notice the last parameter `arial` should be whatever you named your font in the last step.
```csharp
    //Off we go!
    Canvas = new Canvas(this, screenBounds, texture, sourceRects, spriteFonts, "arial");
```
##### Now we are ready to start laying out our UI.

Of course feel free to use the textures and implementation included in the sample project, just bear in mind it uses TexturePacker which isn't a free software, so if you want to add more textures without buying TexturePacker(It's great software), you'll need to manually edit the texture data file.

##Layout


TODO