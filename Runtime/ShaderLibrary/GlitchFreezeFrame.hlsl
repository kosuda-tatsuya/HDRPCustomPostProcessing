//#include "ShaderVariables.hlsl"
//#include "TextureXR.hlsl"
//#include "Color.hlsl"
#include "Common.hlsl"

TEXTURE2D_X(_InputTexture);
TEXTURE2D_X(_FreezeFrame);

float _Angle;
float _Strength;

float4 CustomPostProcess(Varyings input) : SV_Target
{
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

    float2 uv = input.texcoord;
    float s = sin(_Angle * 5);
    float c = cos(_Angle * 5);

    float2x2 mat = float2x2(c, -s, s, c);
    float2 stepuv = mul((uv - 0.5), mat) + 0.5;

    float uvx = clamp(uv.x - s * _Strength, 0, 1);
    float uvy = clamp(uv.y + c * _Strength, 0, 1);
    //float uvx = uv.x - s * _Strength;
    //float uvy = uv.y + c * _Strength;

    uvx = lerp(uvx, uv.x, step(0.5, stepuv.x));
    uvy = lerp(uvy, uv.y, step(0.5, stepuv.x));

    float3 c1 = SAMPLE_TEXTURE2D_X(_FreezeFrame, s_linear_clamp_sampler, float2(uvx, uvy)).rgb;
    float3 c2 = SAMPLE_TEXTURE2D_X(_InputTexture, s_linear_clamp_sampler, uv).rgb;
    float3 outc = lerp(c1, c2, step(0.5f, stepuv.x));

    return float4(outc, 1);
}

float4 CopyTexture(Varyings input) : SV_Target
{
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
    uint2 positionSS = input.positionCS.xy;
    return float4(LOAD_TEXTURE2D_X(_InputTexture, positionSS).rgb, 1);
}
