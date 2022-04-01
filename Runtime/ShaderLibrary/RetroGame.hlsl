//#include "ShaderVariables.hlsl"
//#include "TextureXR.hlsl"
//#include "Common.hlsl"
//#include "Color.hlsl"
#include "Common.hlsl"

float _BrightnessThreshold;
float _PixelSize;
int _Resolution;
TEXTURE2D_X(_InputTexture);

inline float4 bmap(float3 c) {
	float gray = dot(c, float3(0.29, 0.59, 0.11));
	return lerp(float4(min(floor(c / _BrightnessThreshold + float3(0.5, 0.5, 0.5)), float3(1.0, 1.0, 1.0)), 0), float4(floor(c + float3(0.5, 0.5, 0.5)), 1.0), step(_BrightnessThreshold, gray));
}

inline void MinMaxColor(float2 bv, float2 sv, out float4 minc, out float4 maxc, out float bright) {
	minc = float4(1.0, 1.0, 1.0, 1.0);
	maxc = float4(0.0, 0.0, 0.0, 0.0);
	bright = 0.0;

	for (int i = 1; i < _Resolution; i++) {
		for (int j = 0; j < _Resolution; j++) {
            float4 c = bmap(SAMPLE_TEXTURE2D_X(_InputTexture, s_linear_clamp_sampler, (bv + float2(j, i)) / sv).rgb);
			minc = min(c, minc);
			maxc = max(c, maxc);
			bright += c.a;
		}
	}
}

float3 FMapColor(float4 c) {
	return lerp(c.rgb * _BrightnessThreshold, c.rgb, step(_BrightnessThreshold, c.a));
}

float4 CustomPostProcess(Varyings input) : SV_Target
{
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
    float2 originUV = input.texcoord;
    float2 uv, floorUV;
    PixelateUV(_PixelSize, originUV, uv, floorUV);
    float2 bv = float2(_Resolution, _Resolution) * floor(floorUV / _Resolution);
    float2 sv = _ScreenSize.xy / _PixelSize;
    float4 minc, maxc;
    float bright;
    MinMaxColor(bv, sv, minc, maxc, bright);

    bright = step(24, bright);

    if (maxc.r == minc.r && maxc.g == minc.g && maxc.b == minc.b)
    {
        minc.rgb = float3(0, 0, 0);
    }

    if (maxc.r == 0 && maxc.g == 0 && maxc.b == 0)
    {
        bright = 0;
        maxc.rgb = float3(0, 0, 1);
        minc.rgb = float3(0, 0, 0);
    }

    float3 c1 = FMapColor(float4(maxc.rgb, bright));
    float3 c2 = FMapColor(float4(maxc.rgb, bright));
    float4 cs = SAMPLE_TEXTURE2D_X(_InputTexture, s_linear_clamp_sampler, originUV);

    float3 d = cs.rgb + cs.rgb - (c1 + c2);
    float dd = d.r + d.g + d.b;

    float3 res = cs.rgb;
    float dither = 1.0;

    if (fmod(originUV.x + originUV.y, 2) == 1)
    {
        res = float3(dd >= -dither * 0.5 ? c1.r : c2.r, dd >= -dither * 0.5 ? c1.g : c2.g, dd >= -dither * 0.5 ? c1.b : c2.b);
    }
    else
    {
        res = float3(dd >= dither * 0.5 ? c1.r : c2.r, dd >= dither * 0.5 ? c1.g : c2.g, dd >= dither * 0.5 ? c1.b : c2.b);
    }
    
    return float4(res, cs.a);
}
