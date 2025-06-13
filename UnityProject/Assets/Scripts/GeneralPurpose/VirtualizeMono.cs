/*=====
<VirtualizeMono.cs>
���쐬�ҁFtakagi

�����e
MonoBehavior�̃C�x���g�֐������z��(sealed�C���q�p)

�����ӎ���
�EMonoBehavior�̊e�C�x���g�֐���sealed���邱�Ƃ��ł���悤�ɂȂ�܂�
	���������͖h�����������̃^�C�~���O�Œǉ��ŏ������������̂�����΂��ꂼ�ꂱ���ŉ��z�I�ɒ�`����Ă���֐�(Custom~~()�֐�)���g���̂������Ǝv���܂��B
	�Ȃ��A�����Œ�`����Ă���C�x���g�łȂ��֐��͒�`�݂̂ł���A�����܂Ŋe�q�N���X�Œ�`���Ȃ�����Ԃ��Ȃ��Ă���ɂ����܂���B
	�Ăяo���͂���Ă��Ȃ��̂�sealed���`����ۂɌĂяo���Ă�������(override�ŏ㏑�����Ă���̂œ�����O�ł���)
�E�܂��Aprotected�̏C���q�͕ύX�������܂���	�F	�����������Ȃ���΂Ȃ�Ȃ��ꍇ�͂��̃N���X�̎g�p�͂��T�����������B
	(public���Ȃ��̂̓C�x���g�֐����C�x���g�炵���������邽�߁Aprivate���Ȃ��̂̓I�[�o�[���[�h��h�����߂ł�)

���X�V����
__Y24
_M06
D
06:�v���O�����쐬:takagi
21:���t�@�N�^�����O:takagi
__Y25
_M06
D
14:���t�@�N�^�����O:takagi
=====*/

// ���O��Ԑ錾
using UnityEngine;

// �N���X��`
public class CVirtualizeMono : MonoBehaviour
{
	/// <summary>
	/// -�������֐�
	/// <para>�C���X�^���X��������ɍs������</para>
	/// </summary>
	virtual protected void Awake() {}

	/// <summary>
	/// -Awake()�֐��J�X�^���p
	/// <para>Awake()�֐��ŔC�ӂɏ�����ǉ����₷���悤�ɒ�`</para>
	/// </summary>
	virtual protected void CustomAwake() {}

	/// <summary>
	/// -�������֐�
	/// <para>����X�V���O�ɍs������</para>
	/// </summary>
	virtual protected void Start() {}

	/// <summary>
	/// -Start()�֐��J�X�^���p
	/// <para>Start()�֐��ŔC�ӂɏ�����ǉ����₷���悤�ɒ�`</para>
	/// </summary>
	virtual protected void CustomStart() {}

	/// <summary>
	/// -�������֐�
	/// <para>���g�̃I�u�W�F�N�g���L���ɂȂ����u�Ԃɍs������</para>
	/// </summary>
	virtual protected void OnEnable() {}

	/// <summary>
	/// -OnEnable()�֐��J�X�^���p
	/// <para>OnEnable()�֐��ŔC�ӂɏ�����ǉ����₷���悤�ɒ�`</para>
	/// </summary>
	virtual protected void CustomOnEnable() {}

	/// <summary>
	/// -�X�V�֐�
	/// <para>��莞�Ԃ��Ƃɍs���X�V����</para>
	/// </summary>
	virtual protected void Update() {}

	/// <summary>
	/// -Update()�֐��J�X�^���p
	/// <para>Update()�֐��ŔC�ӂɏ�����ǉ����₷���悤�ɒ�`</para>
	/// </summary>
	virtual protected void CustomUpdate() {}

	/// <summary>
	/// -�x�����X�V�֐�
	/// <para>�e�t���[���ɂ����Ēʏ��Update�֐��̌�ɍs���X�V����</para>
	/// </summary>
	virtual protected void LateUpdate() {}

	/// <summary>
	/// -LateUpdate()�֐��J�X�^���p
	/// <para>LateUpdate()�֐��ŔC�ӂɏ�����ǉ����₷���悤�ɒ�`</para>
	/// </summary>
	virtual protected void CustomLateUpdate() {}

	/// <summary>
	/// -�����X�V�֐�
	/// <para>��莞�Ԃ��Ƃɍs�������I�ȍX�V����</para>
	/// </summary>
	virtual protected void FixedUpdate() {}

	/// <summary>
	/// -FixedUpdate()�֐��J�X�^���p
	/// <para>FixedUpdate()�֐��ŔC�ӂɏ�����ǉ����₷���悤�ɒ�`</para>
	/// </summary>
	virtual protected void CustomFixedUpdate() {}

	/// <summary>
	/// -GUI�X�V�֐�
	/// <para>GUI�`��p�ɍ��p�x�ōs����X�V����</para>
	/// </summary>
	virtual protected void OnGUI() {}

	/// <summary>
	/// -OnGUI()�֐��J�X�^���p
	/// <para>OnGUI()�֐��ŔC�ӂɏ�����ǉ����₷���悤�ɒ�`</para>
	/// </summary>
	virtual protected void CustomOnGUI() {}

	/// <summary>
	/// -�j���֐��J�X�^���p
	/// <para>�C���X�^���X�j�����ɍs������</para>
	/// </summary>
	virtual protected void OnDestroy() {}

	/// <summary>
	/// -OnDestroy()�֐��J�X�^���p
	/// <para>OnDestroy()�֐��ŔC�ӂɏ�����ǉ����₷���悤�ɒ�`</para>
	/// </summary>
	virtual protected void CustomOnDestroy() {}

	/// <summary>
	/// -�������֐�
	/// <para>���g�̃I�u�W�F�N�g�������ɂȂ����u�Ԃɍs������</para>
	/// </summary>
	virtual protected void OnDisable() {}

	/// <summary>
	/// -OnDisable()�֐��J�X�^���p
	/// <para>OnDisable()�֐��ŔC�ӂɏ�����ǉ����₷���悤�ɒ�`</para>
	/// </summary>
	virtual protected void CustomOnDisable() {}

	/// <summary>
	/// -��`�掞�֐�
	/// <para>�C�ӂ̃J�����ɉf��n�߂��u�Ԃɍs������</para>
	/// </summary>
	virtual protected void OnBecameVisible() {}

	/// <summary>
	/// -OnBecameVisible()�֐��J�X�^���p
	/// <para>OnBecameVisible()�֐��ŔC�ӂɏ�����ǉ����₷���悤�ɒ�`</para>
	/// </summary>
	virtual protected void CustomOnBecameVisible() {}

	/// <summary>
	/// -�V���C���֐�
	/// <para>�C�ӂ̃J�����ɉf��Ȃ��Ȃ����u�Ԃɍs������</para>
	/// </summary>
	virtual protected void OnBecameInvisible() {}

	/// <summary>
	/// -OnBecameInvisible()�֐��J�X�^���p
	/// <para>OnBecameInvisible()�֐��ŔC�ӂɏ�����ǉ����₷���悤�ɒ�`</para>
	/// </summary>
	virtual protected void CustomOnBecameInvisible() {}

	/// <summary>
	/// -�`�撆�֐�
	/// <para>�f���Ă���ԃJ�������ƂɌĂяo����鏈��(IsTrigger off��)</para>
	/// </summary>
	virtual protected void OnWillRenderObject() {}

	/// <summary>
	/// -OnWillRenderObject()�֐��J�X�^���p
	/// <para>OnWillRenderObject()�֐��ŔC�ӂɏ�����ǉ����₷���悤�ɒ�`</para>
	/// </summary>
	virtual protected void CustomOnWillRenderObject() {}

	/// <summary>
	/// -�ڐG���֐�3D
	/// <para>3D��ԏ�ŐڐG�̓����蔻�肪���ꂽ�u�Ԃɍs������(IsTrigger off��)</para>
	/// </summary>
	/// <param name="_Collision">�ڐG����</param>
	virtual protected void OnCollisionEnter(Collision _Collision) {}

	/// <summary>
	/// -OnCollisionEnter()�֐��J�X�^���p
	/// <para>OnCollisionEnter()�֐��ŔC�ӂɏ�����ǉ����₷���悤�ɒ�`</para>
	/// </summary>
	/// <param name="_Collision">�ڐG����</param>
	virtual protected void CustomOnCollisionEnter(Collision _Collision) {}

	/// <summary>
	/// -�ڐG���֐�2D
	/// <para>2D��ԏ�ŐڐG�̓����蔻�肪���ꂽ�u�Ԃɍs������(IsTrigger off��)</para>
	/// </summary>
	/// <param name="_Collision">�ڐG����</param>
	virtual protected void OnCollisionEnter2D(Collision2D _Collision) {}

	/// <summary>
	/// -OnCollisionEnter2D()�֐��J�X�^���p
	/// <para>OnCollisionEnter2D()�֐��ŔC�ӂɏ�����ǉ����₷���悤�ɒ�`</para>
	/// </summary>
	/// <param name="_Collision">�ڐG����</param>
	virtual protected void CustomOnCollisionEnter2D(Collision2D _Collision) {}

	/// <summary>
	/// -�ڐG���֐�3D
	/// <para>3D��ԏ�ŐڐG�̓����蔻�肪���ꂽ�u�Ԃɍs������(IsTrigger on��)</para>
	/// </summary>
	/// <param name="_Collider">�ڐG����</param>
	virtual protected void OnTriggerEnter(Collider _Collider) {}

	/// <summary>
	/// -OnTriggerEnter()�֐��J�X�^���p
	/// <para>OnTriggerEnter()�֐��ŔC�ӂɏ�����ǉ����₷���悤�ɒ�`</para>
	/// </summary>
	/// <param name="_Collider">�ڐG����</param>
	virtual protected void CustomOnTriggerEnter(Collider _Collider) {}

	/// <summary>
	/// -�ڐG���֐�2D
	/// <para>2D��ԏ�ŐڐG�̓����蔻�肪���ꂽ�u�Ԃɍs������(IsTrigger on��)</para>
	/// </summary>
	/// <param name="_Collider">�ڐG����</param>
	virtual protected void OnTriggerEnter2D(Collider2D _Collider) {}

	/// <summary>
	/// -OnTriggerEnter2D()�֐��J�X�^���p
	/// <para>OnTriggerEnter2D()�֐��ŔC�ӂɏ�����ǉ����₷���悤�ɒ�`</para>
	/// </summary>
	/// <param name="_Collider">�ڐG����</param>
	virtual protected void CustomOnTriggerEnter2D(Collider2D _Collider) {}

	/// <summary>
	/// -�ڐG���֐�3D
	/// <para>3D��ԏ�ŐڐG�̓����蔻�肪����Ă���ԍs������(IsTrigger off��)</para>
	/// </summary>
	/// <param name="_Collision">�ڐG����</param>
	virtual protected void OnCollisionStay(Collision _Collision) {}

	/// <summary>
	/// -OnCollisionStay()�֐��J�X�^���p
	/// <para>OnCollisionStay()�֐��ŔC�ӂɏ�����ǉ����₷���悤�ɒ�`</para>
	/// </summary>
	/// <param name="_Collision">�ڐG����</param>
	virtual protected void CustomOnCollisionStay(Collision _Collision) {}

	/// <summary>
	/// -�ڐG���֐�2D
	/// <para>2D��ԏ�ŐڐG�̓����蔻�肪����Ă���ԍs������(IsTrigger off��)</para>
	/// </summary>
	/// <param name="_Collision">�ڐG����</param>
	virtual protected void OnCollisionStay2D(Collision2D _Collision) {}

	/// <summary>
	/// -OnCollisionStay2D()�֐��J�X�^���p
	/// <para>OnCollisionStay2D()�֐��ŔC�ӂɏ�����ǉ����₷���悤�ɒ�`</para>
	/// </summary>
	/// <param name="_Collision">�ڐG����</param>
	virtual protected void CustomOnCollisionStay2D(Collision2D _Collision) {}

	/// <summary>
	/// -�ڐG���֐�3D
	/// <para>3D��ԏ�ŐڐG�̓����蔻�肪����Ă���ԍs������(IsTrigger on��)</para>
	/// </summary>
	/// <param name="_Collider">�ڐG����</param>
	virtual protected void OnTriggerStay(Collider _Collider) {}

	/// <summary>
	/// -OnTriggerStay()�֐��J�X�^���p
	/// <para>OnTriggerStay()�֐��ŔC�ӂɏ�����ǉ����₷���悤�ɒ�`</para>
	/// </summary>
	/// <param name="_Collider">�ڐG����</param>
	virtual protected void CustomOnTriggerStay(Collider _Collider) {}

	/// <summary>
	/// -�ڐG���֐�2D
	/// <para>2D��ԏ�ŐڐG�̓����蔻�肪����Ă���ԍs������(IsTrigger on��)</para>
	/// </summary>
	/// <param name="_Collider">�ڐG����</param>
	virtual protected void OnTriggerStay2D(Collider2D _Collider) {}

	/// <summary>
	/// -OnTriggerStay2D()�֐��J�X�^���p
	/// <para>OnTriggerStay2D()�֐��ŔC�ӂɏ�����ǉ����₷���悤�ɒ�`</para>
	/// </summary>
	/// <param name="_Collider">�ڐG����</param>
	virtual protected void CustomOnTriggerStay2D(Collider2D _Collider) {}

	/// <summary>
	/// -�������֐�3D
	/// <para>3D��ԏ�ŐڐG���Ă������̂Ɨ��ꂽ�u�Ԃɍs������(IsTrigger off��)</para>
	/// </summary>
	/// <param name="_Collision">�ڐG����</param>
	virtual protected void OnCollisionExit(Collision _Collision) {}

	/// <summary>
	/// -OnCollisionExit()�֐��J�X�^���p
	/// <para>OnCollisionExit()�֐��ŔC�ӂɏ�����ǉ����₷���悤�ɒ�`</para>
	/// </summary>
	/// <param name="_Collision">�ڐG����</param>
	virtual protected void CustomOnCollisionExit(Collision _Collision) {}

	/// <summary>
	/// -�������֐�2D
	/// <para>2D��ԏ�ŐڐG���Ă������̂Ɨ��ꂽ�u�Ԃɍs������(IsTrigger off��)</para>
	/// </summary>
	/// <param name="_Collision">�ڐG����</param>
	virtual protected void OnCollisionExit2D(Collision2D _Collision) {}

	/// <summary>
	/// -OnCollisionExit2D()�֐��J�X�^���p
	/// <para>OnCollisionExit2D()�֐��ŔC�ӂɏ�����ǉ����₷���悤�ɒ�`</para>
	/// </summary>
	/// <param name="_Collision">�ڐG����</param>
	virtual protected void CustomOnCollisionExit2D(Collision2D _Collision) {}

	/// <summary>
	/// -�������֐�3D
	/// <para>3D��ԏ�ŐڐG���Ă������̂Ɨ��ꂽ�u�Ԃɍs������(IsTrigger on��)</para>
	/// </summary>
	/// <param name="_Collider">�ڐG����</param>
	virtual protected void OnTriggerExit(Collider _Collider) {}

	/// <summary>
	/// -OnTriggerExit()�֐��J�X�^���p
	/// <para>OnTriggerExit()�֐��ŔC�ӂɏ�����ǉ����₷���悤�ɒ�`</para>
	/// </summary>
	/// <param name="_Collider">�ڐG����</param>
	virtual protected void CustomOnTriggerExit(Collider _Collider) {}

	/// <summary>
	/// -�������֐�2D
	/// <para>2D��ԏ�ŐڐG���Ă������̂Ɨ��ꂽ�u�Ԃɍs������(IsTrigger on��)</para>
	/// </summary>
	/// <param name="_Collider">�ڐG����</param>
	virtual protected void OnTriggerExit2D(Collider2D _Collider) {}

	/// <summary>
	/// -OnTriggerExit2D()�֐��J�X�^���p
	/// <para>OnTriggerExit2D()�֐��ŔC�ӂɏ�����ǉ����₷���悤�ɒ�`</para>
	/// </summary>
	/// <param name="_Collider">�ڐG����</param>
	virtual protected void CustomOnTriggerExit2D(Collider2D _Collider) {}

	/// <summary>
	/// -�p�[�e�B�N���ڐG���֐�
	/// <para>�p�[�e�B�N�����R���C�_�[�ƐڐG���Ă���ԍs������(SendCollisionMessage on��)</para>
	/// </summary>
	/// <param name="_GameObject">���������I�u�W�F�N�g�̏��</param>
	virtual protected void OnParticleCollision(GameObject _GameObject) {}

	/// <summary>
	/// -OnParticleCollision()�֐��J�X�^���p
	/// <para>OnParticleCollision()�֐��ŔC�ӂɏ�����ǉ����₷���悤�ɒ�`</para>
	/// </summary>
	/// <param name="_GameObject">���������I�u�W�F�N�g�̏��</param>
	virtual protected void CustomOnParticleCollision(GameObject _GameObject) {}

	/// <summary>
	/// -�p�[�e�B�N���g���K�[����֐�
	/// <para>ParticleSystem��Triggers���W���[���𓋍ڂ��Ă���ԌĂяo����鏈��</para>
	/// </summary>
	virtual protected void OnParticleTrigger() {}

	/// <summary>
	/// -OnParticleTrigger()�֐��J�X�^���p
	/// <para>OnParticleTrigger()�֐��ŔC�ӂɏ�����ǉ����₷���悤�ɒ�`</para>
	/// </summary>
	virtual protected void CustomOnParticleTrigger() {}
}