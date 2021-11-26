namespace CaptainCoder.TileBuilder
{
    using System;
    using System.Collections.Generic;

    public class TileUtils
    {
        public static readonly TileSide[] WALLS = new TileSide[] { TileSide.North, TileSide.East, TileSide.South, TileSide.West };
        public static readonly TileSide[] ALL = new TileSide[] { TileSide.North, TileSide.East, TileSide.South, TileSide.West, TileSide.Top, TileSide.Bottom };

        public static readonly Dictionary<TileSide, string> LABEL = new Dictionary<TileSide, string>();

        static TileUtils()
        {
            LABEL[TileSide.North] = "North";
            LABEL[TileSide.East] = "East";
            LABEL[TileSide.South] = "South";
            LABEL[TileSide.West] = "West";
            LABEL[TileSide.Top] = "Top";
            LABEL[TileSide.Bottom] = "Bottom";
        }

        public static WallType Toggle(WallType prev)
        {
            return prev switch {
                WallType.None => WallType.Wall,
                WallType.Wall => WallType.Door,
                WallType.Door => WallType.None,
                _ => throw new Exception($"Illegal WallType Detected.")
            };
        }
    }
}