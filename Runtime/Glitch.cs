using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace cpp.hdrp
{
    [System.Serializable, VolumeComponentMenu("Post-processing/Custom/Glitch")]
    public class Glitch : CPPBase
    {

        public ClampedFloatParameter speed = new ClampedFloatParameter(1, 0, 1);
        public Vector2Parameter intensity = new Vector2Parameter(new Vector2(0f, 0f));

        private static readonly int SPEED_ID = Shader.PropertyToID("_Speed");
        private static readonly int INTENSITY_ID = Shader.PropertyToID("_Intensity");

        protected override string shaderName => "Hidden/Custom/Glitch";

        public override bool IsActive()
        {
            return intensity.value.x != 0 || intensity.value.y != 0;
        }

        protected override void SetMaterialValue(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle dest)
        {
            material.SetTexture(MAINTEX_ID, source);
            material.SetFloat(SPEED_ID, speed.value);
            material.SetVector(INTENSITY_ID, intensity.value);
        }

    }
}
