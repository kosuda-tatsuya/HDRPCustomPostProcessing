using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace cpp.hdrp
{
    [System.Serializable, VolumeComponentMenu("Post-processing/Custom/RetroGame")]
    public class RetroGame : CPPBase
    {

        public BoolParameter enable = new BoolParameter(false);
        public ClampedFloatParameter pixelSize = new ClampedFloatParameter(1, 1, 100);
        public ClampedFloatParameter brightnessThreshold = new ClampedFloatParameter(0.5f, 0, 1);
        public ClampedIntParameter resolution = new ClampedIntParameter(5, 2, 10);

        public override bool IsActive()
        {
            return enable.value;
        }

        protected override string shaderName => "Hidden/Custom/RetroGame";

        private static readonly int PIXELSIZE_ID = Shader.PropertyToID("_PixelSize");
        private static readonly int BRIGHTNESS_THRESHOLD_ID = Shader.PropertyToID("_BrightnessThreshold");
        private static readonly int RESOLUTION_ID = Shader.PropertyToID("_Resolution");

        protected override void SetMaterialValue(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle dest)
        {
            material.SetTexture(MAINTEX_ID, source);
            material.SetFloat(PIXELSIZE_ID, pixelSize.value);
            material.SetFloat(BRIGHTNESS_THRESHOLD_ID, brightnessThreshold.value);
            material.SetInt(RESOLUTION_ID, resolution.value);
        }
    }
}
