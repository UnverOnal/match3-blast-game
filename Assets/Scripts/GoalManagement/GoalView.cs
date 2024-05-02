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

        private readonly Dictionary<Goal, GoalPrefabResources> _counts;

        private readonly GameObject _goalRootParent;

        public GoalView(BoardCreationData creationData, GoalResources goalResources, IPoolService poolService)
        {
            _creationData = creationData;
            var contentParent = goalResources.contentParent;
            _goalRootParent = goalResources.goalRootParent;

            var goalPrefab = creationData.goalPrefab;
            _goalGameObjectPool = poolService.GetPoolFactory()
                .CreatePool(() => Object.Instantiate(goalPrefab, contentParent));

            _counts = new Dictionary<Goal, GoalPrefabResources>();
        }

        public void CreateMove(Goal goal)
        {
            if (!_goalRootParent.activeSelf)
                _goalRootParent.SetActive(true);

            var exist = _counts.TryGetValue(goal, out var resources);
            if (!exist)
            {
                var goalGameObject = _goalGameObjectPool.Get();
                var text = goalGameObject.GetComponentInChildren<TextMeshProUGUI>();
                var image = goalGameObject.GetComponentInChildren<Image>();
                
                resources = new GoalPrefabResources(image, text);
                _counts.Add(goal, resources);
            }
            
            SetGoal(goal, resources);
        }

        public void UpdateMove(Goal goal)
        {
            if (_counts.TryGetValue(goal, out var resources))
                SetCount(resources.text, goal);
        }

        public void Reset()
        {
            _goalRootParent.SetActive(false);
        }

        private void SetGoal(Goal goal, GoalPrefabResources resources)
        {
            resources.image.sprite = GetSprite(goal.cellType);
            SetCount(resources.text, goal);
        }

        private void SetCount(TextMeshProUGUI text, Goal goal)
        {
            var count = goal.Target - goal.CurrentCount;
            text.text = count.ToString();
        }

        private Sprite GetSprite(CellType cellType)
        {
            var goalAssetData = _creationData.goalAssetData;
            for (var i = 0; i < goalAssetData.Length; i++)
            {
                var goalAsset = goalAssetData[i];
                if (cellType == goalAsset.cellType)
                    return goalAsset.sprite;
            }

            return null;
        }
    }

    public struct GoalPrefabResources
    {
        public Image image;
        public TextMeshProUGUI text;

        public GoalPrefabResources(Image image, TextMeshProUGUI text)
        {
            this.image = image;
            this.text = text;
        }
    }
}