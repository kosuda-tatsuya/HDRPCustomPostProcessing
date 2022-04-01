using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace cpp.hdrp
{
    [System.Serializable, VolumeComponentMenu("Post-processing/Custom/ColorGhostGlitch")]
    public class ColorGhostGlitch : CPPBase
    {

        public ColorParameter color1 = new ColorParameter(Color.blue);
        public ColorParameter color2 = new ColorParameter(Color.yellow);
        public ColorParameter color3 = new ColorParameter(Color.red);
        public ColorParameter color4 = new ColorParameter(Color.white);

        public ClampedIntParameter step = new ClampedIntParameter(0, 0, 100);

        private static readonly int[] CACHE_IDs = new int[4]
        {
            Shader.PropertyToID("_Frame1"),
            Shader.PropertyToID("_Frame2"),
            Shader.PropertyToID("_Frame3"),
            Shader.PropertyToID("_Frame4")
        };

        private static readonly int[] COLOR_IDs = new int[4]
        {
            Shader.PropertyToID("_Color1"),
            Shader.PropertyToID("_Color2"),
            Shader.PropertyToID("_Color3"),
            Shader.PropertyToID("_Color4")
        };

        private RTHandle _buffer1, _buffer2, _buffer3, _buffer4;

        protected override string shaderName => "Hidden/Custom/ColorGhostGlitch";

        private int _frame;

        public override void Setup()
        {
            base.Setup();
            _buffer1 = RTHandles.Alloc(Vector2.one, colorFormat: UnityEngine.Experimental.Rendering.GraphicsFormat.B10G11R11_UFloatPack32, name: "ColorGhostGlitch1");
            _buffer2 = RTHandles.Alloc(Vector2.one, colorFormat: UnityEngine.Experimental.Rendering.GraphicsFormat.B10G11R11_UFloatPack32, name: "ColorGhostGlitch2");
            _buffer3 = RTHandles.Alloc(Vector2.one, colorFormat: UnityEngine.Experimental.Rendering.GraphicsFormat.B10G11R11_UFloatPack32, name: "ColorGhostGlitch3");
            _buffer4 = RTHandles.Alloc(Vector2.one, colorFormat: UnityEngine.Experimental.Rendering.GraphicsFormat.B10G11R11_UFloatPack32, name: "ColorGhostGlitch4");
        }

        public override bool IsActive()
        {
            return step.value > 0;
        }

        protected override void SetMaterialValue(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle dest)
        {
            if (++_frame / step.value > CACHE_IDs.Length) { _frame = 1; }

            if (_frame % step.value == 0)
            {
                material.SetTexture(MAINTEX_ID, source);
                material.SetTexture(CACHE_IDs[0], _buffer1);
                material.SetTexture(CACHE_IDs[1], _buffer2);
                material.SetTexture(CACHE_IDs[2], _buffer3);
                material.SetTexture(CACHE_IDs[3], _buffer4);
                CoreUtils.DrawFullScreen(cmd, material, _buffer4, null, 1);
                (_buffer1, _buffer2, _buffer3, _buffer4) = (_buffer4, _buffer1, _buffer2, _buffer3);
            }

            material.SetTexture(MAINTEX_ID, source);
            material.SetColor(COLOR_IDs[0], color1.value);
            material.SetColor(COLOR_IDs[1], color2.value);
            material.SetColor(COLOR_IDs[2], color3.value);
            material.SetColor(COLOR_IDs[3], color4.value);
        }

        public override void Cleanup()
        {
            base.Cleanup();
            _buffer1.Release();
            _buffer2.Release();
            _buffer3.Release();
            _buffer4.Release();
        }

    }
}
