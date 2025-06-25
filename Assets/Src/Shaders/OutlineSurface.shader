Shader "YsoShader/OutlineSurface"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        //[HDR] _Emission ("Emission", Color) = (0,0,0)

        _OutlineColor("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineThickness("Outline Thickness", Range(0, 0.1)) = 0.03
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        fixed4 _Color;
        half _Glossiness;
        half _Metallic;
        //half _Emission;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            //o.Emission = _Emission;
            o.Alpha = c.a;
        }
        ENDCG
    
    
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
                float4 normal : NORMAL;
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