namespace CaptainCoder.TileBuilder
{
    using System.Collections.Generic;
    using System;
    using System.Text;

    [Serializable]
    public class TileMap
    {
        private static readonly Dictionary<TileSide, (int, int, TileSide)> NEIGHBOR_INFO = new Dictionary<TileSide, (int, int, TileSide)>();

        static TileMap()
        {
            NEIGHBOR_INFO[TileSide.West] = (-1, 0, TileSide.East);
            NEIGHBOR_INFO[TileSide.East] = (1, 0, TileSide.West);
            NEIGHBOR_INFO[TileSide.South] = (0, -1, TileSide.North);
            NEIGHBOR_INFO[TileSide.North] = (0, 1, TileSide.South);
        }

        public bool IsEmpty { get => tiles.Count == 0; }

        private readonly Dictionary<(int, int), ITile> tiles = new Dictionary<(int, int), ITile>();

        public IEnumerable<(int, int)> GetAllPos()
        {
            return this.tiles.Keys;
        }

        public bool HasTile((int x, int y) pos)
        {
            return this.tiles.ContainsKey(pos);
        }

        public ITile InitTileAt((int x, int y) pos)
        {
            if (this.tiles.TryGetValue(pos, out ITile tile))
            {
                return tile;
            }

            ITile newTile = new BasicTile(pos);

            // If neighbors exist on any side, copy the wall configuration.
            foreach (TileSide mySide in TileUtils.WALLS)
            {
                (int offX, int offY, TileSide neighborSide) = NEIGHBOR_INFO[mySide];
                if (this.HasTile((pos.x + offX, pos.y + offY)))
                {
                    newTile.SetSide(mySide, this.GetTile((pos.x + offX, pos.y + offY)).GetSide(neighborSide));
                }
            }
            this.tiles[pos] = newTile;
            return newTile;
        }

        public ITile GetTile((int x, int y) pos)
        {
            this.tiles.TryGetValue(pos, out ITile tile);
            return tile;
        }

        public TileMap RemoveTile((int x, int y) pos)
        {
            this.tiles.Remove(pos);
            return this;
        }

        public TileMap SetWall((int x, int y) pos, TileSide side, WallType wallType)
        {
            if (!this.HasTile(pos))
            {
                this.InitTileAt(pos);
            }

            // Set the wall for this tile
            this.GetTile(pos).SetSide(side, wallType);

            if (NEIGHBOR_INFO.ContainsKey(side))
            {
                // Check for neighbor, if the neighbor exists update neighbors wall to match.
                (int offX, int offY, TileSide neighborSide) = NEIGHBOR_INFO[side];
                if (this.HasTile((pos.x + offX, pos.y + offY)))
                {
                    this.GetTile((pos.x + offX, pos.y + offY)).SetSide(neighborSide, wallType);
                }
            }

            return this;
        }

        public TileMap AddWall((int x, int y) pos, TileSide side)
        {
            return this.SetWall(pos, side, WallType.Wall);
        }

        public TileMap RemoveWall((int x, int y) pos, TileSide side)
        {
            return this.SetWall(pos, side, WallType.None);
        }

        public TileMap ToggleWall((int x, int y) pos, TileSide side)
        {
            if (this.HasTile(pos))
            {
                return this.SetWall(pos, side, TileUtils.Toggle(this.GetTile(pos).GetSide(side)));
            }
            return this;
        }

        public string ToStringMap(int cursorX = 0, int cursorY = 0, TileSide facing = TileSide.Top)
        {
            int minX = cursorX;
            int minY = cursorY;
            int maxX = cursorX;
            int maxY = cursorY;
            foreach ((int x, int y) pos in this.tiles.Keys)
            {
                minX = Math.Min(minX, pos.x);
                maxX = Math.Max(maxX, pos.x);
                minY = Math.Min(minY, pos.y);
                maxY = Math.Max(maxY, pos.y);
            }

            int sizeX = (maxX + 1) - minX;
            int sizeY = (maxY + 1) - minY;
            // UnityEngine.Debug.Log($"Size: {sizeX}, {sizeY}");

            char[,] textMap = new char[(sizeX * 2) + 1, (sizeY * 2) + 1];
            for (int i = 0; i < textMap.GetLength(0); i++)
            {
                for (int j = 0; j < textMap.GetLength(1); j++)
                {
                    textMap[i, j] = ' ';
                }
            }

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    // UnityEngine.Debug.Log($"Building {x}, {y}");
                    int _x = x - minX;
                    int _y = maxY - y; // Y decreasing moves up
                    // UnityEngine.Debug.Log($"Translates to {_x}, {_y}");
                    int top = (_y) * 2;
                    int left = (_x) * 2;
                    // UnityEngine.Debug.Log($"Top: {top}, Left: {left}");

                    // If there is no tile here, skip it.
                    if (!this.HasTile((x, y)))
                    {
                        continue;
                    }
                    // UnityEngine.Debug.Log($"Found Tile @ {x}, {y}");

                    ITile tile = this.GetTile((x, y));

                    // Corners


                    textMap[left, top] = '*';
                    // If both the tile to the left of this and the current
                    // have a North, we switch this to a '-'
                    if (tile.HasSide(TileSide.North) &&
                        this.HasTile((x - 1, y)) &&
                        this.GetTile((x - 1, y)).HasSide(TileSide.North))
                    {
                        textMap[left, top] = '-';
                    }

                    // If both the tile to the left of this and the current
                    // do not have a north, we switch this to a ' '
                    if (!tile.HasSide(TileSide.North) &&
                        this.HasTile((x - 1, y)) &&
                        !this.GetTile((x - 1, y)).HasSide(TileSide.North))
                    {
                        textMap[left, top] = ' ';
                    }

                    // If both the tile to the right of this and the current
                    // have a North, we switch this to a '-'
                    textMap[left + 2, top] = '*';
                    if (tile.HasSide(TileSide.North) &&
                        this.HasTile((x + 1, y)) &&
                        this.GetTile((x + 1, y)).HasSide(TileSide.North))
                    {
                        textMap[left + 2, top] = '-';
                    }

                    // If both the tile to the left of this and the current
                    // do not have a north, we switch this to a ' '
                    if (!tile.HasSide(TileSide.North) &&
                        this.HasTile((x + 1, y)) &&
                        !this.GetTile((x + 1, y)).HasSide(TileSide.North))
                    {
                        textMap[left + 2, top] = ' ';
                    }


                    textMap[left, top + 2] = '*';
                    // If both the tile to the left of this and the current
                    // have a South, we switch this to a '-'
                    if (tile.HasSide(TileSide.South) &&
                        this.HasTile((x - 1, y)) &&
                        this.GetTile((x - 1, y)).HasSide(TileSide.South))
                    {
                        textMap[left, top + 2] = '-';
                    }

                    // If both the tile to the right of this and the current
                    // do not have a South, we switch this to a ' '
                    if (!tile.HasSide(TileSide.South) &&
                        this.HasTile((x - 1, y)) &&
                        !this.GetTile((x - 1, y)).HasSide(TileSide.South))
                    {
                        textMap[left, top + 2] = ' ';
                    }

                    textMap[left + 2, top + 2] = '*';
                    // If both the tile to the right of this and the current
                    // have a South, we switch this to a '-'
                    if (tile.HasSide(TileSide.South) &&
                        this.HasTile((x + 1, y)) &&
                        this.GetTile((x + 1, y)).HasSide(TileSide.South))
                    {
                        textMap[left + 2, top + 2] = '-';
                    }

                    // If both the tile to the left of this and the current
                    // do not have a South, we switch this to a ' '
                    if (!tile.HasSide(TileSide.South) &&
                        this.HasTile((x + 1, y)) &&
                        !this.GetTile((x + 1, y)).HasSide(TileSide.South))
                    {
                        textMap[left + 2, top + 2] = ' ';
                    }

                    // Center
                    textMap[left + 1, top + 1] = tile.TextChar;
                    textMap[left + 2, top + 1] = this.GetCharRep(tile, TileSide.East);
                    textMap[left, top + 1] = this.GetCharRep(tile, TileSide.West);
                    textMap[left + 1, top] = this.GetCharRep(tile, TileSide.North);
                    textMap[left + 1, top + 2] = this.GetCharRep(tile, TileSide.South);

                }
            }

            cursorX = ((cursorX - minX) * 2) + 1;
            cursorY = ((maxY - cursorY) * 2) + 1;
            textMap[cursorX, cursorY] = facing switch
            {
                TileSide.North => '^',
                TileSide.South => 'v',
                TileSide.East => '>',
                TileSide.West => '<',
                _ => textMap[cursorX, cursorY]
            };

            StringBuilder builder = new StringBuilder();
            // TODO: Optimize for row / column, this has bad spatial memory access.
            for (int y = 0; y < textMap.GetLength(1); y++)
            {
                for (int x = 0; x < textMap.GetLength(0); x++)
                {
                    builder.Append(textMap[x, y]);
                }
                builder.Append('\n');

            }

            return builder.ToString();
        }

        public char GetCharRep(ITile tile, TileSide facing)
        {
            WallType type = tile.GetSide(facing);
            return type switch
            {
                WallType.None => ' ',
                WallType.Door => '+',
                WallType.Wall => this.GetWallChar(facing),
                _ => throw new Exception("Illegal WallType detected."),
            };
            throw new Exception("Error in GetCharRep.");
        }

        public char GetDoorChar(TileSide facing)
        {
            return '+';
        }

        public char GetWallChar(TileSide facing)
        {
            switch (facing)
            {
                case TileSide.North:
                case TileSide.South:
                    return '-';
                case TileSide.East:
                case TileSide.West:
                    return '|';
            }
            throw new Exception($"Illegal WallChar facing: {facing}");
        }
    }

    public enum WallType
    {
        None,
        Wall,
        Door

    }

    public interface ITileObject
    {
        (int x, int y) Position { get; set; }
        char TextChar { get; }
        void Interact();
        ITileObject Spawn(ITile parent);
    }

    public interface ITile
    {
        (int x, int y) Position {get; }
        ITileObject Spawned {get; set;}
        bool HasSide(TileSide side);
        WallType GetSide(TileSide side);
        bool HasObject { get; }
        ITileObject Object { get; set; }
        void RemoveObject();
        char TextChar { get; }
        void SetSide(TileSide side, WallType isWall);
        void Interact();
        void SpawnObject();
    }

    [Serializable]
    internal class BasicTile : ITile
    {
        public ITileObject Spawned {get; set;}
        public (int x, int y) Position {get; private set;}

        private readonly Dictionary<TileSide, WallType> WallType = new Dictionary<TileSide, WallType>();

        public BasicTile((int x, int y) pos)
        {
            this.Position = pos;
            foreach (TileSide side in TileUtils.ALL)
            {
                WallType[side] = TileBuilder.WallType.Wall;
            }
        }

        public bool HasObject { get => this.Object != null; }
        public ITileObject Object { get; set; }

        public char TextChar { get => this.HasObject ? this.Object.TextChar : '.'; }

        public WallType GetSide(TileSide side)
        {
            return this.WallType[side];
        }

        public bool HasSide(TileSide side)
        {
            return this.WallType.ContainsKey(side) && this.WallType[side] != TileBuilder.WallType.None;
        }

        public void Interact()
        {
            if (this.HasObject)
            {
                this.Spawned.Interact();
            }
        }

        public void RemoveObject()
        {
            this.Object = null;
        }


        public void SetSide(TileSide side, WallType wallType)
        {
            this.WallType[side] = wallType;
        }

        public void SpawnObject()
        {
            // Do nothing
        }
    }

}