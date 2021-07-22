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

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float2 uv = coords.xy / uImageSize0.xy;
    float2 center = float2(0.5, 0.5);
    float4 color = tex2D(uImage0, coords);
    if ((atan2( center.y - coords.y, center.x - coords.x ) + 3.1415926) > clamp( 6.2831852 * uSaturation, 0.0, 6.2831852 )) {
	    color = float4(0, 0, 0, 0);
    }
    
    return float4(color.r * uColor.r, color.g * uColor.g, color.b * uColor.b, color.w * uOpacity);
}

technique Technique1
{
    pass CircleReveal
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}