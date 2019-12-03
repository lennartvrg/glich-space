// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/Glass"
{
Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull front 
        LOD 100
		
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
				float3 normal : NORMAL;
			};

			struct v2f
			{
			    float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 viewDirWorld : VIEW_DIR;
				float3 normDirWorld : NORM_DIR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = ComputeGrabScreenPos(v.vertex);
				
				o.viewDirWorld = UnityWorldSpaceViewDir(mul(unity_ObjectToWorld, v.vertex));
				o.normDirWorld = UnityObjectToWorldNormal(v.normal);
				
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{			
			    float3 r = reflect(-i.viewDirWorld, i.normDirWorld);
				
				half4 skyData = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, r);
                half3 skyColor = DecodeHDR (skyData, unity_SpecCube0_HDR);
				
				return float4(skyColor, 0.2);
			}
			ENDCG
		}
	}
}
