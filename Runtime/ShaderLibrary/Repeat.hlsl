#include "Common.hlsl"

int2 _RepeatTimes;

Varyings VertRepeat(Attributes input)
{
    Varyings output;
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
    output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
    output.texcoord = GetFullScreenTriangleTexCoord(input.vertexID);
    output.texcoord.x *= _RepeatTimes.x;
    output.texcoord.y *= _RepeatTimes.y;
    return output;
}

TEXTURE2D_X(_InputTexture);

float4 CustomPostProcess(Varyings input) : SV_Target
{
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
    float3 outColor = SAMPLE_TEXTURE2D_X(_InputTexture, s_linear_clamp_sampler, frac(input.texcoord)).xyz;
    return float4(outColor, 1);
}
