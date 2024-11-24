using System.Threading;
using Cysharp.Threading.Tasks;
using Interfaces;

namespace Jobs
{
    public abstract class Job : IJob
    {
        public int ExecutionOrder { get; }
        
        protected Job()
        {
        }
        
        
        public abstract UniTask ExecuteAsync(CancellationToken cancellationToken = default);
    }
}