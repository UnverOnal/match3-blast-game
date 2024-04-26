using System.Collections.Generic;
using System.Text.RegularExpressions;
using Board.BoardCreation;
using Board.CellManagement;
using Services.PoolingService;
using UnityEngine;
using VContainer;

namespace Board
{
    public class BoardPresenter
    {
        [Inject] private BoardModel _boardModel;
        private readonly BoardView _boardView;

        private readonly ObjectPool<CellGroup> _groupPool;

        [Inject]
        public BoardPresenter(BlockCreator blockCreator, IPoolService poolService)
        {
            _boardView = new BoardView(blockCreator);
            _groupPool = poolService.GetPoolFactory().CreatePool(() => new CellGroup());
        }

        public void OnBlockSelected(GameObject cellGameObject)
        {
            GetGroups();
            var group = _boardModel.GetGroup(cellGameObject);
            Merge(group);
            _boardView.Collapse();
            _boardView.Fill();
            _boardModel.Update();
        }

        private void GetGroups()
        {
            
        }

        private void Merge(CellGroup group)
        {
            _boardView.Merge(group);
            group.Reset();
            _groupPool.Return(group);
        }

        private void Shuffle()
        {
            
        }
    }
}
