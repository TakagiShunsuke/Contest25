/*=====
<PanelBillboard.cs>
���쐬�ҁFtei

�����e
�p�l���̃r���{�[�h�ݒ�

�����ӎ���
�@�E���̃J�����ƍ��킹�č��܂������A���݂̃J�����͗v�C���Ǝv���܂��̂�
�@�@�J�������ύX������A�r���{�[�h�̐ݒ���v�C���B

���X�V����
__Y25
_M04
D
26�F�v���O�����쐬�Ftei

_M05
D
01�F�X�N���v�g���A�ϐ����C���Ftei
04�F�R�[�f�B���O���[���̉����ăR�[�h�C���Ftei
23�F�r���{�[�h�����C���Ftei

=====*/

// ���O��Ԑ錾
using UnityEngine;

// �N���X��`
public class CPanelBillboard : MonoBehaviour
{
    // ���X�V�֐�
    // �����F�Ȃ�
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�F�X�V����
    void Update()
    {
        if (Camera.main == null) return;

        // �J�����̌����Ă�������iY�������擾�j
        Vector3 camEuler = Camera.main.transform.rotation.eulerAngles;

        // XZ���ʂɒ���ꂽ�Ƃ��āA-90�x�Ő������\������Y�������Ǐ]
        transform.rotation = Quaternion.Euler(-90f, camEuler.y, 0f);
    }
}
