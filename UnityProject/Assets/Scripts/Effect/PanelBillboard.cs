/*=====
<CPanelBillboard.cs>
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

=====*/

using UnityEngine;

public class CPanelBillboard : MonoBehaviour
{
    // �ϐ��錾
    [SerializeField,Tooltip("�J�����Ƃ̌Œ苗��")] private float m_fDistanceFromCamera = 5f;

    // ���X�V�֐�
    // �����F�Ȃ�
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�F�X�V����
    void Update()
    {
        if (Camera.main == null) return;

        // �J�����̈ʒu�ƌ������擾
        Vector3 CameraPosition = Camera.main.transform.position;
        Vector3 CameraForward = Camera.main.transform.forward;

        // �J�����̑O��distance�������ꂽ�ꏊ�Ƀp�l�����ړ�
        transform.position = CameraPosition + CameraForward * m_fDistanceFromCamera;

        // �J������Y����]�������
        Vector3 Euler = Camera.main.transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(-90f, Euler.y, 0f);
    }
}
