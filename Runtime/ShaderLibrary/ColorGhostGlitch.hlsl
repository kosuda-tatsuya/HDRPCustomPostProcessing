//#include "ShaderVariables.hlsl"
//#include "TextureXR.hlsl"
//#include "Color.hlsl"
#include "Common.hlsl"

// List of properties to control your post process effect
float _Intensity;
TEXTURE2D_X(_InputTexture);

Texture2D _Frame1;
Texture2D _Frame2;
Texture2D _Frame3;
Texture2D _Frame4;

float3 _Color1;
float3 _Color2;
float3 _Color3;
float3 _Color4;

float4 CustomPostProcess(Varyings input) : SV_Target
{
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

    uint2 positionSS = input.positionCS.xy;
    float3 base = LOAD_TEXTURE2D_X(_InputTexture, positionSS).xyz;

    float2 uv = input.texcoord;
    float3 frame1Color = SAMPLE_TEXTURE2D_X_LOD(_Frame1, s_point_clamp_sampler, uv, 0).xyz;
    float3 frame2Color = SAMPLE_TEXTURE2D_X_LOD(_Frame2, s_point_clamp_sampler, uv, 0).xyz;
    float3 frame3Color = SAMPLE_TEXTURE2D_X_LOD(_Frame3, s_point_clamp_sampler, uv, 0).xyz;
    float3 frame4Color = SAMPLE_TEXTURE2D_X_LOD(_Frame4, s_point_clamp_sampler, uv, 0).xyz;

    float3 blend1 = (base - frame1Color) * _Color1;
    float3 blend2 = (frame1Color - frame2Color) * _Color2;
    float3 blend3 = (frame2Color - frame3Color) * _Color3;
    float3 blend4 = (frame3Color - frame4Color) * _Color4;

    float3 o1 = BlendScreen(base, blend1, 1);
    float3 o2 = BlendScreen(o1, blend2, 1);
    float3 o3 = BlendScreen(o2, blend3, 1);
    float3 o4 = BlendScreen(o3, blend4, 1);

    return float4(BlendScreen(base, base - o4, 1), 1);
}

float4 CopyTexture(Varyings input) : SV_Target
{
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
    uint2 positionSS = input.positionCS.xy;
    return float4(LOAD_TEXTURE2D_X(_InputTexture, positionSS).xyz, 1);
}
