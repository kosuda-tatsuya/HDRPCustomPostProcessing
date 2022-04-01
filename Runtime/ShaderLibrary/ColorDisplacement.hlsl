//#include "ShaderVariables.hlsl"
//#include "TextureXR.hlsl"
//#include "Color.hlsl"
//#include "CustomPassCommon.hlsl"
#include "Common.hlsl"

// List of properties to control your post process effect
float _Frequency;
float3 _ShiftColor;
TEXTURE2D_X(_InputTexture);

float4 CustomPostProcess(Varyings input) : SV_Target
{
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

    float2 uv = input.texcoord;
    uint2 positionSS = input.positionCS.xy;
    float4 c = LOAD_TEXTURE2D_X(_InputTexture, positionSS);
    float3 c1 = sin(c.rgb * (6.2 + _Frequency * 40.0) + _Time.y + _ShiftColor) * 0.5 + 0.5;
    return float4(c1, c.a);
}

