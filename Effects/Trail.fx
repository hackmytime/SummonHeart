sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;

float4 SoftTrail(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0 {
    float lightness = 1 - abs( (coords.y - 0.5) / 0.5);

    return float4(lightness * uColor.x, lightness * uColor.y, lightness * uColor.z, lightness * uOpacity) * sampleColor;
}

float4 SwordTrail(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0 {
    float slash = (coords.y < 0.05 * (1 - coords.x) ? 0.2 : 0) + (coords.y < 0.1 * (1 - coords.x) ? 0.05 : 0);
    float4 res = float4(0.5, 0.5, 0.5, 1) * (1 - coords.x) * (1 - coords.y) * sampleColor + slash;
    return float4(res.x, res.y, res.z, 1);
    //return float4(coords.x, coords.y, 0, 1);
}

float4 RacketTrail(float4 sampleColor: COLOR0, float2 coords: TEXCOORD0) : COLOR0 {
    return sampleColor;
}

technique Technique1 {
    pass SoftTrail {
        PixelShader = compile ps_2_0 SoftTrail();
    }

    pass SwordTrail {
        PixelShader = compile ps_2_0 SwordTrail();
    }

    pass RacketTrail {
        PixelShader = compile ps_2_0 RacketTrail();
    }
}