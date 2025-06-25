Shader "YsoShader/MaskTexturedAnimated"
{
    Properties
    {
        _MainColor ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _MaskTex ("Mask", 2D) = "white" {}
        _isReverse ("isReverse",  Range(0,1)) = 0
        _StartAngle("Start Angle (Radians)", float) = 60.0
        _AnglePerUnit("Radians per Unit", float) = 0.2
        _Pitch("Pitch", float) = 0.02
        _UnrolledAngle("Unrolled Angle", float) = 1.5
        _Scale("Scale", Vector) = (1,1,1,1)
        _Offset("Offset", Vector) = (0,0,0,0)
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
            sampler2D _MainTex;
            float _isReverse;
            float _StartAngle;
            float _AnglePerUnit;
            float _Pitch;
            float _UnrolledAngle;
            float4 _Offset;
            float4 _Scale;

            float arcLengthToAngle(float angle) {
                float radical = sqrt(angle * angle + 1.0f);
                return _Pitch * (angle * radical + log(angle + radical));
            }

            v2f vert (appdata v)
            {
                float fromStart = v.vertex.z * _AnglePerUnit;

                float fromOrigin = _StartAngle - fromStart;
                float lengthToHere = arcLengthToAngle(fromOrigin);
                float lengthToStart = arcLengthToAngle(_StartAngle);

                if (fromStart < _UnrolledAngle) {
                    float lengthToSplit = arcLengthToAngle(_StartAngle - _UnrolledAngle);
                    v.vertex.z = (lengthToSplit - lengthToHere);
                    v.vertex.y = 0.0f;
                }
                else {
                    float radiusAtSplit = _Pitch * (_StartAngle - _UnrolledAngle);
                    float radius = _Pitch * fromOrigin;
                    float shifted = fromStart - _UnrolledAngle;

                    v.vertex.y = radiusAtSplit - cos(shifted) * radius;
                    v.vertex.z = sin(shifted) * radius;
                }

                v2f o;
                o.vertex = UnityObjectToClipPos((v.vertex + _Offset));
                o.vertex.x *= _Scale.x;
                o.vertex.y *= _Scale.y;
                o.uv = TRANSFORM_TEX(v.uv, _MaskTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _MainColor;
                col.a = _isReverse ? 1 - tex2D(_MaskTex, i.uv).a : tex2D(_MaskTex, i.uv).a;
                return col;
            }
            ENDCG
        }
    }
}
