//#include "ShaderVariables.hlsl"
//#include "TextureXR.hlsl"
//#include "Color.hlsl"
#include "Common.hlsl"

// List of properties to control your post process effect

TEXTURE2D_X(_InputTexture);
Texture2D _FeedbackTexture;
float _Zoom;

float4 FullScreenPass(Varyings input) : SV_Target
{
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

    uint2 positionSS = input.positionCS.xy;
    float3 base = LOAD_TEXTURE2D_X(_InputTexture, positionSS).xyz;

    float2 uv = input.texcoord;
    float z = 0.1 * (_Zoom * 0.2 + 0.02);
    float zoom = 1.0 - z;
    uv -= 0.5;
    uv *= zoom;
    uv += 0.5;

    float3 c1 = SAMPLE_TEXTURE2D_X(_FeedbackTexture, s_linear_clamp_sampler, uv).rgb;
    float3 c3 = lerp(base, c1, 0.9);

    return float4(c3, 1);
}

