Shader "YsoShader/AnimatedWavingTexture"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glow("Intensity", Range(0, 3)) = 1
        _WaveA("Wave A (dir, steepness, wavelength)", Vector) = (1,0,0.5,10)
        _WaveB("Wave B (dir, steepness, wavelength)", Vector) = (1,1,0.25,20)
        _WaveC("Wave C (dir, steepness, wavelength)", Vector) = (1,1,0.15,10)
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
        Cull Back
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM

            #pragma vertex vert alpha
            #pragma fragment frag alpha

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex  : SV_POSITION;
                half2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            half _Glow;
            float4 _WaveA, _WaveB, _WaveC;

            float3 GerstnerWave(float4 wave, float3 p) {
                float steepness = wave.z;
                float wavelength = wave.w;
                float k = 2 * UNITY_PI / wavelength;
                float c = sqrt(9.8 / k);
                float2 d = normalize(wave.xy);
                float f = k * (dot(d, p.xz) - c * _Time.y);
                float a = steepness / k;

                return float3(
                    d.x * (a * cos(f)),
                    a * sin(f),
                    d.y * (a * cos(f))
                    );
            }

            v2f vert(appdata_t v)
            {
                float3 p = float3(v.vertex.xyz);
                p += GerstnerWave(_WaveA, p);
                p += GerstnerWave(_WaveB, p);
                p += GerstnerWave(_WaveC, p);
                v2f o;
                o.vertex = UnityObjectToClipPos(p);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.texcoord) * _Color; 
                return col * _Glow;
            }
            ENDCG
        }
    }
}
