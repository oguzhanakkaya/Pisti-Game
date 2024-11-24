using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Interfaces;
using UnityEngine;

namespace Jobs
{
    public class MoveGainedCardJob : Job
    {
        private const float _moveTime = 0.2f;
        
        private readonly IEnumerable<CardObject> _cardObjects;
        private readonly IPlayer _player;


        public MoveGainedCardJob(List<CardObject> cardObjects, IPlayer player)
        {
            _cardObjects = cardObjects;
            _player = player;
        }
        public override async UniTask ExecuteAsync(CancellationToken cancellationToken = default)
        {
            await UniTask.Delay(500);
            
            List<UniTask> moveTasks = new List<UniTask>();

            foreach (var obj in _cardObjects)
            {
                moveTasks.Add(MoveObjectToPositionAsync(obj.transform, _player.Transform.position));
            }
            
            await UniTask.WhenAll(moveTasks);
        }
        
        private async UniTask MoveObjectToPositionAsync(Transform obj, Vector3 target)
        {
            await obj.DOMove(target, _moveTime).SetEase(Ease.Linear);
            obj.gameObject.SetActive(false);
        }
    }
}