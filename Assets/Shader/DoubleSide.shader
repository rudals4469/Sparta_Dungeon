Shader "Custom/ExplicitDoubleSided"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
        Pass
        {
            Cull Off // 컬링 비활성화
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            half _Glossiness;
            half _Metallic;
            
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }
            
            fixed4 frag(v2f i) : SV_Target
            {
                // 알베도 텍스처
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                
                // 기본 디퓨즈 라이팅
                float3 worldNormal = normalize(i.worldNormal);
                float nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                float3 diffuse = nl * _LightColor0.rgb;
                float3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb;
                col.rgb *= diffuse + ambient;
                
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
