using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace cpp.hdrp
{
    [System.Serializable, VolumeComponentMenu("Post-processing/Custom/Feedback")]
    public class Feedback : CPPBase
    {

        RTHandle _buffer1, _buffer2;

        MaterialPropertyBlock _properties;

        public ClampedFloatParameter zoom = new ClampedFloatParameter(0f, 0f, 1f);

        private static readonly int ZOOM_ID = Shader.PropertyToID("_Zoom");
        private static readonly int FEEDBACK_TEX_ID = Shader.PropertyToID("_FeedbackTexture");

        protected override string shaderName => "Hidden/Custom/Feedback";

        public override void Setup()
        {
            base.Setup();
            _buffer1 = RTHandles.Alloc(Vector2.one, colorFormat: UnityEngine.Experimental.Rendering.GraphicsFormat.B10G11R11_UFloatPack32, name: "Feedback buffer1");
            _buffer2 = RTHandles.Alloc(Vector2.one, colorFormat: UnityEngine.Experimental.Rendering.GraphicsFormat.B10G11R11_UFloatPack32, name: "Feedback buffer2");
        }

        protected override void SetMaterialValue(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle dest)
        {
            if (_properties == null) { _properties = new MaterialPropertyBlock(); }

            _properties.SetFloat(ZOOM_ID, zoom.value);
            material.SetTexture(FEEDBACK_TEX_ID, _buffer1);
            material.SetTexture(MAINTEX_ID, source);
            CoreUtils.DrawFullScreen(cmd, material, _buffer2, _properties, 0);
            //HDUtils.DrawFullScreen(cmd, material, _buffer2, _properties, 0);
            (_buffer1, _buffer2) = (_buffer2, _buffer1);
        }

        public override void Cleanup()
        {
            base.Cleanup();
            _buffer1.Release();
            _buffer2.Release();
        }

        public override bool IsActive()
        {
            return zoom.value > 0;
        }

    }
}
