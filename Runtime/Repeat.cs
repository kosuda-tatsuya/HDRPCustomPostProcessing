using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace cpp.hdrp
{
    [System.Serializable, VolumeComponentMenu("Post-processing/Custom/Repeat")]
    public class Repeat : CPPBase
    {

        public ClampedIntParameter repeatTimesX = new ClampedIntParameter(1, 1, 10);
        public ClampedIntParameter repeatTimesY = new ClampedIntParameter(1, 1, 10);

        public override bool IsActive()
        {
            return repeatTimesX.value > 1 || repeatTimesY.value > 1;
        }

        private static readonly int REPEATTIMES_ID = Shader.PropertyToID("_RepeatTimes");

        protected override string shaderName => "Hidden/Custom/Repeat";

        protected override void SetMaterialValue(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle dest)
        {
            material.SetTexture(MAINTEX_ID, source);
            Vector2 times = new Vector2Int(repeatTimesX.value, repeatTimesY.value);
            material.SetVector(REPEATTIMES_ID, times);
        }
    }
}
