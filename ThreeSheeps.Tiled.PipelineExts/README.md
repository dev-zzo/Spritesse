# XNA Pipeline Importers: Tiled

These classes implement importing of Tiled data (TMX and TSX files) into XNA's content pipeline.

**NOTE**: content processors and writers are to be implemented for each project separately.

## Implementation notes

Most of inspiration came from pre-existing code, namely `TiledLib`. Unfortunately, it is old and no longer supported.

There are two importers for two supported content types:

* The `TmxImporter` class imports TMX map content.
* The `TsxImporter` class imports TSX tileset content.

There is also `TxxObjectGroupImportHelper` handling common task of importing object groups data.

Currently, there is no support for:

* Animated tiles. Animating tiles seems to be very engine specific?
* Embedded textures for TSX data. This use case sounds too marginal to invest effort in supporting it.
