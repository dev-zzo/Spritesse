# Spritesse: Content pipeline

This project contains various content pipeline extensions to handle custom content types.

## Sprite sheets

This is metadata for a texture atlas containing multiple sprites; each definition provides the location and size of a sprite.
The pivot offset is the location of the (0, 0) point within the sprite.

The engine type is `ThreeSheeps.Spritesse.Content.SpriteSheet`.

Content files should have the `.spritesheet` extension.

### Example data

```xml
<?xml version="1.0" encoding="utf-8" ?>
<XnaContent>
  <Asset Type="ThreeSheeps.Spritesse.PipelineExts.SpriteSheetContent">
    <TextureName>Sprites/cat.spritesheet</TextureName>
    <Definitions>
      <Item>
        <SourceRectangle>0 0 32 32</SourceRectangle>
        <PivotOffset>15 31</PivotOffset>
      </Item>
      <Item>
        <SourceRectangle>32 0 32 32</SourceRectangle>
        <PivotOffset>15 31</PivotOffset>
      </Item>
      <Item>
        <SourceRectangle>64 0 32 32</SourceRectangle>
        <PivotOffset>15 31</PivotOffset>
      </Item>
    </Definitions>
  </Asset>
</XnaContent>
```

## Animation sets

This describes a collection of frame-by-frame animations using sprites from a sprite sheet.
An animation set contains multiple animation sequences. 
Each sequence has a name (used to select the sequence during run time) and a set of frame descriptions. Sequences can be looped automatically.

The engine type is `ThreeSheeps.Spritesse.Content.AnimationSet`.

Content files should have the `.animset` extension.

### Example data

```xml
<?xml version="1.0" encoding="utf-8" ?>
<XnaContent>
  <Asset Type="ThreeSheeps.Spritesse.PipelineExts.AnimationSetContent">
    <SpriteSheet>Sprites/cat</SpriteSheet>
    <Animations>
      <Sequence>
        <Name>walk_d</Name>
        <Looped>true</Looped>
        <Frames>
          <Item>
            <SpriteIndex>0</SpriteIndex>
            <Delay>200</Delay>
          </Item>
          <Item>
            <SpriteIndex>1</SpriteIndex>
            <Delay>200</Delay>
          </Item>
          <Item>
            <SpriteIndex>2</SpriteIndex>
            <Delay>200</Delay>
          </Item>
          <Item>
            <SpriteIndex>1</SpriteIndex>
            <Delay>200</Delay>
          </Item>
        </Frames>
      </Sequence>
    </Animations>
  </Asset>
</XnaContent>
```

# References

* https://blogs.msdn.microsoft.com/shawnhar/2008/08/12/everything-you-ever-wanted-to-know-about-intermediateserializer/
* https://blogs.msdn.microsoft.com/shawnhar/2008/08/26/customizing-intermediateserializer-part-1/
* https://blogs.msdn.microsoft.com/shawnhar/2008/08/26/customizing-intermediateserializer-part-2/
* https://blogs.msdn.microsoft.com/shawnhar/2008/08/27/why-intermediateserializer-control-attributes-are-not-part-of-the-content-pipeline/
