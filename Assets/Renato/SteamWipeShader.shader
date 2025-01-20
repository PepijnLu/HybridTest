Shader "Custom/SteamWipeShader"
{
    Properties
    {
        _SteamTexture ("Steam Texture", 2D) = "white" {}
        _ClearTexture ("Clear Texture", 2D) = "white" {}
        _Mask ("Wipe Mask", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard

        sampler2D _SteamTexture;
        sampler2D _ClearTexture;
        sampler2D _Mask;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 uv = IN.uv_MainTex;
            float mask = tex2D(_Mask, uv).r;
            float4 steamColor = tex2D(_SteamTexture, uv);
            float4 clearColor = tex2D(_ClearTexture, uv);
            o.Albedo = lerp(steamColor.rgb, clearColor.rgb, mask);
            o.Alpha = 1.0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
