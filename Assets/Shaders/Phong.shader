Shader "Unlit/Phong"
{
    Properties
    {    
        _Color ("Color", Color) = (1, 1, 1, 1)
        _Albedo ("Albedo", 2D) = "white" {}
        
        [NoScaleOffset] [Normal] _Normal ("Normal", 2D) = "bump" {}        
        _Glossiness ("Glossiness", Int) = 64
        
        [NoScaleOffset] _Parallax ("Parallax", 2D) = "black" {}
        _ParallaxStrength ("Parallax Strength", Range(0, 0.1)) = 0
        
        _Ambient ("Ambient", Color) = (1, 1, 1, 1)
        _AmbientFactor ("Ambient Factor", Range(0, 1)) = 0.1
        _Emissive ("Emissive", Color) = (0, 0, 0, 1)
    }
    SubShader
    {
        Pass
        {
            Tags { "LightMode" = "ForwardBase" }

            CGPROGRAM
            #pragma multi_compile_fwdbase_fullshadows nolightmap nodirlightmap nodynlightmap novertexlight
            #pragma target 3.0
            
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 tangent : TANGENT;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct vertex2Fragment
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 worldPos : WORLD_POSITION;
                float3x3 tangentToWorld : TBN;
                
                SHADOW_COORDS(1)
            };
            
            uniform sampler2D _Albedo;
            float4 _Albedo_ST;
            
            //---------------//
            // VERTEX SHADER //
            //---------------//
            vertex2Fragment vert (appdata i)
            {
                vertex2Fragment o;
                
                o.pos = UnityObjectToClipPos(i.vertex);
                o.worldPos = mul(unity_ObjectToWorld, i.vertex);
                o.uv = TRANSFORM_TEX(i.uv, _Albedo);


                float3 normalWorld = normalize(mul(unity_ObjectToWorld, float4(i.normal, 0.0)).xyz);
                float3 tangentWorld = normalize(mul(unity_ObjectToWorld, float4(i.tangent, 0.0)).xyz);
                float3 bitangentWorld = normalize(cross(normalWorld, tangentWorld));
                
                o.tangentToWorld = float3x3(tangentWorld, bitangentWorld, normalWorld);
                
                TRANSFER_SHADOW(o)              
                
                return o;
            }
            
            //-----------------//
            // FRAGMENT SHADER //
            //-----------------//
            uniform fixed4 _Color;
            
            uniform int _Glossiness;
            uniform sampler2D _Normal;
            
            uniform float _ParallaxStrength;
            uniform sampler2D _Parallax;
           
            uniform fixed4 _Ambient;
            uniform fixed _AmbientFactor;
            
            uniform fixed4 _Emissive;
                                    
            /**
             * Ambient is calculated with the following equation:
             *  color = (Ambient strenght factor) * (The current light source - Unity3D defined) * (The fragments base color)
             */
            float3 ambient()
            {
                return _LightColor0.rgb * _Ambient* max(_AmbientFactor, 0.0);
            }
            
            /**
             * Emissive is just a fixed color which overrides every other lighting output.
             */
            float3 emissive()       
            {
                return _Emissive.rgb;
            }
            
            /**
             * Diffuse is calculated by getting the cos(θ) between the fragment normal and the direction
             * to the light source and adding in the light color.
             * Since the vectors _WorldSpaceLightPos0 and normalize(norm) are both of magnitude 1, we can use
             * the dot() production to calculate cos(θ). At last we also take the _LightColor0 into account.
             */
            float3 diffuse(float3 normal, float3 lightDir)
            {
                return _LightColor0.rgb * max(dot(normal, lightDir), 0.0);
            }
            
            /**
             * The specular component is calculated with the following formular:
             *  cspec = sspec (v · r)^mgls
             *
             * v = Direction vector to the camera = normalized(_WorldSpaceCameraPos - vert (vert must be in world space))
             * l = Direction to light source = normalized(_WorldSpaceLightPos0 (don't need to substract vert since directional light))
             * r = reflection vector = r = 2(n · l)n − l
             * mgls = Glossiness of the specular highlight
             */
            float3 specular(float3 normal, float3 lightDir, float3 viewDir)
            {  
                float3 halfway = normalize(lightDir + viewDir);
                float specFactor = max(dot(normal, halfway), 0.0);
                return _LightColor0 * pow(specFactor, _Glossiness);
            }
            
            float2 parallax(float2 texCoords, float3 viewDir)
            {	
                float height =  tex2D(_Parallax, texCoords).x;    
                float2 p = viewDir.xy * (height * _ParallaxStrength);
                return texCoords - p; 
            }

            float4 frag (vertex2Fragment input) : SV_Target
            {
                float3 lightDirWorld = normalize(_WorldSpaceLightPos0.xyz);
                float3 viewDirWorld =  normalize(_WorldSpaceCameraPos.xyz - input.worldPos.xyz);
            
                float2 texCoords = parallax(input.uv, viewDirWorld);
                float3 normal = mul(input.tangentToWorld, UnpackNormal(tex2D(_Normal, texCoords)));

                fixed shadow = SHADOW_ATTENUATION(input);
                
                return float4((
                    (specular(normal, lightDirWorld, viewDirWorld) +
                    diffuse(normal, lightDirWorld)) * shadow +
                    ambient() +
                    emissive()
                ) * tex2D(_Albedo, texCoords) * _Color, 1.0);
            }
            ENDCG
        }
        
        Pass
        {
            Tags { "LightMode" = "ForwardAdd"}
            Blend One One
            
            CGPROGRAM
            #pragma multi_compile_fwdadd_fullshadows
            #pragma target 3.0

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 tangent : TANGENT;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct vertex2Fragment
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 worldPos : WORLD_POSITION;
                float3x3 tangentToWorld : TBN;
                
                SHADOW_COORDS(1)
            };
            
            uniform sampler2D _Albedo;
            float4 _Albedo_ST;
            
            //---------------//
            // VERTEX SHADER //
            //---------------//
            vertex2Fragment vert (appdata v)
            {
                vertex2Fragment o;
                
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _Albedo);        
                
                float3 normalWorld = normalize(mul(unity_ObjectToWorld, float4(v.normal, 0.0)).xyz);
                float3 tangentWorld = normalize(mul(unity_ObjectToWorld, float4(v.tangent, 0.0)).xyz);
                float3 bitangentWorld = normalize(cross(normalWorld, tangentWorld));
                
                o.tangentToWorld = float3x3(tangentWorld, bitangentWorld, normalWorld);
                
                TRANSFER_SHADOW(o)
               
                return o;
            }
            
            //-----------------//
            // FRAGMENT SHADER //
            //-----------------//
            uniform fixed4 _Color;
            
            uniform sampler2D _Normal;
            uniform int _Glossiness;
            
            uniform sampler2D _Parallax;
            uniform float _ParallaxStrength;
            
            /**
             * Diffuse is calculated by getting the cos(θ) between the fragment normal and the direction
             * to the light source and adding in the light color.
             * Since the vectors _WorldSpaceLightPos0 and normalize(norm) are both of magnitude 1, we can use
             * the dot() production to calculate cos(θ). At last we also take the _LightColor0 into account.
             */
            float3 diffuse(float3 normal, float3 lightDir)
            {
                return _LightColor0.rgb * max(dot(normal, lightDir), 0.0);
            }
            
            /**
             * The specular component is calculated with the following formular:
             *  cspec = sspec (v · r)^mgls
             *
             * v = Direction vector to the camera = normalized(_WorldSpaceCameraPos - vert (vert must be in world space))
             * l = Direction to light source = normalized(_WorldSpaceLightPos0 (don't need to substract vert since directional light))
             * r = reflection vector = r = 2(n · l)n − l
             * mgls = Glossiness of the specular highlight
             */
            float3 specular(float3 normal, float3 lightDir, float3 viewDir)
            {  
                float3 halfway = normalize(lightDir + viewDir);
                float specFactor = max(dot(normal, halfway), 0.0);
                return _LightColor0 * pow(specFactor, _Glossiness);
            }
            
            float2 parallax(float2 texCoords, float3 viewDir)
            {	
                float height =  tex2D(_Parallax, texCoords).x;    
                float2 p = viewDir.xy * (height * _ParallaxStrength);
                return texCoords - p; 
            }

            float4 frag (vertex2Fragment input) : SV_Target
            {   
                float3 lightDirWorld = _WorldSpaceLightPos0.xyz - input.worldPos.xyz;
                fixed distance = length(lightDirWorld);
                
                lightDirWorld = lightDirWorld / distance;
                float3 viewDirWorld =  normalize(_WorldSpaceCameraPos.xyz - input.worldPos.xyz);
            
                float2 texCoords = parallax(input.uv, viewDirWorld);
                float3 normal = mul(input.tangentToWorld, UnpackNormal(tex2D(_Normal, texCoords)));

                float atten = 1.0 / (1.0 + pow(distance, 2) * unity_4LightAtten0.z);
                fixed shadow = SHADOW_ATTENUATION(input);

                return float4((
                    specular(normal, lightDirWorld, viewDirWorld) +
                    diffuse(normal, lightDirWorld)
                ) * shadow * atten * tex2D(_Albedo, texCoords) * _Color, 1.0);
            }
            ENDCG
        }
        
        Pass
        {
            Tags {"LightMode"="ShadowCaster"}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #include "UnityCG.cginc"

            struct v2f { 
                float4 pos : POSITION;
                float3 vec : TEXCOORD0;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                o.vec = mul(unity_ObjectToWorld, v.vertex).xyz - _LightPositionRange.xyz;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                return UnityEncodeCubeShadowDepth ((length(i.vec) + unity_LightShadowBias.x) * _LightPositionRange.w);
            }
            ENDCG
        }
    }
}
