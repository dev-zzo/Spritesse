import argparse

def generate(texture_name, size, count, offset, spacing):
    print('<?xml version="1.0" encoding="utf-8" ?>')
    print('<XnaContent>')
    print('  <Asset Type="ThreeSheeps.Spritesse.PipelineExts.SpriteSheetContent">')
    print('    <TextureName>' + texture_name + '</TextureName>')
    print('    <Definitions>')
    stepX = size[0] + spacing[0]
    stepY = size[1] + spacing[1]
    row = 0
    for y in xrange(0, stepY * count[1], stepY):
        print('      <!-- Row %d, start index %d -->' % (row, row * count[0]))
        for x in xrange(0, stepX * count[0], stepX):
            print('      <Item><SourceRectangle>%d %d %d %d</SourceRectangle><PivotOffset>0 0</PivotOffset></Item>' % (offset[0] + x, offset[1] + y, size[0], size[1]))
        row += 1
    print('    </Definitions>')
    print('  </Asset>')
    print('</XnaContent>')

generate(
    "aaa", 
    (16, 16),
    (27, 16),
    (2, 3),
    (1, 1))
