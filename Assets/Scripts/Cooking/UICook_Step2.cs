using System.Collections.Generic;
using UnityEngine;

namespace Cooking
{
    public class UICook_Step2 : UICookStep
    {
        [SerializeField] private UIMaterialItemSlot _slotPrefab;

        [SerializeField] private Transform _slotParent;

        [SerializeField] private UICookInventory _uiCookInventory;

        private List<UIMaterialItemSlot> _materialItemSlots = new List<UIMaterialItemSlot>();

        private int _selectMaterialSlotIndex = 0;

        private int _slotMaxCount => _uiCook.MaterialSlotMaxCount;

        private int _slotCount => _uiCook.MaterialSlotCount;


        public override void Init(UICook uiCook)
        {
            base.Init(uiCook);

            for (int i = 0, count = _slotMaxCount; i < count; i++)
            {
                UIMaterialItemSlot slot = Instantiate(_slotPrefab);
                slot.transform.parent = _slotParent;
                slot.transform.localScale = Vector3.one;

                int index = i;
                slot.Init(index, OnMaterialButtonClicked);

                _materialItemSlots.Add(slot);
            }

            OnMaterialButtonClicked(0);
            _uiCookInventory.Init(ChoiceItem);
            _cookSystem.OnCookStarted += () => _uiCookInventory.UpdateUI();
        }


        public override void StartStep()
        {
            gameObject.SetActive(true);

            for (int i = 0, count = _slotMaxCount; i < count; i++)
            {
                _materialItemSlots[i].ChoiceItem(null);

                if (i < _slotCount)
                    _materialItemSlots[i].SetActiveSlot(true);

                else
                    _materialItemSlots[i].SetActiveSlot(false);
            }

            _uiCook.CookStepSlot.DisableRightButton();
            OnMaterialButtonClicked(0);
        }


        public override void StopStep()
        {
            _cookSystem.SetCurrentRecipe(_materialItemSlots[0].CurrentItem, _materialItemSlots[1].CurrentItem);
            gameObject.SetActive(false);
        }


        private void OnMaterialButtonClicked(int index)
        {

            //���� ���� ������ �ѹ��� Ŭ���ߴٸ�
            if (index == _selectMaterialSlotIndex)
            {
                _materialItemSlots[_selectMaterialSlotIndex].ChoiceItem(null);
                CheckMaterialItemSlot();
            }

            for (int i = 0, count = _slotMaxCount; i < count; i++)
            {
                _materialItemSlots[i].DisableSelectSlot();
            }

            _materialItemSlots[index].EnableSelectSlot();
            _selectMaterialSlotIndex = index;
        }


        public void ChoiceItem(InventoryItem item)
        {
            _materialItemSlots[_selectMaterialSlotIndex].ChoiceItem(item);
            CheckMaterialItemSlot();
            if (item == null)
                return;

            //�������� null���� �ƴ϶�� ���� �������� ���ñ��� �ѱ��.
            _selectMaterialSlotIndex++;
            _selectMaterialSlotIndex = Mathf.Clamp(_selectMaterialSlotIndex, 0, _slotCount - 1);

            for (int i = 0, count = _slotMaxCount; i < count; i++)
            {
                _materialItemSlots[i].DisableSelectSlot();
            }

            _materialItemSlots[_selectMaterialSlotIndex].EnableSelectSlot();
        }


        private void CheckMaterialItemSlot()
        {
            bool enableCheck = _materialItemSlots[0].CurrentItem != null || _materialItemSlots[1].CurrentItem != null;

            if (_materialItemSlots[0].CurrentItem != null && _materialItemSlots[1].CurrentItem != null)
            {
                //���� 2���� ���� �������� ���ְ� ������ ī��Ʈ�� 2 �̻��̸� Ȱ��ȭ
                bool check = _materialItemSlots[0].CurrentItem == _materialItemSlots[1].CurrentItem;

                if (check)
                {
                    if (2 <= _materialItemSlots[0].CurrentItem.Count)
                    {
                        _uiCook.CookStepSlot.EnableRightButton();
                    }
                    else
                    {
                        _uiCook.CookStepSlot.DisableRightButton();
                    }
                }
                else
                {
                    _uiCook.CookStepSlot.EnableRightButton();
                }
            }


            else if (_uiCook.MaterialSlotCount == 2 && (_materialItemSlots[0].CurrentItem == null || _materialItemSlots[1].CurrentItem == null))
            {
                Debug.Log("2�� ¥������ 1�� �丮 �Ұ���");
                _uiCook.CookStepSlot.DisableRightButton();
            }

            else if (_materialItemSlots[0].CurrentItem != null || _materialItemSlots[1].CurrentItem != null)
            {

                _uiCook.CookStepSlot.EnableRightButton();
            }
            else
            {
                _uiCook.CookStepSlot.DisableRightButton();
            }
        }
    }
}

