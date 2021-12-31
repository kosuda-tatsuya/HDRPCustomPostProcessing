using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace cpp.hdrp
{
    public abstract class CPPBase : CustomPostProcessVolumeComponent, IPostProcessComponent
    {

        #region volume

        public virtual bool IsActive() => true;

        #endregion

        #region pass & feature

        protected Material material;

        protected virtual string shaderName => "";

        public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

        protected static readonly int MAINTEX_ID = Shader.PropertyToID("_InputTexture");

        public override void Setup()
        {
            material = CoreUtils.CreateEngineMaterial(shaderName);
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (material == null || IsActive() == false) { return; }

            SetMaterialValue(cmd, camera, source, destination);
            //HDUtils.DrawFullScreen(cmd, material, destination, null, 0);
            CoreUtils.DrawFullScreen(cmd, material, destination, null, 0);
        }

        protected abstract void SetMaterialValue(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle dest);

        public override void Cleanup()
        {
            CoreUtils.Destroy(material);
        }

        #endregion

    }
}
