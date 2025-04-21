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
22�F�e�ϐ��������Ftei

=====*/
Shader "Custom/SH_LiquidSpread_Unlit"
{
    // �t�̐����ɕK�v�ȃv���p�e�B
      Properties
    {
        _BaseColor ("Base Color", Color) = (0, 1, 0, 1)                     // �F
         _MaxSpread ("Max Spread Distance", Float) = 1.0                    // �g�U�ő勗���i�L���j
        _SpreadDuration ("Spread Duration", Float) = 1.5                    // �g�U�ɂ����鎞��
        _Cutoff ("Cutoff Threshold", Range(0,1)) = 0.05                     // 臒l�ݒ�(���l�p)
        _FadeDuration ("Fade Duration", Float) = 1.5                        // �t�F�C�h����
        _FadeStartTime ("Fade Start Time", Float) = -1.0                    // �t�F�C�h�J�n����
        _StartTime ("Start Time", Float) = 0.0                              // �g�U�J�n����
        _LiquidTex ("Liquid Noise Texture", 2D) = "white" {}                // �e�N�X�`��
        _RandomSpreadBottomLeftToTopRight ("Random Spread BLToTR", Vector) = (1,1,1,1)      // �����_���g�U���i����  x = y �����j
        _RandomSpreadTopLeftToBottomRight ("Random Spread TLToBR", Vector) = (1,1,1,1)      // �����_���g�U���i���� -x = y �����j

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

            float4 _BaseColor;      // �e�ϐ��R�}���h��L"Priorities"�̂Ƃ���ɎQ��
            float _MaxSpread;
            float _SpreadDuration;
            float _Cutoff;
            float _StartTime;
            float _FadeStartTime;
            float _FadeDuration;
            TEXTURE2D(_LiquidTex);          
            SAMPLER(sampler_LiquidTex);                 // �T���v���[�e�N�X�`��
            float4 _RandomSpreadBottomLeftToTopRight;
            float4 _RandomSpreadTopLeftToBottomRight;

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
                float2 fCenteredUV = (i.uv - 0.5f);

                // ���S����̕���
                float2 fDirection = normalize(fCenteredUV);

                // ���̋���
                float fDistance = length(fCenteredUV);

                // ���Ԍo�߂ɂ��g�U���x����
                 float fElapsed = _Time.y - _StartTime;
                 float t = saturate(fElapsed / _SpreadDuration);    // 0~1���K�� (�C�[�W���O�ύX�l)

                // �Ȃ߂炩�Ȋg�U�v�Z�FeaseInOut
                float fSpreadCurve = t * t * (3.0f - 2.0f * t);

                // �ŏI�I�Ȋg�U�͈�
                float fSpread = fSpreadCurve * _MaxSpread;

                // Ver.�@ Direction�Ő���
                {
                    // // �������ƂɊg�U�␳
                    // float strench = 1.0f;

                    // // x���D��
                    // if(abs(fDirection.x) > abs(fDirection.y))
                    // {
                    //     if(fDirection.x > 0)
                    //     {
                    //         strench = _RandomSpreadBottomLeftToTopRight.x;
                    //     }
                    //     else
                    //     {
                    //         strench = _RandomSpreadBottomLeftToTopRight.z;
                    //     }
                    // }

                    // // y���D��
                    // else
                    // {
                    //     if(fDirection.y > 0)
                    //     {
                    //         strench = _RandomSpreadBottomLeftToTopRight.y;
                    //     }
                    //     else
                    //     {
                    //         strench = _RandomSpreadBottomLeftToTopRight.w;
                    //     }
                    // }

                    // // �΂ߕ����␳
                    // if(abs(fDirection.x - fDirection.y) < 0.2)
                    // {
                    //     if(fDirection.x > 0)
                    //     {
                    //         strench = _RandomSpreadTopLeftToBottomRight.x;
                    //     }
                    //     else
                    //     {
                    //         strench = _RandomSpreadTopLeftToBottomRight.z;
                    //     }
                    // }
                    // else if(abs(fDirection.x + fDirection.y) < 0.2)
                    // {
                    //     if(fDirection.x < 0)
                    //     {
                    //         strench = _RandomSpreadTopLeftToBottomRight.y;
                    //     }
                    //     else
                    //     {
                    //         strench = _RandomSpreadTopLeftToBottomRight.w;
                    //     }
                    // }

                    // // �Ō�ɋ�����␳
                    // fDistance /= strench;
                }
                
                // Ver.�A Rotation�Ő���
                {
                    // �p�x�擾(-�΁`��)
                    float fAngle = atan2(fDirection.y, fDirection.x);

                    // �p�x��0~1���K��
                    fAngle = (fAngle + 3.1415926) / (2.0 * 3.1415926);

                    // 8�����ɕ���
                    float fSector = fAngle * 8.0;           // 8�����̃Z�N�^�[
                    float fSectorIndex = floor(fSector);    // �Z�N�^�[�C���f�N�X
                    float fSectorLerp = fSector - fSectorIndex;    // 0~1�̊�

                    // 8�����̃����_���p�����[�^
                    float fStretchArray[8];
                    fStretchArray[0] = _RandomSpreadBottomLeftToTopRight.x;   // �E
                    fStretchArray[1] = _RandomSpreadTopLeftToBottomRight.x;  // �E��
                    fStretchArray[2] = _RandomSpreadBottomLeftToTopRight.y;   // ��
                    fStretchArray[3] = _RandomSpreadTopLeftToBottomRight.y;  // ����
                    fStretchArray[4] = _RandomSpreadBottomLeftToTopRight.z;   // ��
                    fStretchArray[5] = _RandomSpreadTopLeftToBottomRight.z;  // ����
                    fStretchArray[6] = _RandomSpreadBottomLeftToTopRight.w;   // ��
                    fStretchArray[7] = _RandomSpreadTopLeftToBottomRight.w;  // �E��

                    // �Z�N�^�[�Ԃ��Ԃ��Ċg�U�������߂�
                    float fStretch = lerp(
                        fStretchArray[(int)fSectorIndex % 8],
                        fStretchArray[((int)fSectorIndex + 1) % 8],
                        fSectorLerp
                        );

                    // �ŏI�I�ȋ����ɔ��f
                    fDistance /= fStretch;
                }
                

                // �g�U�x�[�X�̃A���t�@
                float fBaseAlpha = smoothstep(fSpread, fSpread - _Cutoff, fDistance);

                // �t�̖͗l�e�N�X�`�����擾�i�K�v�Ȃ�e�N�X�`���ݒ�j
                float fNoise = SAMPLE_TEXTURE2D(_LiquidTex, sampler_LiquidTex, i.uv).r;

                // �t�̖͗l���|���Z
                float fAlpha = fBaseAlpha * fNoise;

                // �t�F�[�h�A�E�g����
                if (_FadeStartTime > 0)
                {
                    float fFadeProgress = saturate((_Time.y - _FadeStartTime) / _FadeDuration);
                    fAlpha *= (1.0 - fFadeProgress);
                }

                // ������������j��
                if (fAlpha <= 0.001)
                    discard;

                return float4(_BaseColor.rgb, fAlpha * _BaseColor.a);
            }
            ENDHLSL
        }
    }
}
