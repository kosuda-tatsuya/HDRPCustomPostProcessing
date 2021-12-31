using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace cpp.hdrp
{
    [System.Serializable, VolumeComponentMenu("Post-processing/Custom/Pixelate")]
    public class Pixelate : CPPBase
    {

        public ClampedIntParameter pixelSize = new ClampedIntParameter(1, 1, 100);

        public override bool IsActive()
        {
            return pixelSize.value > 1;
        }

        private static readonly int PIXELSIZE_ID = Shader.PropertyToID("_PixelSize");

        protected override string shaderName => "Hidden/Custom/Pixelate";

        protected override void SetMaterialValue(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle dest)
        {
            material.SetTexture(MAINTEX_ID, source);
            material.SetInt(PIXELSIZE_ID, pixelSize.value);
        }
    }
}
