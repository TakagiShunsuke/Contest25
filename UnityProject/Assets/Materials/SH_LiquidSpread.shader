/*=====
<SH_LiquidSpread.Shader>
���쐬�ҁFtei

�����e
�t�̊g�U�����̐ݒ�ƌv�Z

�����ӎ���

���X�V����
�@�@__Y25
�@_M04
D
18�F�v���O�����쐬�Ftei
19�F�����������A�R�����g�ǉ��Ftei
21�F�g�U�͈͂Ɗg�U���x��ʁX�Ōv�Z�ɕύX�Ftei

=====*/
Shader "Custom/SH_LiquidSpread_Unlit"
{
    // �t�̐����ɕK�v�ȃv���p�e�B
      Properties
    {
        _BaseColor ("Base Color", Color) = (0, 1, 0, 1)                     // �F
         _MaxSpread ("Max Spread Distance", Float) = 1.0                    // �g�U�ő勗��
        _SpreadDuration ("Spread Duration", Float) = 1.5                    // �g�U�ɂ����鎞��
        _Cutoff ("Cutoff Threshold", Range(0,1)) = 0.05                     // 臒l�ݒ�(���l�p)
        _FadeDuration ("Fade Duration", Float) = 1.5                        // �t�F�C�h����
        _FadeStartTime ("Fade Start Time", Float) = -1.0                    // �t�F�C�h�J�n����
        _StartTime ("Start Time", Float) = 0.0                              // �g�U�J�n����
        _LiquidTex ("Liquid Noise Texture", 2D) = "white" {}                // �e�N�X�`��
        _RandomSpreadPlus ("Random Spread Plus", Vector) = (1,1,1,1)        // �����_���g�U���v���X
        _RandomSpreadMinus ("Random Spread Minus", Vector) = (1,1,1,1)      // �����_���g�U���}�C�i�X

    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            Name "Unlit"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            // �ϐ��錾

            float4 _BaseColor;
            float _MaxSpread;
            float _SpreadDuration;
            float _Cutoff;
            float _StartTime;
            float _FadeStartTime;
            float _FadeDuration;
            TEXTURE2D(_LiquidTex);          
            SAMPLER(sampler_LiquidTex);
            float4 _RandomSpreadPlus;
            float4 _RandomSpreadMinus;

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                // UV�𒆐S��ɕϊ�
                float2 centeredUV = (i.uv - 0.5f);

                // ���S����̕���
                float2 dir = normalize(centeredUV);

                // ���̋���
                float dist = length(centeredUV);

                // ���Ԍo�߂ɂ��g�U���x����
                 float elapsed = _Time.y - _StartTime;
                 float t = saturate(elapsed / _SpreadDuration);    // 0~1���K��

                // �Ȃ߂炩�Ȋg�U�FeaseInOut
                float spreadCurve = t * t * (3.0f - 2.0f * t);

                // �ŏI�I�Ȋg�U�͈�
                float spread = spreadCurve * _MaxSpread;

                // Ver.�@ Direction�Ő���
                {
                    // // �������ƂɊg�U�␳
                    // float strench = 1.0f;

                    // // x���D��
                    // if(abs(dir.x) > abs(dir.y))
                    // {
                    //     if(dir.x > 0)
                    //     {
                    //         strench = _RandomSpreadPlus.x;
                    //     }
                    //     else
                    //     {
                    //         strench = _RandomSpreadPlus.z;
                    //     }
                    // }

                    // // y���D��
                    // else
                    // {
                    //     if(dir.y > 0)
                    //     {
                    //         strench = _RandomSpreadPlus.y;
                    //     }
                    //     else
                    //     {
                    //         strench = _RandomSpreadPlus.w;
                    //     }
                    // }

                    // // �΂ߕ����␳
                    // if(abs(dir.x - dir.y) < 0.2)
                    // {
                    //     if(dir.x > 0)
                    //     {
                    //         strench = _RandomSpreadMinus.x;
                    //     }
                    //     else
                    //     {
                    //         strench = _RandomSpreadMinus.z;
                    //     }
                    // }
                    // else if(abs(dir.x + dir.y) < 0.2)
                    // {
                    //     if(dir.x < 0)
                    //     {
                    //         strench = _RandomSpreadMinus.y;
                    //     }
                    //     else
                    //     {
                    //         strench = _RandomSpreadMinus.w;
                    //     }
                    // }

                    // // �Ō�ɋ�����␳
                    // dist /= strench;
                }
                
                // Ver.�A Rotation�Ő���
                {
                    // �p�x�擾(-�΁`��)
                    float angle = atan2(dir.y, dir.x);

                    // �p�x��0~1���K��
                    angle = (angle + 3.1415926) / (2.0 * 3.1415926);

                    // 8�����ɕ���
                    float sector = angle * 8.0;
                    float sectorIndex = floor(sector);
                    float sectorLerp = sector - sectorIndex;    // 0~1�̊�

                    // 8�����̃����_���p�����[�^
                    float stretchArray[8];
                    stretchArray[0] = _RandomSpreadPlus.x;   // �E
                    stretchArray[1] = _RandomSpreadMinus.x;  // �E��
                    stretchArray[2] = _RandomSpreadPlus.y;   // ��
                    stretchArray[3] = _RandomSpreadMinus.y;  // ����
                    stretchArray[4] = _RandomSpreadPlus.z;   // ��
                    stretchArray[5] = _RandomSpreadMinus.z;  // ����
                    stretchArray[6] = _RandomSpreadPlus.w;   // ��
                    stretchArray[7] = _RandomSpreadMinus.w;  // �E��

                    // �Z�N�^�[�Ԃ��Ԃ��Ċg�U�������߂�
                    float stretch = lerp(
                        stretchArray[(int)sectorIndex % 8],
                        stretchArray[((int)sectorIndex + 1) % 8],
                        sectorLerp
                        );

                    // �ŏI�I�ȋ����ɔ��f
                    dist /= stretch;
                }
                

                // �g�U�x�[�X�̃A���t�@
                float baseAlpha = smoothstep(spread, spread - _Cutoff, dist);

                // �t�̖͗l�e�N�X�`�����擾�i�K�v�Ȃ�e�N�X�`���ݒ�j
                float noise = SAMPLE_TEXTURE2D(_LiquidTex, sampler_LiquidTex, i.uv).r;

                // �t�̖͗l���|���Z
                float alpha = baseAlpha * noise;

                // �t�F�[�h�A�E�g�����ifade�J�n��A���X�ɓ����ɂ���j
                if (_FadeStartTime > 0)
                {
                    float fadeProgress = saturate((_Time.y - _FadeStartTime) / _FadeDuration);
                    alpha *= (1.0 - fadeProgress);
                }

                // ������������j��
                if (alpha <= 0.001)
                    discard;

                return float4(_BaseColor.rgb, alpha * _BaseColor.a);
            }
            ENDHLSL
        }
    }
}
