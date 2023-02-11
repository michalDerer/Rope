Shader "Dede/VertColorShader"
{
	Properties
	{
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }

		Pass
		{
			//Cull Off // turn off backface culling

			CGPROGRAM
			#pragma target 3.0

			#pragma vertex vert
			#pragma fragment frag

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
				float2 uv : TEXCOORD0;
			};

			v2f vert(appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.uv = v.uv;

				return o;
			}

			fixed4 frag(v2f i, fixed facing : VFACE) : SV_Target
			{
				//this feature only exists from shader model 3.0 onwards, 
				//so the shader needs to have the #pragma target 3.0 compilation directive.

				// VFACE input positive for frontbaces,
				// negative for backfaces. Output one
				// of the two colors depending on that.
				return facing > 0 ? i.color : i.color * 0.1f;
			}
			ENDCG
		}
	}
}
