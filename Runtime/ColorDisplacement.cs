using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace cpp.hdrp
{
    [System.Serializable, VolumeComponentMenu("Post-processing/Custom/ColorDisplacement")]
    public class ColorDisplacement : CPPBase
    {

        public ClampedFloatParameter intensity = new ClampedFloatParameter(0, 0, 1);
        public ClampedFloatParameter frequency = new ClampedFloatParameter(0, 0, 1);
        public Vector3Parameter shiftColor = new Vector3Parameter(new Vector3(3, 1.5f, 0));

        private static readonly int FREQUENCY_ID = Shader.PropertyToID("_Frequency");
        private static readonly int SHIFT_COLOR_ID = Shader.PropertyToID("_ShiftColor");

        protected override string shaderName => "Hidden/Custom/ColorDisplacement";

        public override bool IsActive()
        {
            return intensity.value > 0;
        }

        protected override void SetMaterialValue(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle dest)
        {
            material.SetFloat(FREQUENCY_ID, frequency.value);
            material.SetVector(SHIFT_COLOR_ID, shiftColor.value);
            material.SetTexture(MAINTEX_ID, source);
        }
    }
}
