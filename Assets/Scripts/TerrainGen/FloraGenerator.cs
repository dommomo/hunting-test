using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtDawn.TerrainGen
{
    public class FloraGenerator : MonoBehaviour
    {
        [Range(0,100)]
        public int fillPercentTrees;
        [Range(0, 100)]
        public int fillPercentBushes;


        private string seed;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
