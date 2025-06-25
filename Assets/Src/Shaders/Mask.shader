Shader "YsoShader/Mask"
{
    Properties
    {
        _MainColor ("Color", Color) = (1,1,1,1)
        _MaskTex ("Mask", 2D) = "white" {}
        _isReverse("isReverse",  Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _MainColor;
            sampler2D _MaskTex;
            float4 _MaskTex_ST;
            float _isReverse;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MaskTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = _MainColor;
                col.a = _isReverse ? 1 - tex2D(_MaskTex, i.uv).a : tex2D(_MaskTex, i.uv).a;
                return col;
            }
            ENDCG
        }
    }
}
