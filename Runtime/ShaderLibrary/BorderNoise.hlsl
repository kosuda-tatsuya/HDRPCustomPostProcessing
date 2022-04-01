//#include "ShaderVariables.hlsl"
//#include "TextureXR.hlsl"
//#include "Color.hlsl"
#include "Common.hlsl"

// List of properties to control your post process effect
float _Intensity;
float _NoiseScale;
float _Speed;
TEXTURE2D_X(_InputTexture);

float4 CustomPostProcess(Varyings input) : SV_Target
{
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

    float2 uv = input.texcoord;

    float noise = frac(Unity_SimpleNoise_float(float2(_Time.x * _Speed, uv.y), _NoiseScale));

    float4 c = LOAD_TEXTURE2D_X(_InputTexture, input.positionCS.xy);
    float blendAlpha = step(1 - _Intensity, noise);

    float3 c1 = Unity_Blend_HardMix_float3(c.rgb, c.rgb, blendAlpha);

    return float4(c1, c.a);
}

