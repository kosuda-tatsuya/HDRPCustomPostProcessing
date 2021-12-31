using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace cpp.hdrp
{
    [System.Serializable, VolumeComponentMenu("Post-processing/Custom/BorderNoise")]
    public class BorderNoise : CPPBase
    {

        public ClampedFloatParameter intensity = new ClampedFloatParameter(0, 0, 1);
        public ClampedFloatParameter noiseScale = new ClampedFloatParameter(100, 1, 1000);
        public ClampedFloatParameter speed = new ClampedFloatParameter(1, 0, 10);

        public override bool IsActive()
        {
            return intensity.value > 0;
        }

        protected override string shaderName => "Hidden/Custom/BorderNoise";

        private static readonly int INTENSITY_ID = Shader.PropertyToID("_Intensity");
        private static readonly int NOISESCALE_ID = Shader.PropertyToID("_NoiseScale");
        private static readonly int SPEED_ID = Shader.PropertyToID("_Speed");

        protected override void SetMaterialValue(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle dest)
        {
            material.SetTexture(MAINTEX_ID, source);
            material.SetFloat(INTENSITY_ID, intensity.value);
            material.SetFloat(NOISESCALE_ID, noiseScale.value);
            material.SetFloat(SPEED_ID, speed.value);
        }
    }
}
