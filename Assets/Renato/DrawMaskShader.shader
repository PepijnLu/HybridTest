Shader "Custom/DrawMaskShader"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" { }
        _Mask ("Mask Texture", 2D) = "white" { }
        _UV ("Center UV", Vector) = (0.5, 0.5, 0, 0)
        _Radius ("Radius", Float) = 0.1
    }
    
    SubShader
    {
        Tags { "Queue"="Overlay" }
        Pass
        {
            ZWrite On
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            float2 _UV;
            float _Radius;
            sampler2D _MainTex;
            sampler2D _Mask;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                float2 center = _UV; // Center of the circle
                float dist = distance(i.uv, center);

                // Check if the pixel is within the circle radius
                if (dist < _Radius)
                {
                    return half4(0, 0, 0, 1); // Mask is black (cleared area)
                }
                else
                {
                    return tex2D(_Mask, i.uv); // Otherwise, use the mask texture
                }
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
