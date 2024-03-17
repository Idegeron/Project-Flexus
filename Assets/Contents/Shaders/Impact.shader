Shader "GameEngine/Impact"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_ImpactMap("Impact Map", 2D) = "white" {}
		_ImpactSize("Impact Size", Float) = 1
		_ImpactPosition("Impact Position", Vector) = (0,0,0,0)
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
			};

			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform sampler2D _ImpactMap;
			uniform float _ImpactSize;
			uniform float2 _ImpactPosition;
			
			fixed4 frag (v2f i ) : SV_Target
			{
				fixed4 finalColor;
				
				float2 uv_MainTex = i.ase_texcoord1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				
				finalColor = (tex2D(_MainTex, uv_MainTex ) + tex2D(_ImpactMap, (i.ase_texcoord1.xy * _ImpactSize + ( float2( 0.5,0.5 ) + ((_ImpactSize * _ImpactPosition) * float2( -1,-1 ))))));

				return finalColor;
			}
			ENDCG
		}
	}
}