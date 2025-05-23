/*=====
<SH_DeathEffect.Shader>
���쐬�ҁFtei

�����e
���S�G�t�F�N�g�����v�Z

�����ӎ���

���X�V����
�@�@__Y25
�@_M04
D
25�F�v���O�����쐬�Ftei

=====*/
Shader "Custom/SH_DeathEffect"
{
    // �G�t�F�N�g�����ɕK�v�ȃv���p�e�B
   Properties
    {
        _Color ("Main Color", Color) = (0.2, 1.0, 0.5, 0.8) // �J���[
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="transparent" }
        Pass
        {
            Name "RaymarchWithDepth"
            Tags { "LightMode" = "UniversalForward" }

            ZWrite On
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha     // �������p�̃u�����h�ݒ�

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            // �\���̒�`
            // ���_/�t���O�����g�\����
            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            struct FragOutput
            {
                float4 color : SV_Target;
                float depth : SV_Depth;
            };

            // // ���_�V�F�[�_�[�F���ʏ�̊e�_�̃��[���h�ʒu���擾
            // VS_OUT vert(VS_IN v)
            // {
            //     VS_OUT o;
            //     o.pos = UnityObjectToClipPos(v.vertex);
            //     o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
            //     return o;
            // }

            // ��������Ԋ֐�
            // �����P�Ffloat fFirstNum�F���l
            // �����Q�Ffloat fSecondNum�F���l
            // �����R�Ffloat fConstK�F���l
            // ��
            // �ߒl�F��Ԃ̍ŏ��l
            // ��
            // �T�v�F�I�u�W�F�N�g���m���Ԃ��Č�������ŏ��l���v�Z

            float SmoothMin(float fFirstNum, float fSecondNum, float fConstK)
            {
                float h = clamp(0.5 + 0.5 * (fSecondNum - fFirstNum) / fConstK, 0.0, 1.0);
                return lerp(fSecondNum, fFirstNum, h) - fConstK * h * (1.0 - h);
            }

            // �ϐ��錾
            float4 _Color;  // �J���[

            // �ő�256�̋���������悤�ɒ�`
            uniform int _nSphereCount;      // ���̂̐����v�Z
            uniform float4 _fSpheres[256];  // ���̂̃f�[�^ �ixyz: ���S, w: ���a�j

            // �������Q�b�g�֐�
            // �����Ffloat3 fPos�F���W
            // ��
            // �ߒl�F�v�Z��������
            // ��
            // �T�v�F�ł��߂����܂ł̋�����T��

            float GetDistance(float3 fPos)
            {
                float d = 1000.0;
                for (int i = 0; i < _nSphereCount; i++)
                {
                    float dist = length(fPos - _fSpheres[i].xyz) - _fSpheres[i].w;
                    d = SmoothMin(d, dist, 0.9);
                }
                return d;
            }

            // ���@���v�Z�֐�
            // �����Ffloat3 fPos�F���W
            // ��
            // �ߒl�F�@���x�N�g��
            // ��
            // �T�v�F�@���i�m�[�}���j��SDF����v�Z

            float3 GetNormal(float3 fPos)
            {
                float eps = 0.001;
                return normalize(float3(
                    GetDistance(fPos + float3(eps, 0, 0)) - GetDistance(fPos - float3(eps, 0, 0)),
                    GetDistance(fPos + float3(0, eps, 0)) - GetDistance(fPos - float3(0, eps, 0)),
                    GetDistance(fPos + float3(0, 0, eps)) - GetDistance(fPos - float3(0, 0, eps))
                ));
            }

            // ���_�V�F�[�_�[
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.worldPos = TransformObjectToWorld(IN.positionOS).xyz;
                return OUT;
            }

            // �t���O�����g�V�F�[�_�[�F�s�N�Z�����Ƃɋ��Ƀ��C���΂��ĐF������
            FragOutput frag(Varyings IN)
            {
                
                FragOutput OUT;

                // ���C�̋N�_�ƕ���
                float3 rayOrigin = GetCameraPositionWS(); // Unity 6 URP�̈��S�ȃJ�����ʒu�擾
                float3 rayDir = normalize(IN.worldPos - rayOrigin);
                float3 pos = rayOrigin;

                float dist;
                const int maxSteps = 64;
                for (int i = 0; i < maxSteps; i++)
                {
                    dist = GetDistance(pos);
                    if (dist < 0.001) break;
                    pos += rayDir * dist;
                }

                if (dist >= 0.001)
                {
                    OUT.color = float4(0, 0, 0, 0);
                    OUT.depth = IN.positionHCS.z / IN.positionHCS.w;
                    return OUT;
                }

                // ���C�e�B���O
                float3 normal = GetNormal(pos);
                Light mainLight = GetMainLight();
                float3 lightDir = normalize(mainLight.direction);
                float diff = saturate(dot(normal, lightDir));
                float3 col = lerp(float3(0.05, 0.2, 0.1), float3(0.3, 1.0, 0.6), diff);

                // �ŏI�J���[
                OUT.color = float4(col * _Color.rgb, _Color.a);

                // �[�x�������݁i�J������Ԃ���擾�j
                float4 clipPos = TransformWorldToHClip(pos);
                OUT.depth = clipPos.z / clipPos.w;

                return OUT;
            }

            ENDHLSL
        }
    }
}
