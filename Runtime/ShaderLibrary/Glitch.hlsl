//#include "ShaderVariables.hlsl"
//#include "TextureXR.hlsl"
//#include "Color.hlsl"
#include "Common.hlsl"

float _Speed;
float2 _Intensity;

TEXTURE2D_X(_InputTexture);

float4 CustomPostProcess(Varyings input) : SV_Target
{
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

    float2 uv = input.texcoord.xy;

    float2 seed = float2(uv.y, _Time.x * _Speed);
    float rand = RandomRange(seed, -1, 1);
    float2 finUV = float2(frac(step(0.3, abs(rand)) * _Intensity.x * rand + uv.x), frac(step(0.3, abs(rand)) * _Intensity.y * rand + uv.y));

    float3 outColor = SAMPLE_TEXTURE2D_X(_InputTexture, s_linear_clamp_sampler, finUV).rgb;

    return float4(outColor, 1);
}

