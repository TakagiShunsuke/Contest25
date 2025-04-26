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

=====*/

using UnityEngine;

public class CPanelBillboard : MonoBehaviour
{
    // �ϐ��錾
    [SerializeField,Tooltip("�J�����Ƃ̌Œ苗��")] private float fDistanceFromCamera = 5f;

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
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 cameraForward = Camera.main.transform.forward;

        // �J�����̑O��distance�������ꂽ�ꏊ�Ƀp�l�����ړ�
        transform.position = cameraPosition + cameraForward * fDistanceFromCamera;

        // �J������Y����]�������
        Vector3 euler = Camera.main.transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(-90f, euler.y, 0f);
    }
}
