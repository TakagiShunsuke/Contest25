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
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Pass
        {
            ZWrite Off                          // �w�i�̂��̂�������悤��ZWrite�I�t
            Blend SrcAlpha OneMinusSrcAlpha     // �������p�̃u�����h�ݒ�

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // �\���̒�`
            // ���_�f�[�^���󂯎��
            struct VS_IN
            {
                float4 vertex : POSITION;
            };

            // ���_�V�F�[�_�[�̏o��
            struct VS_OUT
            {
                float4 pos : SV_POSITION;       // ��ԍ��W
                float3 worldPos : TEXCOORD0;    // ���[���h���W
            };

            // ���_�V�F�[�_�[�F���ʏ�̊e�_�̃��[���h�ʒu���擾
            VS_OUT vert(VS_IN v)
            {
                VS_OUT o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

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
                float fDistance = 1000; // �������i�[�p
                for (int i = 0; i < _nSphereCount; i++)
                {
                    float dis = length(fPos - _fSpheres[i].xyz) - _fSpheres[i].w;
                    fDistance = SmoothMin(fDistance, dis, 0.8); // ��3���������炩���̋����i�傫������Ɛڍ����L����j
                }
                return fDistance;
            }

            // ���@���v�Z�֐�
            // �����Ffloat3 fPos�F���W
            // ��
            // �ߒl�F�@���x�N�g��
            // ��
            // �T�v�F�@���i�m�[�}���j��SDF����v�Z

            float3 getNormal(float3 fPos)
            {
                float eps = 0.001; // ��������
                float3 dx = float3(eps, 0, 0);
                float3 dy = float3(0, eps, 0);
                float3 dz = float3(0, 0, eps);

                return normalize(float3(
                    GetDistance(fPos + dx) - GetDistance(fPos - dx),
                    GetDistance(fPos + dy) - GetDistance(fPos - dy),
                    GetDistance(fPos + dz) - GetDistance(fPos - dz)
                ));
            }
           
            fixed4 _Color;  // �J���[

            // �t���O�����g�V�F�[�_�[�F�s�N�Z�����Ƃɋ��Ƀ��C���΂��ĐF������
            fixed4 frag(VS_OUT i) : SV_Target
            {
                
                float3 rayOrigin = _WorldSpaceCameraPos; // ���C�̏o���_�i�J�����j
                float3 rayDir = normalize(i.worldPos - rayOrigin); // ���ʂ̃s�N�Z���֌���������

                float3 pos = rayOrigin; // ���݂̃��C�̈ʒu
                float dist = 0;
                
                // �ő�64�X�e�b�v�܂Ői�߂ċ��Ƃ̌�����T��
                for (int j = 0; j < 64; j++)
                {
                    // ���_����̋������擾
                    dist = GetDistance(pos); // �����ŕ������̋������g��
                    
                    if (dist < 0.001) break; // ���������������q�b�g
                    pos += rayDir * dist; // �q�b�g���Ȃ���΋����Ԃ�i��
                }

                if (dist < 0.001)
                {
                    // ���C�g�̌������@���v�Z
                    float3 normal = getNormal(pos); // �X���C���\�ʂ̖@��

                    // �V�[����Directional Light�̌���
                    float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);

                    // Lamabert�g�U��(0~1)
                    float diff = saturate(dot(normal, lightDir));


                    // �g�D�[�����i�K
                    // �F��`
                    float3 shadowColor = float3(0.05, 0.2, 0.1); // �e
                    float3 lightColor  = float3(0.3, 1.0, 0.6);  // ���邢

                    // �A�e�̊��炩���𒲐�
                    float shade = smoothstep(0.3, 0.7, diff);

                    // ���邳�ɉ����ĐF����
                    float3 col = lerp(shadowColor, lightColor, shade);
                    
                    // �o�́����l����
                    return float4(col, _Color.a);
                }
                else
                {
                    return float4(0, 0, 0, 0);
                }
            }

            ENDCG
        }
    }
}
