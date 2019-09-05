 Shader "ParticleShader" {
	Properties {
		_size ("ParticleSize", float) = 0.3
		_col ("int", float) = 0.3

	}
	SubShader {
        ZWrite Off
        Blend One One
        Cull off
        
        Pass {
            Tags { "RenderType"="Transparent" "Queue"="Transparent+1" }
	        CGPROGRAM
	        
	        #pragma vertex vert
			#pragma geometry geom
	        #pragma fragment frag
	         
	        #include "UnityCG.cginc"
	        
        	struct Particle {
        		float3 pos;
				float3 vec;
        	};
        	
        	StructuredBuffer<Particle> Particles;
            float _size;
			float _col;

            static float2 trianglePos[3] = {float2(0.0,0.1),
                                            float2(0.1,-0.0577),
                                            float2(-0.1,-0.0577)};

            static float2 triangleUV[3] = { float2(0,2),
                                            float2(1.732,-1),
                                            float2(-1.732,-1)};
	        
	        struct VSOut {
	            float4 pos : POSITION;
	            float2 uv : TEXCOORD0;
	            fixed4 col : COLOR;
	        };
            struct Finput {
	            float4 pos : SV_POSITION;
	            float2 uv : TEXCOORD0;
	            fixed4 col : COLOR;
	        };
	        
			VSOut vert (uint id : SV_VertexID){
	            VSOut output;
	            output.pos = float4(Particles[id].pos, 1);
	            output.uv = float2(0,0);
                output.col = fixed4(abs(Particles[id].vec),1);
	             
	            return output;
	       	}
	       	
		   	[maxvertexcount(3)]
		   	void geom (point VSOut input[1], inout TriangleStream<Finput> outStream){
		     	Finput output;
		     	
		      	[unroll]for(int i=0; i<3; i++){
                    output.pos = mul(UNITY_MATRIX_P, mul(UNITY_MATRIX_V, input[0].pos) + float4(trianglePos[i]*_size,0,0));
                    output.uv = triangleUV[i];
                    output.col = input[0].col*_col;
                    outStream.Append(output);
		      	}
		      	outStream.RestartStrip();
		   	}
			
	        fixed4 frag (VSOut i) : COLOR {
				float alpha = 1-distance( fixed2(0,0),i.uv);
	        	float4 col =  i.col * saturate(alpha);
		        
	            return col;
	        }
	         
	        ENDCG
	     } 
     }
 }