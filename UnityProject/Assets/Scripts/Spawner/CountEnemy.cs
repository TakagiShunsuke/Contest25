/*=====
<CountEnemy.cs>
���쐬�ҁFNishibu

�����e
// �G�̑������J�E���g���邽�߂̃N���X

���X�V����
__Y25 
_M05
D
5:CountEnemy�N���X����:nishibu
6:�C��:nishibu
7:�C���A�R�����g:nishibu
=====*/

// ���O��Ԑ錾
using UnityEngine;

// �N���X��`
public class CCountEnemy : MonoBehaviour
{
    // ���݃X�e�[�W��ɑ��݂��Ă���G�̐�
    public static uint m_nValInstances { get; private set; } = 0;

    // ���������ꏊ
    // �����F�Ȃ�
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�F�G�̃C���X�^���X����1���₷
    protected virtual void Start()
    {
        m_nValInstances++; // �G�̐���1���₷
    }

    // �����S���ꏊ
    // �����F�Ȃ�
    // ��
    // �ߒl�F�Ȃ�
    // ��
    // �T�v�F�G�̃C���X�^���X����1���炷
    protected virtual void OnDestroy()
    {
        m_nValInstances--; // �G�̐���1���炷
    }
}
