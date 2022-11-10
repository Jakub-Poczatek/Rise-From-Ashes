using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GlobalData
{

    public class Directions
    {
        public static Direction[] CardinalDirections = new Direction[] {
            new Direction(0,1), new Direction(1,0), new Direction(0, -1), new Direction(-1, 0),
            new Direction(1,1), new Direction(1, -1), new Direction(-1, -1), new Direction(-1, 1)
        };
    }
    public class Direction
    {
        public float x;
        public float z;

        public Direction(int x, int z)
        {
            this.x = x;
            this.z = z;
        }
    }
}
