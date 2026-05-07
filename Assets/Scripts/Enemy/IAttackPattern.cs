using UnityEngine;
public interface IAttackPattern
{
    void Enter(BossController boss);     // Setup
    void Execute();                      // Called every frame
    void Exit();                         // Cleanup
    bool IsFinished { get; }             // If pattern is done
}