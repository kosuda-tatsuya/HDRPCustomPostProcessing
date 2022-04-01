#include "Common.hlsl"

int _PixelSize;
TEXTURE2D_X(_InputTexture);

float4 CustomPostProcess(Varyings input) : SV_Target
{
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

    float2 origin = input.texcoord;
    float2 uv, floorUV;
    PixelateUV(_PixelSize, origin, uv, floorUV);

    //uint2 positionSS = clamp(uv, 0, 0.9999) * _ScreenSize.xy;
    //float3 outColor = LOAD_TEXTURE2D_X(_InputTexture, positionSS).xyz;
    float4 outColor = SAMPLE_TEXTURE2D_X(_InputTexture, s_linear_clamp_sampler, uv);

    return outColor;
}
