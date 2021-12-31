using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace cpp.hdrp
{
    [System.Serializable, VolumeComponentMenu("Post-processing/Custom/GlitchFreezeFrame")]
    public class GlitchFreezeFrame : CPPBase
    {

        public ClampedFloatParameter strength = new ClampedFloatParameter(0f, 0, 0.1f);
        public ClampedFloatParameter angle = new ClampedFloatParameter(0, -1f, 1f);

        private static readonly int STRENGTH_ID = Shader.PropertyToID("_Strength");
        private static readonly int ANGLE_ID = Shader.PropertyToID("_Angle");
        private static readonly int FREEZEFRAME_ID = Shader.PropertyToID("_FreezeFrame");

        protected override string shaderName => "Hidden/Custom/GlitchFreezeFrame";

        private RenderTexture _freezeFrame;

        public override bool IsActive()
        {
            if (strength.value <= 0 && _freezeFrame != null)
            {
                CoreUtils.Destroy(_freezeFrame);
                _freezeFrame = null;
            }

            return strength.value > 0;
        }

        protected override void SetMaterialValue(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle dest)
        {
            if (_freezeFrame == null) { CacheFreezeFrame(source, cmd); }

            material.SetTexture(MAINTEX_ID, source);
            material.SetTexture(FREEZEFRAME_ID, _freezeFrame);
            material.SetFloat(STRENGTH_ID, strength.value);
            material.SetFloat(ANGLE_ID, angle.value);
        }

        private void CacheFreezeFrame(RTHandle source, CommandBuffer cmd)
        {
            _freezeFrame = new RenderTexture(source.rt);
            CoreUtils.DrawFullScreen(cmd, material, _freezeFrame, null, 1);
        }

    }
}
