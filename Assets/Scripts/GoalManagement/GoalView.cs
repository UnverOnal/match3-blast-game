using System.Collections.Generic;
using GamePlay.CellManagement;
using Level.LevelCreation;
using Services.PoolingService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GoalManagement
{
    public class GoalView
    {
        private readonly BoardCreationData _creationData;

        private readonly ObjectPool<GameObject> _goalGameObjectPool;

        private readonly Dictionary<Goal, TextMeshProUGUI> _counts;

        private readonly GameObject _goalRootParent;

        public GoalView(BoardCreationData creationData, GoalResources goalResources, IPoolService poolService)
        {
            _creationData = creationData;
            var contentParent = goalResources.contentParent;
            _goalRootParent = goalResources.goalRootParent;

            var goalPrefab = creationData.goalPrefab;
            _goalGameObjectPool = poolService.GetPoolFactory()
                .CreatePool(() => Object.Instantiate(goalPrefab, parent: contentParent));

            _counts = new Dictionary<Goal, TextMeshProUGUI>();
        }

        public void CreateMove(Goal goal)
        {
            if(!_goalRootParent.activeSelf)
                _goalRootParent.SetActive(true);
            
            if (_counts.TryGetValue(goal, out var text))
                SetCount(text, goal);
            else
            {
                var goalGameObject = _goalGameObjectPool.Get();
                text = goalGameObject.GetComponentInChildren<TextMeshProUGUI>();
                var image = goalGameObject.GetComponentInChildren<Image>();
                SetGoal(goal, image, text);
                _counts.Add(goal, text);
            }
        }

        public void UpdateMove(Goal goal)
        {
            if (_counts.TryGetValue(goal, out var text))
                SetCount(text, goal);
        }

        public void Reset()
        {
            _goalRootParent.SetActive(false);
        }

        private void SetGoal(Goal goal, Image image, TextMeshProUGUI text)
        {
            image.sprite = GetSprite(goal.cellType);
            SetCount(text, goal);
        }

        private void SetCount(TextMeshProUGUI text,  Goal goal)
        {
            var count = goal.Target - goal.CurrentCount;
            text.text = count.ToString();
        }

        private Sprite GetSprite(CellType cellType)
        {
            var goalAssetData = _creationData.goalAssetData;
            for (int i = 0; i < goalAssetData.Length; i++)
            {
                var goalAsset = goalAssetData[i];
                if (cellType == goalAsset.cellType)
                    return goalAsset.sprite;
            }

            return null;
        }
    }
}
