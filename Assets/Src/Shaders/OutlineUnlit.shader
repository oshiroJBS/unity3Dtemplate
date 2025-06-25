Shader "YsoShader/OutlineUnlit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _OutlineColor("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineThickness("Outline Thickness", Range(0, 0.1)) = 0.03
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry"}

        Pass
        {
            CGPROGRAM
            //Include usefull functions
            #include "UnityCG.cginc"

            //Define Vertex and fragment shader
            #pragma vertex vert
            #pragma fragment frag

            //Texture and its transform
            sampler2D _MainTex;
            float4 _MainTex_ST;

            //Texture's tint
            fixed4 _Color;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            //Vertex Shader
            v2f vert (appdata v)
            {
                v2f o;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //Convert the vertex position from object space to clip space
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            //Fragment Shader
            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                return col;
            }
            ENDCG
        }

        //Second pass where outline are render
        Pass
        {
            Cull front

            CGPROGRAM
            //Include usefull functions
            #include "UnityCG.cginc"

            //Define Vertex and fragment shader
            #pragma vertex vert
            #pragma fragment frag

            fixed4 _OutlineColor;
            float _OutlineThickness;

            //Object datas to put into the vertex shader
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            //Datas used to generate fragments and can be read by the fragment shader
            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            //Vertex Shader
            v2f vert(appdata v)
            {
                v2f o;
                //Convert the vertex position from object space to clip space
                o.vertex = UnityObjectToClipPos(v.vertex + normalize(v.normal) * _OutlineThickness);
                return o;
            }

            //Fragment Shader
            fixed4 frag(v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }
    }

    //Adds stuff we didn't implement like shadows and meta passes
    FallBack "Standard"
}
