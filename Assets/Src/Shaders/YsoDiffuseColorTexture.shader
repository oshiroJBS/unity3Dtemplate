Shader "YsoShader/YsoDiffuseColorTexture(Standard)" 
{
    Properties 
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" { }
    }
    SubShader 
    {
        Tags {"RenderType"="Opaque"}
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert noforwardadd

        fixed4 _Color;
        sampler2D _MainTex;
        
        struct Input
        {
            float2 uv_MainTex;
        };
        
        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb ;
            o.Alpha = c.a;
        }
        ENDCG
    }
    
    Fallback "Mobile/VertexLit"
    
}