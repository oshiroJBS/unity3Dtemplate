Shader "YsoShader/YsoUnlitColor" 
{
    Properties 
    {
        _Color("Color", Color) = (1.000000,1.000000,1.000000,1.000000)
    }
    SubShader 
    {
        Tags {"RenderType"="Opaque"}
        LOD 100
        Pass 
        {
            Lighting Off
            ZWrite On
            Cull Back
            SetTexture[_] 
            {
                constantColor [_Color]
                Combine constant
            }
        }
    }
    
}