
using DataTier;


namespace LogicTier
{
    public class ���������������
    {
        private ����� _�����;
        public ����� ����� => _�����;
        public ���������������(����� p)
        {
            _����� = p;
        }
        public string �������������������
        {
            get
            {
                return _�����.��� + ":" + _�����.������������
                    + " (" + _�����.����.ToString("C") + ")"
                    + " [������: " + _�����.���� + "]"; 
            }
        }
        public string ���������
        {
            get { return _�����.���; }
            set { _�����.������������ = value; }
        }
        public string ������������������
        {
            get { return _�����.������������; }
            set { _�����.������������ = value; }
        }
        public string ����
        {
            get { return _�����.����; }
            set { _�����.���� = value; }
        }
        public float ����������
        {
            get { return _�����.����; }
            set { _�����.���� = value; }
        }
        public int ����������������
        {
            get { return _�����.����������; }
            set { _�����.���������� = value; }
        }
        public float �������������������������
        {
            get { return _�����.���� * _�����.����������; }
        }
        
        
    }
}