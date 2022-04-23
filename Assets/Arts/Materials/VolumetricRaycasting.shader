Shader "Unlit/VolumetricRaycasting"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,0,1)
        _Centre("Center", Vector) = (1,1,1,1)
        _Radius ("Radius", Float) = 1
        _SpecularPower ("_SpecularPower", Float) = 1
        _Gloss ("_Gloss", Float) = 1
        _Spacing ("_Spacing", Float) = 1
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
        }

        Pass
        {
            Blend One OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            #define STEPS 256
            #define MIN_DISTANCE 0.01
            #define STEP_SIZE 0.2


            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float3 _Centre;
            float _Radius;
            float _Spacing;
            float _SpecularPower;
            float _Gloss;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION; // Clip space
                float3 wPos : TEXCOORD1; // World position
            };

            v2f vert(appdata_full v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.wPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            float sdf_box(float3 pointPos, float3 cubePos, float3 width)
            {
                float x = max
                (pointPos.x - cubePos.x - float3(width.x / 2., 0, 0),
                 cubePos.x - pointPos.x - float3(width.x / 2., 0, 0)
                );
                float y = max
                (pointPos.y - cubePos.y - float3(width.y / 2., 0, 0),
                 cubePos.y - pointPos.y - float3(width.y / 2., 0, 0)
                );

                float z = max
                (pointPos.z - cubePos.z - float3(width.z / 2., 0, 0),
                 cubePos.z - pointPos.z - float3(width.z / 2., 0, 0)
                );
                float d = x;
                d = max(d, y);
                d = max(d, z);
                return d;
            }

            float vmax(float3 v)
            {
                return max(max(v.x, v.y), v.z);
            }

            float sdf_boxcheap(float3 p, float3 c, float3 s)
            {
                return vmax(abs(p - c) - s);
            }

            float sdf_blend(float d1, float d2, float a)
            {
                return a * d1 + (1 - a) * d2;
            }

            float sdf_smin(float a, float b, float k = 32)
            {
                float res = exp(-k * a) + exp(-k * b);
                return -log(max(0.0001, res)) / k;
            }

            bool sphereHit(float3 p)
            {
                return distance(p, _Centre) < _Radius;
            }

            float sdf_sphere(float3 p, float3 c, float r)
            {
                return distance(p, c) - r;
            }

            float sdf_sphereUnion(float3 c, float3 p1, float3 p2, float r)
            {
                float dist1 = distance(p1, c) - r;
                float dist2 = distance(p2, c) - r;
                return sdf_smin(dist1, dist2);
            }

            float sphereDistance(float3 p)
            {
                return distance(p, _Centre) - _Radius;
            }

            bool raymarchHit(float3 position, float3 direction)
            {
                for (int i = 0; i < STEPS; i++)
                {
                    if (sphereDistance(position))
                        return true;
                    position += direction * STEP_SIZE;
                }
                return false;
            }

            float3 normal(float3 p)
            {
                const float eps = 0.001;
                return normalize(float3(
                    sphereDistance(p + float3(eps, 0, 0)) - sphereDistance(p - float3(eps, 0, 0)),
                    sphereDistance(p + float3(0, eps, 0)) - sphereDistance(p - float3(0, eps, 0)),
                    sphereDistance(p + float3(0, 0, eps)) - sphereDistance(p - float3(0, 0, eps))
                ));
            }

            fixed4 simpleLambert(fixed3 normal)
            {
                fixed3 lightDir = normalize(_WorldSpaceLightPos0.xyz); // Light direction
                fixed3 lightCol = _LightColor0.rgb; // Light color
                fixed lambertianValue = max(dot(normal, lightDir), 0);
                fixed4 c;
                c.rgb = _Color * lightCol * lambertianValue;
                c.a = 1;

                // Specular
                fixed3 viewDirection = normalize(lightDir + normal);
                fixed3 h = normalize(lightDir + viewDirection);
                fixed s = pow(dot(normal, h), _SpecularPower) * _Gloss;
                c.rgb = _Color * lightCol * lambertianValue + s;
                c.a = 1;

                return c;
            }

            fixed4 renderSurface(float3 p)
            {
                float3 n = normal(p);
                return simpleLambert(n);
            }

            fixed4 raymarch(float3 position, float3 direction)
            {
                for (int i = 0; i < STEPS; i++)
                {
                    //float d = sdf_blend
                    //(
                    //    sdf_sphereUnion(_Centre, position- float3(_Spacing, 0, 0), position+ float3(_Spacing, 0, 0),_Radius),
                    //    sdf_sphere(position, _Centre, 1.8 * _Radius),
                    //    // sdf_sphereUnion(position, _Centre- float3(0,0,_Spacing) , _Centre + float3(0,0,_Spacing), _Radius),
                    //    //sdf_box(position, _Centre, float3(1,0.5,0.5)),
                    //    (sin(_Time.z) + 1.) / 2.
                    //);

                    float d = sdf_sphere(position, _Centre, 1.8 * _Radius);

                    if (d < MIN_DISTANCE)
                        return renderSurface(position) * (sin(_Time.yyyy)* 0.5f + 0.5f);
                    position += d * direction;
                }
                return fixed4(1, 1, 1, 0);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 worldPosition = i.wPos;
                float3 viewDirection = normalize(i.wPos - _WorldSpaceCameraPos); // ray from cam to point

                return raymarch(worldPosition, viewDirection);
            }
            ENDCG
        }
    }
}